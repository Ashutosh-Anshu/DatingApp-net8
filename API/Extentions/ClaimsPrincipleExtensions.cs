using System.Security.Claims;

namespace API.Extentions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string Getusername(this ClaimsPrincipal user)
        {
            try
            {
                var username = user.FindFirstValue(ClaimTypes.Name)
                ?? throw new Exception("cannot get username from token");
                return username;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            try
            {
                var userid = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("cannot get username from token"));
                return userid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
