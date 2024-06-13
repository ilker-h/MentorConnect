using System.Globalization;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
  public class ConnectionRequestsController : BaseApiController
  {
    private readonly IUnitOfWork _uow;

    public ConnectionRequestsController(IUnitOfWork uow)
    {
      _uow = uow;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddConnectionRequest(string username)
    {
      var sourceUserId = User.GetUserId();
      var connectionRequestedFromUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
      var sourceUser = await _uow.ConnectionRequestsRepository.GetUserWithConnectionRequests(sourceUserId);

      if (connectionRequestedFromUser == null) return NotFound();

      if (sourceUser.UserName == username) return BadRequest("You cannot follow yourself.");

      var userConnectionRequest = await _uow.ConnectionRequestsRepository.GetUserConnectionRequest(sourceUserId, connectionRequestedFromUser.Id);

      if (userConnectionRequest != null) return BadRequest("You already followed this user");

      userConnectionRequest = new UserConnectionRequest
      {
        SourceUserId = sourceUserId,
        TargetUserId = connectionRequestedFromUser.Id
      };

      sourceUser.ConnectionRequestedFromUsers.Add(userConnectionRequest);

      if (await _uow.Complete()) return Ok();

      return BadRequest("Failed to follow user");

    }

    [HttpPost("remove/{username}")]
    public async Task<ActionResult> RemoveConnectionRequest(string username)
    {
      var sourceUserId = User.GetUserId();
      var connectionRequestedToRemoveFromUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
      var sourceUser = await _uow.ConnectionRequestsRepository.GetUserWithConnectionRequests(sourceUserId);

      if (connectionRequestedToRemoveFromUser == null) return NotFound();

      if (sourceUser.UserName == username) return BadRequest("You cannot unfollow yourself.");

      var userConnectionRemovalRequest = await _uow.ConnectionRequestsRepository.GetUserConnectionRequest(sourceUserId, connectionRequestedToRemoveFromUser.Id);

      userConnectionRemovalRequest = new UserConnectionRequest
      {
        SourceUserId = sourceUserId,
        TargetUserId = connectionRequestedToRemoveFromUser.Id
      };

      var elementToRemoveFromList = sourceUser.ConnectionRequestedFromUsers.FirstOrDefault(x => x.TargetUserId == userConnectionRemovalRequest.TargetUserId);
      var indexOfElementToRemoveFromList = sourceUser.ConnectionRequestedFromUsers.IndexOf(elementToRemoveFromList);

      // i.e., the target user exists in the db but is not being followed by the source user
      if (userConnectionRemovalRequest.TargetUserId.GetType() == typeof(int) && indexOfElementToRemoveFromList == -1)
      {
        return BadRequest("You cannot unfollow this user because you are not following them");
      }

      sourceUser.ConnectionRequestedFromUsers.RemoveAt(indexOfElementToRemoveFromList);

      if (await _uow.Complete()) return Ok();

      return BadRequest("Failed to unfollow user");

    }

    // since ConnectionRequestsParams is an object and not a query string that the API controller would be easily able to bind to,
    // [FromQuery] must be used to tell the API where to find the parameters
    [HttpGet]
    public async Task<ActionResult<PagedList<ConnectionRequestDto>>> GetUserConnectionRequests([FromQuery] ConnectionRequestsParams connectionRequestsParams)
    {
      connectionRequestsParams.UserId = User.GetUserId();

      var users = await _uow.ConnectionRequestsRepository.GetUserConnectionRequests(connectionRequestsParams);

      Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,
       users.PageSize, users.TotalCount, users.TotalPages));

      return Ok(users);
    }
  }
}