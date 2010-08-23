
using Westwind.Utilities;
using Westwind.Utilities.Configuration;
namespace Amazon
{
    public class AmazonConfiguration : AppConfiguration
    {

        public AmazonConfiguration() 
        {

        }

        public AmazonConfiguration(IConfigurationProvider provider)
        {
            if (provider == null)
            {
                this.Provider = new ConfigurationFileConfigurationProvider<AmazonConfiguration>()
                {
                    ConfigurationSection = "AmazonConfiguration"
                    //PropertiesToEncrypt = "AmazonSecretId",
                    //EncryptionKey = "WebToolkit"
                };
            }
            else
                this.Provider = provider;

            this.Read();
        }

        /// <summary>
        /// Amazon Associates Id
        /// </summary>
        public string AmazonAssociateId
        {
            get { return _AmazonId; }
            set { _AmazonId = value; }
        }
        private string _AmazonId = "";

        /// <summary>
        /// The Amazon Web Service Key 
        /// </summary>
        public string AmazonAWSAccessKey
        {
            get { return _AmazonAWSAccessKey; }
            set { _AmazonAWSAccessKey = value; }
        }
        private string _AmazonAWSAccessKey = "";


        /// <summary>
        /// Amazon Secret Key
        /// </summary>
        public string AmazonSecretId
        {
            get { return _AmazonSecretId; }
            set { _AmazonSecretId = value; }
        }
        private string _AmazonSecretId = "4DtDvIaVj5cr+A3w/SXFIrErSFRmsseQfA94e4oP";


        /// <summary>
        /// The Service end point url that is called for requests (not the WSDL)
        /// </summary>
        public string AmazonServiceEndPointUrl
        {
            get { return _AmazonServiceEndPointUrl; }
            set { _AmazonServiceEndPointUrl = value; }
        }
        private string _AmazonServiceEndPointUrl = "https://ecs.amazonaws.com/onca/soap?Service=AWSECommerceService";


    }
}