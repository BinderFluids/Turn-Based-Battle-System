using Core.Enums;
using RequestHub;

namespace Battle.Requests
{
    public struct RequestablePlayerId : IRequestable
    {
        public PlayerId PlayerId; 
    }
}