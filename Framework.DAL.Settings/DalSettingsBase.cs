using Framework.WebCommon;
using System;
using System.Configuration;

namespace Framework.DAL.Settings
{
    public abstract class DalSettingsBase : WebConfig, IDalSettings
    {
        private readonly string _connectionString;
        private readonly int _commandTimeoutInSeconds;
        private const int DefaultTimeOutInSeconds = 300;

        protected DalSettingsBase(string ConnectionStringAttributeDev, string ConnectionStringAttributeQA, string ConnectionStringAttributeProd, string commandTimeoutAttributeName)
        {
            var section = (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");
                
            if (GetSetting<string>(ConnectionStringAttributeDev) == "true")
            {
                _connectionString = section.ConnectionStrings["DefaultConnectionDev"].ConnectionString;
            }
            else if (GetSetting<string>(ConnectionStringAttributeQA) == "true")
            {
                _connectionString = section.ConnectionStrings["DefaultConnectionQA"].ConnectionString;
            }
            else if (GetSetting<string>(ConnectionStringAttributeProd) == "true")
            {
                _connectionString = section.ConnectionStrings["DefaultConnectionProd"].ConnectionString;
            }
            
            try
            {
                _commandTimeoutInSeconds = GetSetting<int>(commandTimeoutAttributeName);
            }
            catch (Exception)
            {
                _commandTimeoutInSeconds = DefaultTimeOutInSeconds;
            }

        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public int CommandTimeout
        {
            get { return _commandTimeoutInSeconds; }
        }
    }
}
