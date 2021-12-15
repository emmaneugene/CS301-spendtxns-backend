namespace CS301_Spend_Transactions.Domain.Configurations
{
    public class SQSOption
    {
        public SQSOption() {}
        public SQSOption(string queueUrl, string region, string accessKey, string secretKey)
        {
            QueueURL = queueUrl;
            Region = region;
            AccessKey = accessKey;
            SecretKey = secretKey;
        }

        public string QueueURL { get; set; }

        public string Region { get; set; }
        
        public string AccessKey { get; set; }
        
        public string SecretKey { get; set; }

    }
}