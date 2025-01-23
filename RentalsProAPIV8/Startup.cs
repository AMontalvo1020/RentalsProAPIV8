using System.Net;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using RentalsProAPIV8.Core.Extensions;
using RentalsProAPIV8.Infrastructure.Profiles;
using RentalsProAPIV8.Infrastructure.Repositories;
using RentalsProAPIV8.Infrastructure.Repositories.Interfaces;
using RentalsProAPIV8.Infrastructure.UnitOfWork;
using RentalsProAPIV8.Infrastructure.UnitOfWork.Interface;
using RentalsProAPIV8.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RentalsProAPIV8
{
    public class Startup
    {
        private string Message = "";

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void ConfigureServices(IServiceCollection services)
        {
            // Enable detailed identity logging (for development purposes only)
            IdentityModelEventSource.ShowPII = true;

            // Configure controllers with Newtonsoft.Json
            services.AddControllers().AddNewtonsoftJson(opts => opts.UseMemberCasing());
            // Configure API versioning
            services.AddApiVersioning(opts =>
            {
                opts.ReportApiVersions = true;
                opts.AssumeDefaultVersionWhenUnspecified = true;
            }).AddApiExplorer();
            // Configure Swagger with versioning
            services.AddSwaggerGen();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // Register Repositories
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            //services.AddTransient<IExpensesRepository, ExpenseRepository>();
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IPropertyStatusRepository, PropertyStatusRepository>();
            services.AddTransient<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddTransient<IPaymentStatusRepository, PaymentStatusRepository>();
            services.AddTransient<ILeaseRepository, LeaseRepository>();
            //services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IUnitRepository, UnitRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            // Register UnitOfWork
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddMemoryCache();

            // Add DB Context and Factory
            services.AddDbContext<RentalsProContext>(opt => opt.UseSqlServer(_configuration.GetConnectionString(nameof(RentalsProContext))));
            services.AddDbContextFactory<RentalsProContext>(opt => opt.UseSqlServer(_configuration.GetConnectionString(nameof(RentalsProContext))), ServiceLifetime.Scoped);

            // Register configuration as a singleton
            services.AddSingleton(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            // Development exception page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Enable routing
            app.UseRouting();
            // Configure endpoints
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            // Configure URL rewriting
            var rewriteOptions = new RewriteOptions().AddRedirect("^$", "index.html");
            app.UseRewriter(rewriteOptions);

            app.UseSwagger();
            app.AddSwaggerUI(provider);

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new
                        {
                            context.Response.StatusCode,
                            Message = contextFeature.Error.GetFullMessage()
                        }.ToString());
                    }
                });
            });
        }
    }
}
