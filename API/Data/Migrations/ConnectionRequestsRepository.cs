using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API.Data
{
    public class ConnectionRequestsRepository : IConnectionRequestsRepository
    {
        private readonly DataContext _context;

        public ConnectionRequestsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserConnectionRequest> GetUserConnectionRequest(int SourceUserId, int TargetUserId)
        {
            return await _context.ConnectionRequests.FindAsync(SourceUserId, TargetUserId);
        }

        public async Task<PagedList<ConnectionRequestDto>> GetUserConnectionRequests(ConnectionRequestsParams connectionRequestsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable(); // AsQueryable means it hasn't been executed on the db yet
            var connectionRequests = _context.ConnectionRequests.AsQueryable();

            if (connectionRequestsParams.Predicate == "connection_requested_from") // instead of "liked"
            {
                connectionRequests = connectionRequests.Where(connectionRequest => connectionRequest.SourceUserId == connectionRequestsParams.UserId);
                users = connectionRequests.Select(connectionRequest => connectionRequest.TargetUser);
            }

            if (connectionRequestsParams.Predicate == "connection_requested_by") // instead of "likedBy"
            {
                connectionRequests = connectionRequests.Where(connectionRequest => connectionRequest.TargetUserId == connectionRequestsParams.UserId);
                users = connectionRequests.Select(connectionRequest => connectionRequest.SourceUser);
            }

            var connectionRequestedFromUsers = users.Select(user => new ConnectionRequestDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                YearsOfCareerExperience = user.YearsOfCareerExperience,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<ConnectionRequestDto>.CreateAsync(connectionRequestedFromUsers, connectionRequestsParams.PageNumber, connectionRequestsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithConnectionRequests(int userId)
        {
            return await _context.Users
                .Include(x => x.ConnectionRequestedFromUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}