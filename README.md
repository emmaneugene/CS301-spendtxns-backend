# G1-T5-Spend-Transactions-Backend

### Dependencies
- Local installation of MySQL server  
- [.NET 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) framework
- Project dependencies as specified in `CS301-Spend-Transactions.csproj`
- [Docker](https://www.docker.com/get-started) for building container images

### Configuration 
Add this file at root directory and below configurations 
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": {
      "<Machine-Name>": "server=<host-url>;uid=<mysql-user>;pwd=<mysql-password>;database=<db-name>;",
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

Replace the variables `<Machine-Name>`, `<host-url>`, `<mysql-user>`, `<mysql-password>` and `<db-name>` 
with the relevant values for your local machine 

```json
// Properties/launchSettings.json
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:13361",
      "sslPort": 44368
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "CS301_Spend_Transactions": {
      "commandName": "Project",
      "launchBrowser": false,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "QueueUrl": "<queue-url>",
        "SQSRegion": "<queue-region>",
        "AccessKey": "<access-key>",
        "SecretKey": "<secret-key>",
        "SenderEmail": "<sender-email-address>",
        "ReceiverEmail": "<receiver-email-address>"
      }
    }
  }
}

```

The application consumes messages from an AWS SQS queue, which must be specified in `<queue-url>`. `<queue-region>`denotes the region that the queue is deployed in. `access-key` and `secret-key` correspond to the key pairs for AWS programmatic access. Finally, `sender-email-address`  and `receiver-email-address` correspond to the email addresses to be used for notifications on failed transactions.

### Setting up database
Making sure your database URL in appsettings.json is valid:
1. In the directorty CS301-Spend-Transactions, run 
```sh
dotnet ef database update
```

*Note that for this service to work properly, you require data in these tables
1. users
2. cards
3. exclusions
4. merchants 
5. points_type
6. rules

You can use ```/api/Database/InitialSeed``` endpoint to do the initial seeding. For users and cards we recommend using Jetbrains Riders to import the user.csv to users and cards table as seeding up to millions of rows is rather slow.

### Usage

**Local**: After filling out the necessary details in `appsettings.json` and `launchSettings.json`, navigate into `CS301-Spend-Transactions` directory and execute `dotnet run` within the terminal

**Docker**: After running `docker build` from the root directory, you can run a container of the docker image, specifying individual environment variables with the `-e` flag or in an file with`--env-file`


### Upload to ECR
On any successful push or merge request to the main branch, a github action will be triggered, which automatically builds an updated container image and pushes it to ECR. Some secret variables must first be specified in **Settings** -> **Secrets**

Testing CI/CD workflow!
