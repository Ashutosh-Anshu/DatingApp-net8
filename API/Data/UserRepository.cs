using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    public UserRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await _dataContext.Users.FindAsync(id);
    }
    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _dataContext.Users
        .Include(x => x.Photos)
        .ToListAsync();
    }


    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await _dataContext.Users
        .Include(x => x.Photos)
        .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<bool> SaveAllAync()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
    }

    public async Task<IEnumerable<MemberDto>> GetMemberAsync()
    {
        return await _dataContext.Users
               .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
               .ToListAsync();
    }

    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await _dataContext.Users
               .Where(x => x.UserName == username)
               .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync();
    }
}
