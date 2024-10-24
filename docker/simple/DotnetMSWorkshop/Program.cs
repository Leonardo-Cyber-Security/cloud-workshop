using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DotnetMSWorkshop.Db;
using DotnetMSWorkshop.Utils.Filters;
using DotnetMSWorkshop.Utils.Formatters;
using DotnetMSWorkshop.Utils.OpenApi;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace DotnetMSWorkshop
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add appsettings from kubernetes configmap
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, new InputFormatterStream());
            })
            .AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opts.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
                c.SwaggerDoc("1.0.0", new OpenApiInfo
                {
                    Title = "Workshop",
                    Description = "Workshop",
                    Version = "1.0.0",
                });
                c.CustomSchemaIds(type => type.FriendlyId(true));
                c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetExecutingAssembly().GetName().Name}.xml");

                // Include DataAnnotation attributes on Controller Action parameters as OpenAPI validation rules (e.g required, pattern, ..)
                // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                c.OperationFilter<GeneratePathParamsValidationFilter>();
            });
            builder.Services.AddSwaggerGenNewtonsoftSupport();

            #region Serilog
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    // .Enrich.FromLogContext()
                    .CreateLogger();
            builder.Logging.ClearProviders().AddSerilog(Log.Logger);
            builder.Services.AddSingleton(Log.Logger);
            #endregion

            builder.Services.AddHttpClient();
            builder.Services.AddHealthChecks();

            #region SQL Connection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            builder.Services.AddHttpContextAccessor();

            #region Custom Services

            //INSERT HERE YOUR SERVICES

            #endregion

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "openapi/{documentName}/openapi.yaml";
            })
                .UseSwaggerUI(c =>
                {
                    // set route prefix to openapi, e.g. http://localhost:8080/openapi/index.html
                    c.RoutePrefix = "openapi";
                    //TODO: Either use the SwaggerGen generated OpenAPI contract (generated from C# classes)
                    c.SwaggerEndpoint("/openapi/1.0.0/openapi.yaml", "Workshop");

                    //TODO: Or alternatively use the original OpenAPI contract that's included in the static files
                    // c.SwaggerEndpoint("/openapi-original.json", "Workshop Original");
                });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health/readiness");
                endpoints.MapHealthChecks("/health/liveness");
            });

            #region CultureInfo
            if (!builder.Environment.IsDevelopment())
            {
                string cInfoString = "it-IT";
                string env = Environment.GetEnvironmentVariable("CultureInfo");
                if (!string.IsNullOrEmpty(env))
                    cInfoString = env;
                var cultureInfo = new CultureInfo(cInfoString);
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DecimalSeparator")))
                    cultureInfo.NumberFormat.NumberDecimalSeparator = Environment.GetEnvironmentVariable("DecimalSeparator");
                else
                    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

                CultureInfo.CurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
            #endregion

            app.Run();
        }
    }
}