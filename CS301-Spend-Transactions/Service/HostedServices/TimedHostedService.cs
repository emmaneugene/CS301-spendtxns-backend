using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CS301_Spend_Transactions.Domain.Exceptions;
using CS301_Spend_Transactions.Repo.Helpers;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using CS301_Spend_Transactions.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Service.HostedServices
{
    public class TimedHostedService : BackgroundService
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly ISQSService _sqsService;
        private readonly ITransactionService _transactionService;
        private readonly IMerchantService _merchantService;

        public TimedHostedService(ILogger<TimedHostedService> logger, ISQSService sqsService,
            ITransactionService transactionService, IMerchantService merchantService)
        {
            _logger = logger;
            _sqsService = sqsService;
            _transactionService = transactionService;
            _merchantService = merchantService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is running");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "[TimedHostedService/DoWork] Starting an iteration");
                
                var sw = Stopwatch.StartNew();
                int txn_count = 0;

                Parallel.For(1, 101, i =>
                {
                    var messages = _sqsService.GetMessages();
                    _logger.LogInformation($"Consumed {messages.Result.Count} messages from SQS");

                    if (messages.Result.Count == 0) return;

                    var dtos = messages.Result.Select(m =>
                    {
                        return TransactionMapperHelper.ToTransactionDTO(m.Body);
                    });
                    _logger.LogInformation($"Converted {dtos.Count()} messages to DTO");

                    foreach (var dto in dtos)
                    {
                        txn_count++;
                        try
                        {
                            _logger.LogInformation(dto.ToString());

                            // cannot async because need merchant as fk to index transactions
                            _merchantService.AddMerchant(dto);
                            _transactionService.AddTransaction(dto);
                        }
                        catch (InvalidTransactionException e)
                        {
                            _logger.LogCritical(
                                $"[TimedHostedService/DoWork] Transaction {dto.Transaction_Id} failed due to {e.Message}");
                        }
                    }
                });
                _logger
                    .LogInformation($"[PERF] Time taken to ingest {txn_count} transactions: {sw.ElapsedMilliseconds}");
            }
            // not delaying for now as we want to maximize performance
            // await Task.Delay(1, stoppingToken);
        }
    }
}