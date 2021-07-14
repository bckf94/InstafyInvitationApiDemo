using System.Threading.Tasks;

namespace CreateInvitationApi.Service
{
    public interface IAzureCliService
    {
        Task<Invitation> GetInvitation(string name);
    }
}