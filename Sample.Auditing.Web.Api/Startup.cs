using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Auditing.Data;
using Sample.Auditing.Data.UnitsOfWork;

namespace Sample.Auditing.Web.Api
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
            #region Add Entity Framework 

            services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("CatalogModelConnection")));

            services.AddScoped<IUnitOfWork, ProductCatalogUnitOfWork>();
            #endregion

            #region Add AutoMapper

            //Add all mapping profiles from the assembly
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                foreach (var profile in this.GetType().Assembly.GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Profile)))
                     .Select(t => Activator.CreateInstance(t) as Profile))
                {
                    cfg.AddProfile(profile);
                }
            }).CreateMapper());
            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
