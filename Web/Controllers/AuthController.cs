using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Users.Commands.Login;
using MediatR;
using Application.Users.Commands.Register;
using Application.Users.Commands.Common;
using Application.Users.Commands.RefreshTokens;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender mediatr) : ControllerBase
{

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterCommand request)
    {
        var response = await mediatr.Send(request);
        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response.Result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginCommand request)
    {
        var response = await mediatr.Send(request);
        if (!response.Success)
        {
            return Unauthorized(response.Message);
        }

        return Ok(response.Result);
    }

    [HttpPost("refresh-tokens")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokensCommand request)
    {
        var response = await mediatr.Send(request);
        if (!response.Success)
        {
            return Unauthorized(response.Message);
        }

        return Ok(response.Result);
    }

    // For testing purposes

    [Authorize]
    [HttpGet("authenticated-only")]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        return Ok("You are authenticated!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("You are an admin!");
    }
}

