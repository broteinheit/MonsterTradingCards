using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Users
{
    internal class EditProfileCommand : ProtectedRouteCommand
    {
        private UserInfo userInfo;
        private readonly IUserManager userManager;

        public EditProfileCommand(IUserManager userManager, UserInfo userInfo)
        {
            this.userManager = userManager;
            this.userInfo = userInfo;
        }

        public override Response Execute()
        {
            Response response = new Response();
            try
            {
                if (userInfo.Username != null)
                {
                    throw new Exception("Username is not a parameter! You can only change your own profile!");
                }

                userInfo.Username = User.Username;
                userManager.EditUserInfo(userInfo);
                response.StatusCode = StatusCode.Created;
            } 
            catch (Exception ex)
            {
                response.Payload = ex.Message;
                response.StatusCode = StatusCode.InternalServerError;
            }
            return response;
        }
    }
}
