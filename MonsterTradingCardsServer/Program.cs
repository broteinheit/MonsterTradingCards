using System;
using System.Net;
using MonsterTradingCards.Server.RouteCommands.Cards;
using MonsterTradingCards.Server.RouteCommands.Deck;
using MonsterTradingCards.Server.RouteCommands.Packages;
using MonsterTradingCards.Server.RouteCommands.Users;
using Newtonsoft.Json;
using MonsterTradingCards.Server.Core.Request;
using MonsterTradingCards.Server.Core.Routing;
using MonsterTradingCards.Server.Core.Server;
using MonsterTradingCards.Server.DAL;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.RouteCommands.Stats;
using MonsterTradingCards.Server.RouteCommands.Scoreboard;
using MonsterTradingCards.Server.RouteCommands.Battles;
using MonsterTradingCards.Server.RouteCommands.Tradings;
using System.Collections.Generic;

namespace MonsterTradingCards.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Database("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=monster_trading_cards");

            var userManager = new UserManager(db.UserRepository);
            var cardManager = new CardManager(db.CardRepository);
            var packageManager = new PackageManager(db.PackageRepository, db.CardRepository);

            var identityProvider = new UserIdentityProvider(db.UserRepository);
            var routeParser = new IdRouteParser();

            

            var router = new Router(routeParser, identityProvider);
            RegisterRoutes(router, userManager, cardManager, packageManager);

            var httpServer = new HttpServer(IPAddress.Any, 10001, router);
            httpServer.Start();
        }

        private static void RegisterRoutes(Router router, IUserManager userManager, ICardManager cardManager, IPackageManager packageManager)
        {
            // public routes
            router.AddRoute(HttpMethod.Post, "/sessions", (r, p) => new LoginCommand(userManager, Deserialize<Credentials>(r.Payload)));
            router.AddRoute(HttpMethod.Post, "/users", (r, p) => new RegisterCommand(userManager, Deserialize<Credentials>(r.Payload)));

            // protected routes
            router.AddProtectedRoute(HttpMethod.Post, "/packages", (r, p) => new CreatePackageCommand(cardManager, packageManager, 
                new Package() { Cards=Deserialize<List<Card>>(r.Payload) }));
            router.AddProtectedRoute(HttpMethod.Post, "/transactions/packages", (r, p) => new AcquirePackageCommand(cardManager, packageManager, userManager));

            router.AddProtectedRoute(HttpMethod.Get, "/cards", (r, p) => new ShowCardsCommand());

            router.AddProtectedRoute(HttpMethod.Get, "/deck", (r, p) => new ShowDeckCommand());
            router.AddProtectedRoute(HttpMethod.Put, "/deck", (r, p) => new ConfigureDeckCommand());
            //show deck different representation

            router.AddProtectedRoute(HttpMethod.Get, "/users/{username}", (r, p) => new GetProfileCommand());
            router.AddProtectedRoute(HttpMethod.Put, "/users/{username}", (r, p) => new EditProfileCommand());

            router.AddProtectedRoute(HttpMethod.Get, "/stats", (r, p) => new GetStatsCommand());

            router.AddProtectedRoute(HttpMethod.Get, "/score", (r, p) => new ScoreboardCommand());

            router.AddProtectedRoute(HttpMethod.Post, "/battles", (r, p) => new BattleCommand());

            router.AddProtectedRoute(HttpMethod.Get, "/tradings", (r, p) => new GetTradingDealsCommand());
            router.AddProtectedRoute(HttpMethod.Post, "/tradings", (r, p) => new CreateTradingDealCommand());
            router.AddProtectedRoute(HttpMethod.Delete, "/tradings/{tradeId}", (r, p) => new DeleteTradingDealCommand());
            router.AddProtectedRoute(HttpMethod.Post, "/tradings/{tradeId}", (r, p) => new TradeCommand());
        }

        private static T Deserialize<T>(string payload) where T : class
        {
            var deserializedData = JsonConvert.DeserializeObject<T>(payload);
            return deserializedData;
        }
    }
}
