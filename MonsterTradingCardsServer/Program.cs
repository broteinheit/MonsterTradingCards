using System;
using System.Net;
using Newtonsoft.Json;
using SWE1HttpServer.Core.Request;
using SWE1HttpServer.Core.Routing;
using SWE1HttpServer.Core.Server;
using SWE1HttpServer.DAL;
using SWE1HttpServer.Models;
using SWE1HttpServer.RouteCommands.Messages;
using SWE1HttpServer.RouteCommands.Users;

namespace SWE1HttpServer
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
            var db = new Database("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=swe1messagedb");

            var messageManager = new MessageManager(db.MessageRepository, db.UserRepository);

            var identityProvider = new MessageIdentityProvider(db.UserRepository);
            var routeParser = new IdRouteParser();

            var router = new Router(routeParser, identityProvider);
            RegisterRoutes(router, messageManager);

            var httpServer = new HttpServer(IPAddress.Any, 10001, router);
            httpServer.Start();
        }

        private static void RegisterRoutes(Router router, IMessageManager messageManager)
        {
            // public routes
            router.AddRoute(HttpMethod.Post, "/sessions", (r, p) => new LoginCommand(messageManager, Deserialize<Credentials>(r.Payload)));
            router.AddRoute(HttpMethod.Post, "/users", (r, p) => new RegisterCommand(messageManager, Deserialize<Credentials>(r.Payload)));

            // protected routes
            router.AddProtectedRoute(HttpMethod.Get, "/messages", (r, p) => new ListMessagesCommand(messageManager));
            router.AddProtectedRoute(HttpMethod.Post, "/messages", (r, p) => new AddMessageCommand(messageManager, r.Payload));
            router.AddProtectedRoute(HttpMethod.Get, "/messages/{id}", (r, p) => new ShowMessageCommand(messageManager, int.Parse(p["id"])));
            router.AddProtectedRoute(HttpMethod.Put, "/messages/{id}", (r, p) => new UpdateMessageCommand(messageManager, int.Parse(p["id"]), r.Payload));
            router.AddProtectedRoute(HttpMethod.Delete, "/messages/{id}", (r, p) => new RemoveMessageCommand(messageManager, int.Parse(p["id"])));
        }

        private static T Deserialize<T>(string payload) where T : class
        {
            var deserializedData = JsonConvert.DeserializeObject<T>(payload);
            return deserializedData;
        }
    }
}
