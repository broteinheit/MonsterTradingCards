using MonsterTradingCards.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server
{
    public interface IPackageManager
    {
        void AddPackage(Package package);
        Package GetRandomPackage();
    }
}
