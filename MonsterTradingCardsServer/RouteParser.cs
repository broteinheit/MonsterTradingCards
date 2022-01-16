using MonsterTradingCards.Server.Core.Request;
using MonsterTradingCards.Server.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonsterTradingCards.Server
{
    class RouteParser : IRouteParser
    {
        public bool IsMatch(RequestContext request, HttpMethod method, string routePattern)
        {
            if (method != request.Method)
            {
                return false;
            }

            var pattern = "^" + Regex.Replace(routePattern, "{.*}", ".*").Replace("/", "\\/").Replace("?", "\\?.*") + "$";

            return Regex.IsMatch(request.ResourcePath, pattern);
        }

        public Dictionary<string, string> ParseParameters(RequestContext request, string routePattern)
        {
            var parameters = new Dictionary<string, string>();
            var pattern = "^" + routePattern.Replace("{", "(?<").Replace("}", ">.*)").Replace("/", "\\/") + "$";

            var result = Regex.Match(request.ResourcePath, pattern);
            foreach (var key in result.Groups.Keys)
            {
                if (result.Groups[key].Success)
                {
                    parameters[key] = result.Groups[key].Captures[0].Value;
                }
            }

            pattern = "^" + routePattern.Replace("?", "\\?(.*)").Replace("/", "\\/") + "$";
            result = Regex.Match(request.ResourcePath, pattern);
            foreach (var key in result.Groups.Keys)
            {
                if (result.Groups[key].Success)
                {
                    var value = result.Groups[key].Captures[0].Value;
                    if (value.Equals(request.ResourcePath))
                    {
                        parameters.Add("fullPath", value);
                    }
                    else
                    {
                        foreach (var param in result.Groups[key].Captures[0].Value.Split("&"))
                        {
                            var tmp = param.Split("=");
                            if (tmp.Length == 2)
                            {
                                parameters[tmp[0]] = tmp[1];
                            }
                        }
                    }
                }
            }

            return parameters;
        }
    }
}
