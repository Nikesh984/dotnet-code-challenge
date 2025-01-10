﻿using System;

using CodeChallenge.Data;
using CodeChallenge.Repositories;
using CodeChallenge.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeChallenge.Config
{
    public class App
    {
        public WebApplication Configure(string[] args)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.UseEmployeeDB();

            AddServices(builder.Services);

            var app = builder.Build();

            var env = builder.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedEmployeeDB();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private void AddServices(IServiceCollection services)
        {

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRespository>();
            services.AddScoped<IReportingService, ReportingService>();
            services.AddScoped<IReportingRepository, ReportingRepository>();

            services.AddControllers();
        }

        private void SeedEmployeeDB()
        {
            // new EmployeeDataSeeder(
            //     new EmployeeContext(
            //         new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
            // )).Seed().Wait();

            var employeeContext = new EmployeeContext(
                new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
            );

            var employeeSeeder = new EmployeeDataSeeder(employeeContext);
            var compensationSeeder = new CompensationDataSeeder(employeeContext);

            // Seeding employee data from resources
            employeeSeeder.Seed().Wait();

            // Seeding compensation data from resources
            compensationSeeder.Seed().Wait();
        }
    }
}
