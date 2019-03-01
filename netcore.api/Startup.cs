using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using netcore.api.Models;
using netcore.infrastructure;
using netcore.infrastructure.Interfaces;
using netcore.infrastructure.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace netcore.api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddHateoas(options =>
            {
                options
                   .AddLink<UserDto>("get-user", p => new { id = p.Id })
                   .AddLink<List<UserDto>>("create-user")
                   .AddLink<UserDto>("update-user", p => new { id = p.Id })
                   .AddLink<UserDto>("delete-user", p => new { id = p.Id });
            });
            #region Entity framework core 
            var connection = Configuration.GetConnectionString("SqlDb");
            services.AddDbContext<SampleDbContext>(options =>
               options
                   //.UseLazyLoadingProxies()
                   .UseSqlServer(connection,
                   sqlServerOptionsAction: sqlOptions =>
                   {
                       sqlOptions.MigrationsAssembly("netcore.api");
                       sqlOptions.EnableRetryOnFailure(
                       maxRetryCount: 5,
                       maxRetryDelay: TimeSpan.FromSeconds(10),
                       errorNumbersToAdd: null);
                   }).EnableSensitiveDataLogging());
            #endregion
            #region Swagger
            services.AddSwaggerGen(c =>
                       {
                           c.SwaggerDoc("v1", new Info
                           {
                               Version = "v1",
                               Title = "NetCore API",
                               Description = "Base project with Net Core 2.2",
                               TermsOfService = "None",
                               Contact = new Contact
                               {
                                   Name = "Andrés Londoño",
                                   Email = "andreslon@outlook.com",
                                   Url = "http://www.andreslon.com/"
                               }
                           });
                           var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                           var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                           c.IncludeXmlComments(xmlPath);
                       });
            #endregion

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region CORS
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });
            #endregion
            #region Entity framework core 
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SampleDbContext>();
                context.Database.EnsureCreated();
            }
            #endregion
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCore API");
                c.RoutePrefix = string.Empty;
            });
            #endregion
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
