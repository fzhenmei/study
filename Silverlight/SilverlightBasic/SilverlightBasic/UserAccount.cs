namespace SilverlightApplication1
{
    public class UserAccount
    {
        public string AccountName { get; set; }
        public double AccountBalance { get; set; }

        public UserAccount(string accountName, double accountBanlance)
        {
            AccountName = accountName;
            AccountBalance = accountBanlance;
        }
    }
}