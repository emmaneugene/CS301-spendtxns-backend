using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CS301_Spend_Transactions.Models;
using CS301_Spend_Transactions.Repo.Helpers.Interfaces;
using CS301_Spend_Transactions.Services;
using CS301_Spend_Transactions.Services.Interfaces;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS301_Spend_Transactions.Repo.Helpers
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<UserService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DatabaseSeeder(IServiceScopeFactory scopeFactory,
            ILogger<UserService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task<int> SeedDummyUserEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/DummyUsers.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                var users = csvReader.GetRecords<User>();

                dbContext.Users.AddRange(users);
                return await dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> SeedUserEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/users.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    var record = new User
                    {
                        Id = csvReader.GetField("id"),
                        FirstName = csvReader.GetField("first_name"),
                        LastName = csvReader.GetField("last_name"),
                        PhoneNo = csvReader.GetField("phone"),
                        Email = csvReader.GetField("email"),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    try
                    {
                        dbContext.Users.Add(record);
                    }
                    catch (InvalidOperationException e)
                    {
                        _logger.LogInformation($"User {record.Id} already added");
                    }
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedUserAndCardEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            int i = 0;

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/users.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    i++;
                    var userRecord = new User
                    {
                        Id = csvReader.GetField("id"),
                        FirstName = csvReader.GetField("first_name"),
                        LastName = csvReader.GetField("last_name"),
                        PhoneNo = csvReader.GetField("phone"),
                        Email = csvReader.GetField("email"),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    if (dbContext.Users.Find(csvReader.GetField("id")) is null)
                    {
                        dbContext.Users.Add(userRecord);
                        dbContext.SaveChanges();
                    }

                    var cardRecord = new Card
                    {
                        Id = csvReader.GetField("card_id"),
                        UserId = csvReader.GetField("id"),
                        CardPan = csvReader.GetField("card_pan"),
                        CardType = csvReader.GetField("card_pan"),
                        User = dbContext.Users.Find(csvReader.GetField("id"))
                    };


                    dbContext.Cards.Add(cardRecord);
                    _logger.LogInformation($"Card {cardRecord.Id} added");

                    _logger.LogInformation($"Read row {i}");
                    await dbContext.SaveChangesAsync();
                }
            }

            return 0;
        }

        public async Task<int> SeedCardEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/users.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    var record = new Card
                    {
                        Id = csvReader.GetField("card_id"),
                        UserId = csvReader.GetField("id"),
                        CardPan = csvReader.GetField("card_pan"),
                        CardType = csvReader.GetField("card_pan")
                    };

                    dbContext.Cards.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedDummyCardEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/DummyCards.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    var record = new Card
                    {
                        Id = csvReader.GetField("Id"),
                        UserId = csvReader.GetField("UserId"),
                        CardPan = csvReader.GetField("CardPan"),
                        CardType = csvReader.GetField("CardType")
                    };

                    dbContext.Cards.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedTransactionEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var cardService = scope.ServiceProvider.GetRequiredService<ICardService>();
            var ruleService = scope.ServiceProvider.GetRequiredService<IRuleService>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/DummyTransactions.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    var record = new Transaction
                    {
                        Id = csvReader.GetField("Id"),
                        CardId = csvReader.GetField("CardId"),
                        MerchantName = csvReader.GetField("MerchantName"),
                        TransactionDate = csvReader.GetField<DateTime>("TransactionDate"),
                        Currency = csvReader.GetField("Currency"),
                        Amount = csvReader.GetField<decimal>("Amount")
                    };

                    Card card = cardService.GetCardById(record.CardId);
                    Rule rule = ruleService.GetRule(card, record);


                    var point = new Points
                    {
                        TransactionId = record.Id,
                        PointsTypeId = rule.PointsTypeId,
                        Amount = rule.GetReward(record.Amount),
                        ProcessedDate = DateTime.Now
                    };

                    dbContext.Transactions.Add(record);
                    dbContext.Points.Add(point);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedGroupEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/Groups.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var record = new Groups
                    {
                        MinMCC = csvReader.GetField<int>("MinMCC"),
                        MaxMCC = csvReader.GetField<int>("MaxMCC"),
                        Name = csvReader.GetField("Name"),
                    };
                    dbContext.Groups.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedPointsTypeEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/PointsType.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var record = new PointsType()
                    {
                        Id = csvReader.GetField<int>("Id"),
                        Description = csvReader.GetField("Description"),
                        Unit = csvReader.GetField("Unit"),
                    };
                    dbContext.PointsTypes.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedProgramEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/Programs.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var record = new CS301_Spend_Transactions.Models.Program
                    {
                        CardType = csvReader.GetField("CardType"),
                        Multiplier = csvReader.GetField<decimal>("Multiplier"),
                        MCC = csvReader.GetField<int>("MCC"),
                        MinSpend = csvReader.GetField<decimal>("MinSpend"),
                        MaxSpend = csvReader.GetField<decimal>("MaxSpend"),
                        ForeignSpend = csvReader.GetField<bool>("ForeignSpend"),
                        PointsTypeId = csvReader.GetField<int>("PointsTypeId")
                    };

                    dbContext.Rules.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedMerchantEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/Merchants.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var record = new Merchant()
                    {
                        Name = csvReader.GetField("MerchantName"),
                        MCC = csvReader.GetField<int>("MCC"),
                    };

                    dbContext.Merchants.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedExclusionEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/Exclusions.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var record = new Exclusion()
                    {
                        CardType = csvReader.GetField("CardType"),
                        MCC = csvReader.GetField<int>("MCC")
                    };

                    dbContext.Exclusions.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> SeedCampaignEntries()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            using (TextReader fileReader = File.OpenText("Repo/Helpers/Seeds/Campaigns.csv"))
            {
                CsvReader csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var record = new Campaign()
                    {
                        CardType = csvReader.GetField("CardType"),
                        MinSpend = csvReader.GetField<decimal>("MinSpend"),
                        MaxSpend = csvReader.GetField<decimal>("MaxSpend"),
                        ForeignSpend = csvReader.GetField<bool>("ForeignSpend"),
                        Multiplier = csvReader.GetField<decimal>("Multiplier"),
                        StartDate = csvReader.GetField<DateTime>("StartDate"),
                        EndDate = csvReader.GetField<DateTime>("EndDate"),
                        Description = csvReader.GetField("Description"),
                        MerchantName = csvReader.GetField("MerchantName"),
                        PointsTypeId = csvReader.GetField<int>("PointsTypeId")
                    };
                    dbContext.Campaigns.Add(record);
                }
            }

            return await dbContext.SaveChangesAsync();
        }
    }
}