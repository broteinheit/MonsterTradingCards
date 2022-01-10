using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Packages
{
    internal class CreatePackageCommand : ProtectedRouteCommand
    {
        private readonly ICardManager cardManager;
        private readonly IPackageManager packageManager;
        public Package package { get; private set; }

        public CreatePackageCommand(ICardManager cardManager, IPackageManager packageManager, Package package)
        {
            this.cardManager = cardManager;
            this.packageManager = packageManager;
            this.package = package;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                foreach (var card in package.Cards)
                {
                    cardManager.AddCard(card);
                }
                packageManager.AddPackage(package);

                response.StatusCode = StatusCode.Created;
            }
            catch (Exception)
            {
                response.StatusCode = StatusCode.Conflict;
            }

            return response;
        }
    }
}
