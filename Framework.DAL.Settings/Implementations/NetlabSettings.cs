
namespace Framework.DAL.Settings.Implementations
{
    public class NetlabSettings : DalSettingsBase, IDalSettings
    {

        private const string CommandTimeoutAttribute = "CommandTimeout";

        private const string ConnectionStringAttributeDev = "useConnectionDev";
        private const string ConnectionStringAttributeQA  = "useConnectionQA";
        private const string ConnectionStringAttributeProd = "useConnectionProd";

        public NetlabSettings() : base(ConnectionStringAttributeDev, ConnectionStringAttributeQA, ConnectionStringAttributeProd, CommandTimeoutAttribute)
        {
        }
    }
}
