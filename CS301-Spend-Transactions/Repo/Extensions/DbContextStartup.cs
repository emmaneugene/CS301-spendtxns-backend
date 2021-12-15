using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;

namespace CS301_Spend_Transactions.Extensions
{
    public static class DbContextStartup
    {
        public static void AddDbContextInjections(this IServiceCollection services, IWebHostEnvironment env,
            IConfiguration configuration)
        {
            Console.WriteLine($"Welcome to '{env.EnvironmentName}' environment, your machine" +
                              $" name is '{Environment.MachineName}'.");

            var dbConnection = GetConnectionStrings(env, configuration);
            
            if (string.IsNullOrEmpty(dbConnection))
                throw new InvalidDataException("Invalid database connection string");
            
            services.AddDbContextPool<AppDbContext>(options => options.UseMySQL(dbConnection));
        }
        private static string GetConnectionStrings(IWebHostEnvironment env, IConfiguration configuration)
        {
            if (!env.IsProduction())
                return configuration.GetConnectionString($"DefaultConnection:{Environment.MachineName}");

            // TODO: Need to set Db name during deployment
            var connectionString = Environment.GetEnvironmentVariable("transactionDb");
            return !string.IsNullOrEmpty(connectionString) ? connectionString : null;
        }
    }
}