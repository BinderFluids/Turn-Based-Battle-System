using Core.Enums;
using RequestHub;

namespace Battle.Requests
{
    public struct RequestPlayerId : IRequest
    {
        public PlayerId PlayerId; 
    }
}