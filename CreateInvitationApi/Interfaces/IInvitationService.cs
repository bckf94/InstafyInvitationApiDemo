using System.Threading.Tasks;
using CreateInvitationApi.Models;

namespace CreateInvitationApi.Interfaces
{
    public interface IInvitationService
    {
        Task<Invitation> GetInvitationAsync(string email, string roles, string provider, int numHoursToExpiration);
    }
}