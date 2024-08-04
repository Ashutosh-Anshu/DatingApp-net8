using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersAsync()
    {
        var users = await _userRepository.GetMemberAsync();
        return Ok(users);
    }

    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<MemberDto>> GetUserByIdAsync(int id)
    // {
    //     var user = await _userRepository.GetUserByIdAsync(id);
    //     return user == null ? NotFound() : user;
    // }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetMemberAsync(username);
        return user == null ? NotFound() : user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdate)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null) return BadRequest("No user found in token");
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null) return BadRequest("Could not find user");
        _mapper.Map(memberUpdate, user);
        if (await _userRepository.SaveAllAync()) return NoContent();
        return BadRequest("Failed to update user");
    }
}
