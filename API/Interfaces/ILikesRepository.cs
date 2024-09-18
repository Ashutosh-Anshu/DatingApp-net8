using API.DTOs;
using API.Entites;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        public Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
        Task<PagedList<MemberDto>> GetUserLike(LikesParams likesParams);
        Task<IEnumerable<int>> GetCurrecntUserLikeIds(int currecntUserId);
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);
        Task<bool> SaveChanges();

    }
}
