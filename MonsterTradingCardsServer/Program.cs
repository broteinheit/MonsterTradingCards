using System;
using System.Net;
using MonsterTradingCardsServer.RouteCommands.Cards;
using MonsterTradingCardsServer.RouteCommands.Deck;
using MonsterTradingCardsServer.RouteCommands.Packages;
using MonsterTradingCardsServer.RouteCommands.Users;
using Newtonsoft.Json;
using MonsterTradingCardsServer.Core.Request;
using MonsterTradingCardsServer.Core.Routing;
using MonsterTradingCardsServer.Core.Server;
using MonsterTradingCardsServer.DAL;
using MonsterTradingCardsServer.Models;
using MonsterTradingCardsServer.RouteCommands.Stats;
using MonsterTradingCardsServer.RouteCommands.Scoreboard;
using MonsterTradingCardsServer.RouteCommands.Battles;
using MonsterTradingCardsServer.RouteCommands.Tradings;

namespace MonsterTradingCardsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var messageRepository = new InMemoryMessageRepository();
            //var userRepository = new InMemoryUserRepository();

            // use the DB connection instead
            // better: fetch the connection string from a config file -> see next semester ;)
            // we use an extra Database class to coordinate the creation of the repositories and manage the DB connection
            var db = new Database("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=monster_trading_cards");

            var userManager = new UserManager(db.UserRepository);

            var identityProvider = new MessageIdentityProvider(db.UserRepository);
            var routeParser = new IdRouteParser();

            var router = new Router(routeParser, identityProvider);
            RegisterRoutes(router, userManager);

            var httpServer = new HttpServer(IPAddress.Any, 10001, router);
            httpServer.Start();
        }

        private static void RegisterRoutes(Router router, IUserManager messageManager)
        {
            // public routes
            router.AddRoute(HttpMethod.Post, "/sessions", (r, p) => new LoginCommand(messageManager, Deserialize<Credentials>(r.Payload)));
            router.AddRoute(HttpMethod.Post, "/users", (r, p) => new RegisterCommand(messageManager, Deserialize<Credentials>(r.Payload)));

            // protected routes
            /*
            router.AddProtectedRoute(HttpMethod.Get, "/messages", (r, p) => new ListMessagesCommand(messageManager));
            router.AddProtectedRoute(HttpMethod.Post, "/messages", (r, p) => new AddMessageCommand(messageManager, r.Payload));
            router.AddProtectedRoute(HttpMethod.Get, "/messages/{id}", (r, p) => new ShowMessageCommand(messageManager, int.Parse(p["id"])));
            router.AddProtectedRoute(HttpMethod.Put, "/messages/{id}", (r, p) => new UpdateMessageCommand(messageManager, int.Parse(p["id"]), r.Payload));
            router.AddProtectedRoute(HttpMethod.Delete, "/messages/{id}", (r, p) => new RemoveMessageCommand(messageManager, int.Parse(p["id"])));
            */

            router.AddProtectedRoute(HttpMethod.Post, "/packages", (r, p) => new CreatePackageCommand());
            router.AddProtectedRoute(HttpMethod.Post, "/transactions/packages", (r, p) => new AcquirePackageCommand());

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
