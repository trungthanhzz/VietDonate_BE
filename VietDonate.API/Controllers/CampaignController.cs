using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using VietDonate.API.Common;
using VietDonate.Application.Common.Mediator;

namespace VietDonate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CampaignController(
  IMediator mediator
) : ApiController
{
  // GET
  
  [HttpGet]
  [Authorize(Roles = "User")]
  public IActionResult GetCampaigns()
  {
    var context = HttpContext;
    return Ok(
      new
      {
        data = new List<object>
        {
          new
          {
            Title = "Campaign 1",
            Description = "Description 1",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 1000.0
          },
          new
          {
            Title = "Campaign 2",
            Description = "Description 2",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 2000.0
          },
          new
          {
            Title = "Campaign 3",
            Description = "Description 3",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 3000.0
          },
          new
          {
            Title = "Campaign 4",
            Description = "Description 4",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 4000.0
          },
          new
          {
            Title = "Campaign 5",
            Description = "Description 5",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 5000.0
          },
          new
          {
            Title = "Campaign 6",
            Description = "Description 6",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 6000.0
          },
          new
          {
            Title = "Campaign 7",
            Description = "Description 7",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 7000.0
          },
          new
          {
            Title = "Campaign 8",
            Description = "Description 8",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 8000.0
          },
          new
          {
            Title = "Campaign 9",
            Description = "Description 9",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 9000.0
          },
          new
          {
            Title = "Campaign 10",
            Description = "Description 10",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
            TargetAmount = 10000.0
          }
        },
      }
    );
  }
}