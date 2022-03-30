using ApiExamenTicketsApb.Data;
using ApiExamenTicketsApb.Repositories;
using ApiExamenTicketsApb.Services;
using ApiPracticaExamenAzure.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiExamenTicketsApb {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            string azuresql = this.Configuration.GetConnectionString("azuresql");
            services.AddTransient<RepositoryApplication>();
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(azuresql));

            HelperOAuthToken helper = new HelperOAuthToken(this.Configuration);
            services.AddAuthentication(helper.GetAuthenticationOptions()).AddJwtBearer(helper.GetJwtOptions());
            services.AddTransient<HelperOAuthToken>(x => helper);

            ServiceLogicApp serviceLogicApp = new ServiceLogicApp();
            services.AddTransient<ServiceLogicApp>(x => serviceLogicApp);

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiExamenTicketsApb", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();    
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiExamenTicketsApb v1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
