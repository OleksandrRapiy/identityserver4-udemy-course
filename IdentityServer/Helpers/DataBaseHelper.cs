using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerCourse.IdentityServer.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerCourse.IdentityServer.Helpers
{
    public static class DataBaseHelper
    {
        public static void InitDataBase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                
                context.Database.Migrate();
                //context.Clients.RemoveRange(context.Clients);
                context.SaveChanges();

                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityConfiguration.GetAllClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityConfiguration.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var apiResource in IdentityConfiguration.GetAllApiResources())
                    {
                        context.ApiResources.Add(apiResource.ToEntity());
                    }
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var apiScope in IdentityConfiguration.GetAllApiScopes())
                    {
                        context.ApiScopes.Add(apiScope.ToEntity());
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
