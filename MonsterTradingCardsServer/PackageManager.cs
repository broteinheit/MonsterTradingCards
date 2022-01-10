using MonsterTradingCards.Server.DAL.Repositories.Cards;
using MonsterTradingCards.Server.DAL.Repositories.Package;
using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server
{
    public class PackageManager : IPackageManager
    {
        private readonly IPackageRepository packageRepository;
        private readonly ICardRepository cardRepository;

        public PackageManager(IPackageRepository packageRepository, ICardRepository cardRepository)
        {
            this.packageRepository = packageRepository;
            this.cardRepository = cardRepository;
        }

        public void AddPackage(Package package)
        {
            if (!packageRepository.CreatePackage(package))
            {
                throw new Exception("Could not create package");
            }
        }

        public Package GetRandomPackage()
        {
            var cardIds = packageRepository.AcquireRandomPackage();
            List<Card> cards = new List<Card>() 
            { 
                cardRepository.GetCardById(cardIds[0]),
                cardRepository.GetCardById(cardIds[1]),
                cardRepository.GetCardById(cardIds[2]),
                cardRepository.GetCardById(cardIds[3]),
                cardRepository.GetCardById(cardIds[4])
            };
            return new Package() { Cards = cards };
        }
    }
}
