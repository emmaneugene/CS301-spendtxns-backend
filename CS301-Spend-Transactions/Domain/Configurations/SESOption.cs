namespace CS301_Spend_Transactions.Domain.Configurations
{
    public class SESOption
    {
        public SESOption() {}
        public SESOption(string accessKey, string secretKey, string senderEmail, string receiverEmail)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            SenderEmail = senderEmail;
            ReceiverEmail = receiverEmail;
        }  

        public string AccessKey { get; set; }
        
        public string SecretKey { get; set; }
        
        public string SenderEmail { get; set; }
        
        public string ReceiverEmail { get; set; }
    }
}