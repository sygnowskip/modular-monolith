// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static class ModularMonolith
        {
            public static string ClientId = "modular-monolith-client";
            public static string ClientSecret = "modular-monolith-client-secret".Sha256();

            public static class Scopes
            {
                public static string Registrations = "registrations";
                public static string Payments = "payments";
            }
        }

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> Scopes =>
            new ApiScope[]
            {
                new ApiScope(ModularMonolith.Scopes.Payments),
                new ApiScope(ModularMonolith.Scopes.Registrations)
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("modular-monolith")
                {
                    DisplayName = "Modular Monolith",
                    Scopes = new List<string>()
                    {
                        ModularMonolith.Scopes.Payments,
                        ModularMonolith.Scopes.Registrations
                    }
                },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientId = ModularMonolith.ClientId,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret(ModularMonolith.ClientSecret)
                    },
                    AllowedScopes =
                    {
                        ModularMonolith.Scopes.Registrations,
                        ModularMonolith.Scopes.Payments
                    }
                }
            };
    }
}