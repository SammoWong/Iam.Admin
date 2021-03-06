using Iam.HealthCheck;
using Iam.Jwt;
using Iam.ServiceDiscovery.Consul;
using Iam.ServiceDiscovery.LoadBalancers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iam.Identity.ServiceHost
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
            services.AddHealthCheck()/*.AddMySql("SqlConnection")*/;
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Iam.Identity.ServiceHost", Version = "v1" });
            });

            var jwtConfig = Configuration.GetSection("Jwt").Get<JwtConfig>();
            services.AddJwt(jwtConfig);

            var consulConfig = Configuration.GetSection("Consul").Get<ConsulConfig>();
            services.AddConsul(consulConfig.ConsulUrl, loadBalancer: TypeLoadBalancer.RoundRobin);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iam.Identity.ServiceHost v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //������֤
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthCheck();
            });

            var consulConfig = Configuration.GetSection("Consul").Get<ConsulConfig>();
            app.RegisterToConsul(consulConfig);
        }
    }
}
