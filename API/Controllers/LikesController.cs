using API.DTOs;
using API.Entites;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(ILikesRepository likesRepository) : BaseApiController
    {
        [HttpPost("{targetUserid:int}")]
        public async Task<IActionResult> ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();
            if (sourceUserId == targetUserId) return BadRequest("You cannot like Yourself");
            var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);
            if (existingLike == null)
            {
                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId,
                };
                likesRepository.AddLike(like);
            }
            else
            {
                likesRepository.DeleteLike(existingLike);
            }
            if (await likesRepository.SaveChanges()) return Ok();
            return BadRequest("Failed to update likes");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            return Ok(await likesRepository.GetCurrecntUserLikeIds(User.GetUserId()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLike([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await likesRepository.GetUserLike(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }

    }



}
