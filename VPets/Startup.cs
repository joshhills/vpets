using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VPets.Domain.Repositories;
using VPets.Domain.Services;
using VPets.Persistence.Contexts;
using VPets.Persistence.Repositories;
using VPets.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;

namespace VPets
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
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "VPets",
                    Version = "v1",
                    Description = "A simple virtual pets game as a ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Josh Hills",
                        Email = "josh@jargonify.com",
                        Url = new System.Uri("https://jargonify.com")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase("vpets-in-memory");
            });

            services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IPetRepository, PetRepository>()
                .AddScoped<IPetService, PetService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddAutoMapper(typeof(Startup));

            services.AddHostedService<PetMetricStateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VPets V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
