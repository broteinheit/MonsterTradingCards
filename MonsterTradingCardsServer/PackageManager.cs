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

        public PackageManager(IPackageRepository packageRepository)
        {
            this.packageRepository = packageRepository;
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
            throw new NotImplementedException();
        }
    }
}
