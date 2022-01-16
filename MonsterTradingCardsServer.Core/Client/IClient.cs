using MonsterTradingCards.Server.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server.Core.Client
{
    public interface IClient
    {
        public RequestContext ReceiveRequest();
        public void SendResponse(Response.Response response);
    }
}
