/**
 * Shared code for the web and mobile clients.
 * Business rules live in the API; this package only holds types, validation and helpers.
 */

/** Nominal ("branded") id type so ids of different entities cannot be mixed up. */
export type Brand<T, B extends string> = T & { readonly __brand: B };

/** Identifies a person whose health is recorded (ADR-0006). */
export type ProfileId = Brand<string, "ProfileId">;
