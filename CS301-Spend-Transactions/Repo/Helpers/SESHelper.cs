using Amazon;
using System;
using System.Collections.Generic;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SQS;
using CS301_Spend_Transactions.Domain.Configurations;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CS301_Spend_Transactions.Repo.Helpers
{
    public class SESHelper : ISESHelper
    {
        // Replace sender@example.com with your "From" address.
        // This address must be verified with Amazon SES.
        static string senderAddress;

        // Replace recipient@example.com with a "To" address. If your account
        // is still in the sandbox, this address must be verified.
        static string receiverAddress;

        // The subject line for the email.
        static readonly string subject = "[Warning] Invalid transactions on Transactions service";

        // The email body for recipients with non-HTML email clients.
        static readonly string textBody = "Invalid transaction(s), please check failed_transaction database";
        
        private readonly SESOption _option;
        private AmazonSimpleEmailServiceClient _amazonSimpleEmailServiceClient;
        private ILogger<SQSHelper> _logger;

        public SESHelper(IOptions<SESOption> option, ILogger<SQSHelper> logger)
        {
            _option = option.Value;
            _amazonSimpleEmailServiceClient = new AmazonSimpleEmailServiceClient(
                new BasicAWSCredentials(_option.AccessKey, _option.SecretKey),
                RegionEndpoint.APSoutheast1
            );
            senderAddress = _option.SenderEmail;
            receiverAddress = _option.ReceiverEmail;
            _logger = logger;
        }

        public async void SendFailedTransactionEmail(string transactionId)
        {
            using (var client = _amazonSimpleEmailServiceClient)
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                            new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = $@"<html>
        <head></head>
        <body>
          <p> Transaction with transaction_id {transactionId} is invalid.</p>
          <p> Please check failed_transactions table for debugging. </p>
        </body>
        </html>"
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    // ConfigurationSetName = configSet
                };
                try
                {
                    Console.WriteLine("Sending email using Amazon SES...");
                    var sendEmailResponse = await client.SendEmailAsync(sendRequest);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }
        }
    }
}