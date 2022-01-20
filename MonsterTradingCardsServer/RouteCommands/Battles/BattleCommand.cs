using MonsterTradingCards.Server.BattleLogic.Battle;
using MonsterTradingCards.Server.Core.Response;
using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using MonsterTradingCards.Server.RouteCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.RouteCommands.Battles
{
    public class BattleCommand : ProtectedRouteCommand
    {
        private readonly GameManager lobbyManager;

        public BattleCommand(GameManager lobbyManager)
        {
            this.lobbyManager = lobbyManager;
        }

        public override Response Execute()
        {
            Response response = new Response();

            try
            {
                if (GameManager.IsPlayerBlocked(User.Username))
                {
                    throw new PlayerAlreadInBattleException();
                }

                response.Payload = lobbyManager.SearchForBattle(User).GetCompleteLog();
                response.StatusCode = StatusCode.Ok;
            }
            catch (PlayerAlreadInBattleException)
            {
                response.Payload = $"Player {User.Username} already in Battle or Battle Queue";
                response.StatusCode = StatusCode.BadRequest;
                return response;
            }
            catch (Exception ex)
            {
                response.Payload = ex.StackTrace + "\n" + ex.Message;
                response.StatusCode = StatusCode.publicServerError;
            }

            return response;
        }
    }
}
