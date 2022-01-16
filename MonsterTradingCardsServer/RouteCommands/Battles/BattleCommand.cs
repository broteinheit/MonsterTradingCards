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
    internal class BattleCommand : ProtectedRouteCommand
    {
        private static Queue<BattleHandler> battleQueue = new Queue<BattleHandler>();
        public static List<User> usersInBattleOrQueue { get; private set; } = new List<User>();
        private BattleHandler battle;

        private readonly IUserManager userManager;
        private readonly IDeckManager deckManager;
        private readonly ICardManager cardManager;

        public BattleCommand(IUserManager userManager, IDeckManager deckManager, ICardManager cardManager)
        {
            this.userManager = userManager;
            this.deckManager = deckManager;
            this.cardManager = cardManager;
        }

        public override Response Execute()
        {
            Response response = new Response();

            try
            {
                if (usersInBattleOrQueue.Any(u => u.Username == User.Username))
                {
                    throw new PlayerAlreadInBattleException();
                }

                usersInBattleOrQueue.Add(User);

                lock (battleQueue)
                {
                    if (battleQueue.Count == 0)
                    {
                        battle = new BattleHandler(User, deckManager, cardManager);
                        battleQueue.Enqueue(battle);
                    }
                    else
                    {
                        battle = battleQueue.Dequeue();
                        battle.ConnectPlayer(User);
                    }
                }

                //wait till two players are connected
                while (battle.player1 == null || battle.player2 == null) { Thread.Sleep(100); }

                //player 1 starts battle
                if (battle.player1 == User)
                {
                    //adjust elo for both players after battle
                    switch (battle.StartBattle())
                    {
                        case BattleResult.PLAYER_ONE:
                            userManager.AdjustEloForUser(battle.player1, 3);
                            userManager.AdjustWinLoseForUser(battle.player1, WinLoseDrawColumns.WINNER);
                            userManager.AdjustEloForUser(battle.player2, -5);
                            userManager.AdjustWinLoseForUser(battle.player2, WinLoseDrawColumns.LOSER);
                            break;
                        case BattleResult.PLAYER_TWO:
                            userManager.AdjustEloForUser(battle.player1, -5);
                            userManager.AdjustWinLoseForUser(battle.player1, WinLoseDrawColumns.LOSER);
                            userManager.AdjustEloForUser(battle.player2, 3);
                            userManager.AdjustWinLoseForUser(battle.player2, WinLoseDrawColumns.WINNER);
                            break;
                        case BattleResult.DRAW:
                            userManager.AdjustWinLoseForUser(battle.player1, WinLoseDrawColumns.DRAW);
                            userManager.AdjustWinLoseForUser(battle.player2, WinLoseDrawColumns.DRAW);
                            break;
                    }
                }
                else 
                {
                    //player 2 waits till battle is over
                    while (battle.battleState != BattleState.DONE) { Thread.Sleep(100); }
                }

                response.Payload = battle.battleLogger.GetCompleteLog();
                response.StatusCode = StatusCode.Ok;
            }
            catch (PlayerAlreadInBattleException e)
            {
                response.Payload = $"Player {User.Username} already in Battle or Battle Queue";
                response.StatusCode = StatusCode.BadRequest;
                return response;
            }
            catch (Exception ex)
            {
                response.Payload = ex.Message;
                response.StatusCode = StatusCode.InternalServerError;
            }

            usersInBattleOrQueue.Remove(User);

            return response;
        }
    }
}
