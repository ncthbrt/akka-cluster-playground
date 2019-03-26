using System.IO;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Playground.Shared;
using Swashbuckle.AspNetCore.Swagger;

namespace Playground.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            var configTemplate = File.ReadAllText("./akka.conf");
            var systemName = "playground";
            var akkaConfig = 
                ConfigurationTemplating.WithEnvironmentVariables(configTemplate)
                            .WithFallback(DistributedPubSub.DefaultConfig())
                            .WithFallback(ConfigurationFactory.Default());
            services.AddSingleton(new WebService(systemName, akkaConfig));
            
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Zoo Sample",
                    Version = "v1",
                });

                // Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo Sample");
            });
        }
    }
}
