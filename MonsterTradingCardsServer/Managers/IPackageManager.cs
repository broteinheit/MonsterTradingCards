using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Managers
{
    public interface IPackageManager
    {
        void AddPackage(Package package);
        Package GetPackage();
    }
}
