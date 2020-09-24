using Microsoft.AspNet.Identity.EntityFramework;
using RecruitmentTask.Models;
using System.Linq;

namespace RecruitmentTask.Services
{
    public interface IAuthService
    {
        IdentityUser GetUserById(string name);
    }

    public class AuthService : IAuthService
    {
        public IdentityUser GetUserById(string name)
        {
            using (var context = ApplicationDbContext.Create())
            {
                return context.Users.SingleOrDefault(u => u.UserName == name);
            }
        }
    }
}