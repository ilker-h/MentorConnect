using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IConnectionRequestsRepository
    {
        Task<UserConnectionRequest> GetUserConnectionRequest(int SourceUserId, int TargetUserId);
        Task<AppUser> GetUserWithConnectionRequests(int userId);
        // "predicate" means, does the user want to get the user they sent a connection request to, or a user they have been sent connections requests from?
        Task<PagedList<ConnectionRequestDto>> GetUserConnectionRequests(ConnectionRequestsParams connectionRequestsParams);
    }
}