using System.Reflection;
using IdentityServerCourse.IdentityServer.Configurations;
using IdentityServerCourse.IdentityServer.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerCourse.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            var connection = Configuration.GetConnectionString("DefaultConnection");
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connection,
                        sql => sql.MigrationsAssembly(migrationAssembly));
                })
                // Store tokens, connects ...
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connection,
                        sql => sql.MigrationsAssembly(migrationAssembly));
                })
                //.AddInMemoryApiResources(IdentityConfiguration.GetAllApiResources())
                //.AddInMemoryIdentityResources(IdentityConfiguration.GetIdentityResources())
                //.AddInMemoryApiScopes(IdentityConfiguration.GetAllApiScopes())
                //.AddInMemoryClients(IdentityConfiguration.GetAllClients())
                .AddTestUsers(IdentityConfiguration.GetUsers());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.InitDataBase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
