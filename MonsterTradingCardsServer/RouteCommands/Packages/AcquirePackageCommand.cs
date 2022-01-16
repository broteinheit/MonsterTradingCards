using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Packages
{
    internal class AcquirePackageCommand : ProtectedRouteCommand
    {
        private readonly ICardManager cardManager;
        private readonly IPackageManager packageManager;
        private readonly IUserManager userManager;

        public AcquirePackageCommand(ICardManager cardManager, IPackageManager packageManager, IUserManager userManager)
        {
            this.cardManager = cardManager;
            this.packageManager = packageManager;
            this.userManager = userManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                if (User.Gold < 5)
                {
                    throw new Exception("Not enough Gold");
                }

                Package package = packageManager.GetRandomPackage();
                userManager.AdjustGoldForUser(User, -5);

                foreach (var card in package.Cards)
                {
                    cardManager.ChangeOwner(card, User.Username);
                }

                response.StatusCode = StatusCode.Created;
            }
            catch (Exception e)
            {
                response.StatusCode = StatusCode.Conflict;
                response.Payload = e.Message;
            }

            return response;
        }
    }
}
