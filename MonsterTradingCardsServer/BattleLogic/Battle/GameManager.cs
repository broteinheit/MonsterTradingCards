using MonsterTradingCards.Server.BattleLogic.Logger;
using MonsterTradingCards.Server.DAL.Repositories.Users;
using MonsterTradingCards.Server.Managers;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.BattleLogic.Battle
{
    public class GameManager
    {
        public GameManager(IDeckManager deckManager, ICardManager cardManager, IUserManager userManager)
        {
            this.deckManager = deckManager;
            this.cardManager = cardManager;
            this.userManager = userManager;

            queueThread = new Thread(QueueWorker);
            queueThread.Start();
        }

        ~GameManager()
        {
            running = false;
            queueThread.Join();
        }

        public IBattleLogger SearchForBattle(User user)
        {
            blockedUsers.Add(user);
            usersInQueue.Enqueue(user);

            //wait until picked up by QueueWorker
            while (!battles.Keys.Contains(user)) { Thread.Sleep(50); }
            
            BattleHandler battleHandler;
            battles.Remove(user, out battleHandler);
            
            //wait until battle is done
            while (battleHandler.battleState != BattleState.DONE)
            {
                //if battle handler reports error -> throw exception
                if (battleHandler.battleState == BattleState.NONE)
                {
                    throw new Exception("Error during Battle");
                }

                Thread.Sleep(50);
            }
            lock(blockedUsers)
            {
                blockedUsers.Remove(user);
            }
            return battleHandler.battleLogger;
        }

        


        private void QueueWorker()
        {
            while (running)
            {
                if (usersInQueue.Count >= 2)
                {
                    BattleHandler battle = new BattleHandler(usersInQueue.Dequeue(), usersInQueue.Dequeue(), deckManager, cardManager);
                    battles[battle.player1] = battle;
                    battles[battle.player2] = battle;
                    Task.Run(battle.StartBattle);
                    Task.Run(() => AdjustPlayerStatsAfterBattle(battle));
                } 
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void AdjustPlayerStatsAfterBattle(BattleHandler battle)
        {
            while (battle.battleState != BattleState.DONE)
            {
                if (battle.battleState == BattleState.NONE || !running) { return; }
                Thread.Sleep(100);
            }

            switch (battle.GetResult())
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


        private ConcurrentDictionary<User, BattleHandler> battles = new ConcurrentDictionary<User, BattleHandler>();
        private Queue<User> usersInQueue = new Queue<User>();
        private ICardManager cardManager;
        private IDeckManager deckManager;
        private IUserManager userManager;
        private Thread queueThread;
        private bool running = true;


        public static bool IsPlayerBlocked(string username)
        {
            return blockedUsers.Any(x => x.Username == username);
        }

        private static List<User> blockedUsers = new List<User>();
    }
}
