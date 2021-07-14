using CreateInvitationApi.Models;

namespace CreateInvitationApi.Interfaces
{
    public interface IConfigurationManager
    {
        ApiConfiguration GetAzureCliSettings();
    }
}