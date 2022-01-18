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
        private string username;
        private readonly IUserManager userManager;

        public EditProfileCommand(IUserManager userManager, string username, UserInfo userInfo)
        {
            this.userManager = userManager;
            this.username = username;
            this.userInfo = userInfo;
        }

        public override Response Execute()
        {
            Response response = new Response();
            try
            {
                //check if user tries to access somebody else's profile
                if (username != User.Username)
                {
                    throw new Exception("Invalid Username! You can only change your own Profile!");
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
