using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCards.Server.Models;

namespace MonsterTradingCards.Server.DAL.Repositories.Package
{
    public interface IPackageRepository
    {
        bool CreatePackage(Models.Package package);
        List<string> AcquirePackage();
    }
}
