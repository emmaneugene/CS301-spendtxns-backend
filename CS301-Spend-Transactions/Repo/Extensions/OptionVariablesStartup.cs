using System;
using System.IO;
using CS301_Spend_Transactions.Domain.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CS301_Spend_Transactions.Extensions
{
    public static class OptionVariablesStartup
    {
        public static void AddOptionVariables(this IServiceCollection services, IWebHostEnvironment env,
            IConfiguration configuration)
        {
            var sqsConfig = GetSqsOption(configuration, env);
            var sesConfig = GetSesOption(configuration, env);

            if (sqsConfig == null || sesConfig == null)
                throw new InvalidDataException("Invalid sqsConfig object received.");

            services.Configure<SQSOption>(options =>
            {
                options.QueueURL = sqsConfig.QueueURL;
                options.Region = sqsConfig.Region;
                options.AccessKey = sqsConfig.AccessKey;
                options.SecretKey = sqsConfig.SecretKey;
            });

            services.Configure<SESOption>(options =>
            {
                options.AccessKey = sesConfig.AccessKey;
                options.SecretKey = sesConfig.SecretKey;
                options.SenderEmail = sesConfig.SenderEmail;
                options.ReceiverEmail = sesConfig.ReceiverEmail;
            });
        }

        private static SQSOption GetSqsOption(IConfiguration configuration, IWebHostEnvironment env)
        {
            var queueUrl = Environment.GetEnvironmentVariable("QueueUrl");
            var region = Environment.GetEnvironmentVariable("SQSRegion");
            var accessKey = Environment.GetEnvironmentVariable("AccessKey");
            var secretKey = Environment.GetEnvironmentVariable("SecretKey");

            if (string.IsNullOrEmpty(queueUrl) || string.IsNullOrEmpty(region) || string.IsNullOrEmpty(accessKey) ||
                string.IsNullOrEmpty(secretKey))
                return null;

            return new SQSOption(queueUrl, region, accessKey, secretKey);
        }

        private static SESOption GetSesOption(IConfiguration configuration, IWebHostEnvironment env)
        {
            var accessKey = Environment.GetEnvironmentVariable("AccessKey");
            var secretKey = Environment.GetEnvironmentVariable("SecretKey");
            var senderEmail = Environment.GetEnvironmentVariable("SenderEmail");
            var receiverEmail = Environment.GetEnvironmentVariable("ReceiverEmail");

            if (string.IsNullOrEmpty(accessKey) ||
                string.IsNullOrEmpty(secretKey) ||
                string.IsNullOrEmpty(senderEmail) ||
                string.IsNullOrEmpty(receiverEmail))
                return null;

            return new SESOption(accessKey, secretKey, senderEmail, receiverEmail);
        }
    }
}