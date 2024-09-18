using API.DTOs;
using API.Entites;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public LikesRepository(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext;
            _mapper = mapper;
        }
        public void AddLike(UserLike like)
        {
            _context.Likes.Add(like);
        }

        public void DeleteLike(UserLike like)
        {
            _context.Likes.Remove(like);
        }

        public async Task<IEnumerable<int>> GetCurrecntUserLikeIds(int currecntUserId)
        {
            return await _context.Likes
                .Where(x => x.SourceUserId == currecntUserId)
                .Select(x => x.TargetUserId)
                .ToListAsync();
        }

        public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<MemberDto>> GetUserLike(LikesParams likesParams)
        {
            var likes = _context.Likes.AsQueryable();
            IQueryable<MemberDto> query;

            switch (likesParams.Predicate)
            {
                case "liked":
                    query = likes
                        .Where(x => x.SourceUserId == likesParams.UserId)
                        .Select(x => x.TargetUser)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                    break;

                case "likedBy":
                    query = likes
                        .Where(x => x.TargetUserId == likesParams.UserId)
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                    break;
                default:
                    var likeIds = await GetCurrecntUserLikeIds(likesParams.UserId);
                    query = likes
                        .Where(x => x.TargetUserId == likesParams.UserId && likeIds.Contains(x.SourceUserId))
                        .Select(x => x.SourceUser)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider);
                    break;
            }
            return await PagedList<MemberDto>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
