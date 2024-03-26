using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Services;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Otus.Teaching.PromoCodeFactory.WebHost.Mapping;
using static System.Formats.Asn1.AsnWriter;

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {

        private static IServiceCollection InstallAutomapper(IServiceCollection services)
        {
            services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));            
            return services;
        }

        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingsProfile>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            InstallAutomapper(services);
            services.AddControllers();
            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });

            // получаем строку подключения.
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var connectionString = builder.Build()["ConnectionString"];

            // определяем DBContext.
            services
                .AddDbContext<DatabaseContext>(optionsBuilder =>
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite(connectionString));


            services.AddScoped(typeof(IRepository<Role>), (x) =>
               new EfRepository<Role>(x.GetService<DatabaseContext>()));

            services.AddScoped(typeof(IRepository<Employee>), (x) =>
                new EfRepository<Employee>(x.GetService<DatabaseContext>()));    
            
            services.AddScoped(typeof(IRepository<Preference>), (x) =>
                new EfRepository<Preference>(x.GetService<DatabaseContext>()));

            services.AddScoped(typeof(IRepository<Customer>), (x) =>
                new EfRepository<Customer>(x.GetService<DatabaseContext>()));

            services.AddScoped(typeof(IRepository<PromoCode>), (x) =>
                new EfRepository<PromoCode>(x.GetService<DatabaseContext>()));

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IPreferenceService, PreferenceService>();

            services.AddScoped<IPromoCodeService, PromoCodeService>();            

            services.AddScoped<IDbInit, DbInit>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInit dbinit)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            dbinit.Init();
        }
    }
}