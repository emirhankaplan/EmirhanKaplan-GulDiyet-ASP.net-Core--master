using GulDiyet.Core.Application.Helpers;
using GulDiyet.Core.Application.ViewModels.Users;

namespace GulDiyet.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            UserViewModel userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            if (userViewModel == null)
            {
                return false;
            }

            return true;
        }
    }
}
