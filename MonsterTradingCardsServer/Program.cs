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
using MonsterTradingCards.Server.Managers;

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
            var deckManager = new DeckManager(db.DeckRepository);

            var identityProvider = new UserIdentityProvider(db.UserRepository);
            var routeParser = new RouteParser();

            

            var router = new Router(routeParser, identityProvider);
            RegisterRoutes(router, userManager, cardManager, packageManager, deckManager);

            var httpServer = new HttpServer(IPAddress.Any, 10001, router);
            httpServer.Start();
        }

        private static void RegisterRoutes(Router router, IUserManager userManager, ICardManager cardManager, IPackageManager packageManager, IDeckManager deckManager)
        {
            // public routes
            router.AddRoute(HttpMethod.Post, "/sessions", (r, p) => new LoginCommand(userManager, Deserialize<Credentials>(r.Payload)));
            router.AddRoute(HttpMethod.Post, "/users", (r, p) => new RegisterCommand(userManager, Deserialize<Credentials>(r.Payload)));

            // protected routes
            router.AddProtectedRoute(HttpMethod.Post, "/packages", (r, p) => new CreatePackageCommand(cardManager, packageManager, 
                new Package() { Cards=Deserialize<List<Card>>(r.Payload) }));
            router.AddProtectedRoute(HttpMethod.Post, "/transactions/packages", (r, p) => new AcquirePackageCommand(cardManager, packageManager, userManager));

            router.AddProtectedRoute(HttpMethod.Get, "/cards", (r, p) => new ShowCardsCommand(cardManager));

            router.AddProtectedRoute(HttpMethod.Get, "/deck", (r, p) => new ShowDeckCommand(deckManager));
            router.AddProtectedRoute(HttpMethod.Get, "/deck?", (r, p) => new ShowDeckCommand(deckManager, p["format"]));
            router.AddProtectedRoute(HttpMethod.Put, "/deck", (r, p) => new ConfigureDeckCommand(deckManager, cardManager, Deserialize<List<string>>(r.Payload)));
            //show deck different representation

            router.AddProtectedRoute(HttpMethod.Get, "/users/{username}", (r, p) => new GetProfileCommand(userManager, p["username"]));
            router.AddProtectedRoute(HttpMethod.Put, "/users/{username}", (r, p) => new EditProfileCommand(userManager, Deserialize<UserInfo>(r.Payload)));

            router.AddProtectedRoute(HttpMethod.Get, "/stats", (r, p) => new GetStatsCommand(userManager));

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
