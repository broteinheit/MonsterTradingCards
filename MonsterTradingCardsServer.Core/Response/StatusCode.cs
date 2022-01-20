namespace MonsterTradingCards.Server.Core.Response
{
    public enum StatusCode
    {
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        publicServerError = 500,
        NotImplemented = 501
    }
}