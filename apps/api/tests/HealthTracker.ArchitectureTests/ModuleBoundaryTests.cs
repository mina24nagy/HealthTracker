using System.Reflection;
using HealthTracker.SharedKernel;
using HealthTracker.SharedKernel.Modules;
using NetArchTest.Rules;
using TestResult = NetArchTest.Rules.TestResult;

namespace HealthTracker.ArchitectureTests;

/// <summary>
/// Enforces the modular-monolith boundaries from ADR-0002 so they hold by test, not by convention:
/// <list type="bullet">
///   <item>SharedKernel depends on nothing project-specific.</item>
///   <item>Modules never depend on the host or on another module, except through a module's <c>.Contracts</c> namespace.</item>
///   <item>A module's public surface is only its <see cref="IModule"/> entry point and its contracts.</item>
/// </list>
/// </summary>
public sealed class ModuleBoundaryTests
{
    private const string ModulesNamespace = "HealthTracker.Modules";
    private const string HostNamespace = "HealthTracker.Api";

    private static readonly Assembly SharedKernel = typeof(ProfileId).Assembly;
    private static readonly Assembly Host = Assembly.Load(HostNamespace);

    /// <summary>Every module the host composes (discovered, so new modules are covered automatically).</summary>
    private static readonly Assembly[] Modules =
    [
        .. Host.GetReferencedAssemblies()
            .Where(name => name.Name!.StartsWith(ModulesNamespace + ".", StringComparison.Ordinal))
            .Select(Assembly.Load),
    ];

    [Fact]
    public void Host_composes_at_least_one_module() => Assert.NotEmpty(Modules);

    [Fact]
    public void SharedKernel_does_not_depend_on_modules_or_host()
    {
        var result = Types.InAssembly(SharedKernel)
            .ShouldNot().HaveDependencyOnAny(ModulesNamespace, HostNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Modules_do_not_depend_on_host()
    {
        foreach (var module in Modules)
        {
            var result = Types.InAssembly(module).ShouldNot().HaveDependencyOn(HostNamespace).GetResult();
            Assert.True(result.IsSuccessful, $"{module.GetName().Name}: {Describe(result)}");
        }
    }

    [Fact]
    public void Modules_only_use_other_modules_through_their_contracts()
    {
        var violations =
            from consumer in Modules
            from provider in Modules
            where consumer != provider
            from type in TypesUsingInternalsOf(consumer, provider)
            select $"{type} -> {provider.GetName().Name}";

        Assert.Empty(violations);
    }

    [Fact]
    public void Module_public_surface_is_entry_point_and_contracts_only()
    {
        var leaked =
            from module in Modules
            from type in module.GetExportedTypes()
            where !typeof(IModule).IsAssignableFrom(type) && !IsContract(type)
            select type.FullName;

        Assert.Empty(leaked);
    }

    /// <summary>
    /// Guards the guard: the host does reference a module's internals (its IModule class),
    /// so the detector must report it. If this fails, the boundary rule above would pass vacuously.
    /// </summary>
    [Fact]
    public void Boundary_detector_reports_a_known_dependency()
    {
        var module = Assert.Single(Modules, m => m.GetName().Name == "HealthTracker.Modules.Profiles");

        Assert.NotEmpty(TypesUsingInternalsOf(Host, module));
    }

    private static IReadOnlyList<string> TypesUsingInternalsOf(Assembly consumer, Assembly provider)
    {
        // Full type names (not namespaces) so that "X.Contracts.*" types stay allowed.
        string[] internalTypes =
        [
            .. provider.GetTypes()
                .Where(t => !t.IsNested && t.FullName is not null && !t.FullName.Contains('<', StringComparison.Ordinal) && !IsContract(t))
                .Select(t => t.FullName!),
        ];

        if (internalTypes.Length == 0)
        {
            return [];
        }

        var result = Types.InAssembly(consumer).ShouldNot().HaveDependencyOnAny(internalTypes).GetResult();
        return result.IsSuccessful ? [] : [.. result.FailingTypeNames ?? []];
    }

    private static bool IsContract(Type type) =>
        type.Namespace?.Split('.').Contains("Contracts") ?? false;

    private static string Describe(TestResult result) =>
        result.IsSuccessful ? string.Empty : "Violations: " + string.Join(", ", result.FailingTypeNames ?? []);
}
