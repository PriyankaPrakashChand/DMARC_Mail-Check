﻿using Amazon.SimpleSystemsManagement;
using Dmarc.Common.Api.Middleware;
using Dmarc.Common.Data;
using Dmarc.Common.Encryption;
using Dmarc.Metrics.Api.Dao;
using Dmarc.Metrics.Api.Domain;
using Dmarc.Metrics.Api.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dmarc.Metrics.Api
{
    public class StartUp
    {
        public StartUp(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .Build();
        }

        public static IConfigurationRoot Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHealthChecks(HealthCheckOptions)
                .AddCors(CorsOptions)
                .AddTransient<IConnectionInfoAsync, ConnectionInfoAsync>()
                .AddTransient<IParameterStoreRequest, ParameterStoreRequest>()
                .AddTransient<IConnectionInfo>(p => new StringConnectionInfo(Environment.GetEnvironmentVariable("ConnectionString")))
                .AddTransient<IValidator<MetricsDateRange>, MetricsDateRangeValidator>()
                .AddTransient<IMetricsDao, MetricsDao>()
                .AddTransient<IAmazonSimpleSystemsManagement, AmazonSimpleSystemsManagementClient>()
                .AddMvc();
        }

        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole((st, logLevel) => logLevel >= LogLevel.Debug);

            appBuilder
                .UseAuthentication()
                .UseMiddleware<UnhandledExceptionMiddleware>()
                .UseCors(CorsPolicyName)
                .UseMvc();
        }

        private static Action<CorsOptions> CorsOptions => options =>
        {
            options.AddPolicy(CorsPolicyName, builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        };

        private static Action<HealthCheckBuilder> HealthCheckOptions => checks =>
        {
            checks.AddValueTaskCheck("HTTP Endpoint", () => new ValueTask<IHealthCheckResult>(HealthCheckResult.Healthy("Ok")));
        };

        private const string CorsPolicyName = "CorsPolicy";
    }
}
