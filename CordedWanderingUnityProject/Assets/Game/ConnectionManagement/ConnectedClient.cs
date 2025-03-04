using Unity.Netcode;

namespace Game.ConnectionManagement
{
    public class ConnectedClient : NetworkBehaviour
    {
        public string ClientName { get; private set; }
    }
}