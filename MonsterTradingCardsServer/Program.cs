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
using MonsterTradingCards.Server.BattleLogic.Battle;
using MonsterTradingCards.Server.RouteCommands.Sacrifice;

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
            var tradingsManager = new TradingsManager(db.TradingsRepository, db.CardRepository);

            var gameManager = new GameManager(deckManager, cardManager, userManager);

            var identityProvider = new UserIdentityProvider(db.UserRepository);
            var routeParser = new RouteParser();

            

            var router = new Router(routeParser, identityProvider);
            RegisterRoutes(router, userManager, cardManager, packageManager, deckManager, tradingsManager, gameManager);

            var httpServer = new HttpServer(IPAddress.Any, 10001, router);
            httpServer.Start();
        }

        private static void RegisterRoutes(Router router, IUserManager userManager, ICardManager cardManager, 
            IPackageManager packageManager, IDeckManager deckManager, ITradingsManager tradingsManager, GameManager gameManager)
        {
            // public routes
            router.AddRoute(HttpMethod.Post, "/sessions", (r, p) => new LoginCommand(userManager, Deserialize<Credentials>(r.Payload)));
            router.AddRoute(HttpMethod.Post, "/users", (r, p) => new RegisterCommand(userManager, Deserialize<Credentials>(r.Payload)));

            // protected routes
            router.AddProtectedRoute(HttpMethod.Post, "/packages", (r, p) => new CreatePackageCommand(cardManager, packageManager, 
                new Package() { Cards=Deserialize<List<Card>>(r.Payload) }));
            router.AddProtectedRoute(HttpMethod.Post, "/transactions/packages", (r, p) => new AcquirePackageCommand(cardManager, packageManager, userManager));

            router.AddProtectedRoute(HttpMethod.Get, "/cards", (r, p) => new ShowCardsCommand(cardManager));

            router.AddProtectedRoute(HttpMethod.Get, "/deck", (r, p) => new ShowDeckCommand(deckManager, cardManager));
            router.AddProtectedRoute(HttpMethod.Get, "/deck?", (r, p) => new ShowDeckCommand(deckManager, cardManager, p["format"]));
            router.AddProtectedRoute(HttpMethod.Put, "/deck", (r, p) => new ConfigureDeckCommand(deckManager, cardManager, Deserialize<List<string>>(r.Payload)));
            //show deck different representation

            router.AddProtectedRoute(HttpMethod.Get, "/users/{username}", (r, p) => new GetProfileCommand(userManager, p["username"]));
            router.AddProtectedRoute(HttpMethod.Put, "/users/{username}", (r, p) => new EditProfileCommand(userManager, p["username"], Deserialize<UserInfo>(r.Payload)));

            router.AddProtectedRoute(HttpMethod.Get, "/stats", (r, p) => new GetStatsCommand(userManager));

            router.AddProtectedRoute(HttpMethod.Get, "/score", (r, p) => new ScoreboardCommand(userManager));

            router.AddProtectedRoute(HttpMethod.Post, "/battles", (r, p) => new BattleCommand(gameManager));

            router.AddProtectedRoute(HttpMethod.Get, "/tradings", (r, p) => new GetTradingDealsCommand(tradingsManager));
            router.AddProtectedRoute(HttpMethod.Post, "/tradings", (r, p) => new CreateTradingDealCommand(tradingsManager, Deserialize<TradeSerializedObject>(r.Payload)));
            router.AddProtectedRoute(HttpMethod.Delete, "/tradings/{tradeId}", (r, p) => new DeleteTradingDealCommand(tradingsManager, p["tradeId"]));
            router.AddProtectedRoute(HttpMethod.Post, "/tradings/{tradeId}", (r, p) => new TradeCommand(tradingsManager, p["tradeId"], Deserialize<string>(r.Payload)));

            //unique feature
            router.AddProtectedRoute(HttpMethod.Post, "/sacrifice", (r, p) => new SacrificeCommand(cardManager, deckManager, Deserialize<SacrificeModel>(r.Payload)));
        }

        private static T Deserialize<T>(string payload) where T : class
        {
            var deserializedData = JsonConvert.DeserializeObject<T>(payload);
            return deserializedData;
        }
    }
}
