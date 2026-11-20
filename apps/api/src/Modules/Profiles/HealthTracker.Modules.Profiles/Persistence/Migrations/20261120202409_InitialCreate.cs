using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTracker.Modules.Profiles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The module owns its schema from the first migration; tables arrive with #7.
            migrationBuilder.EnsureSchema(name: "profiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
