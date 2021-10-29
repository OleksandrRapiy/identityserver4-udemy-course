using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServerCourse.IdentityServer.Configurations
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("customerAPI", "Customer API resources")
            };
        }

        public static IEnumerable<Client> GetAllClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "customerAPI" }
                }
            };
        }

        public static IEnumerable<ApiScope> GetAllApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("customerAPI")
            };
        }
    }

}
