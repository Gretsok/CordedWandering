using Game.RuntimeDebugTools.Logs;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.ConnectionManagement
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkManagerHandler : MonoBehaviour
    {
        public NetworkManager NetworkManager { get; private set; }

        private void Awake()
        {
            NetworkManager = GetComponent<NetworkManager>();
        }

        private void Start()
        {
            NetworkManager.OnClientStarted += HandleClientStarted;
            NetworkManager.OnClientStopped += HandleClientStopped;
            NetworkManager.OnClientConnectedCallback += HandleClientConnectedCallback;
            NetworkManager.OnClientDisconnectCallback += HandleClientDisconnectCallback;
            NetworkManager.OnServerStarted += HandleServerStarted;
            NetworkManager.OnServerStopped += HandleServerStopped;
            NetworkManager.OnConnectionEvent += HandleConnectionEvent;
            NetworkManager.OnTransportFailure += HandleTransportFailure;
            NetworkManager.OnSessionOwnerPromoted += HandleSessionOwnerPromoted;
        }

        private void HandleClientStarted()
        {
            LogConsole.Log("[NET] Client started");
        }

        private void HandleClientStopped(bool a_obj)
        {
            LogConsole.Log($"[NET] Client stopped, value : {a_obj}");
        }

        private void HandleClientConnectedCallback(ulong a_obj)
        {
            LogConsole.Log($"[NET] Client connected : {a_obj}");
        }

        private void HandleClientDisconnectCallback(ulong a_obj)
        {
            LogConsole.Log($"[NET] Client disconnected : {a_obj}");
        }

        private void HandleServerStarted()
        {
            LogConsole.Log("[NET] Server started");
            NetworkManager.SceneManager.LoadScene("GameplayScene", LoadSceneMode.Single);
        }

        private void HandleServerStopped(bool a_obj)
        {
            LogConsole.Log($"[NET] Server stopped, value : {a_obj}");
        }

        private void HandleConnectionEvent(NetworkManager a_arg1, ConnectionEventData a_arg2)
        {
            LogConsole.Log($"[NET] Connection event : {a_arg2.EventType} for {a_arg2.ClientId}");
        }

        private void HandleTransportFailure()
        {
            LogConsole.LogError($"[NET] Handle Transport Failure");
        }

        private void HandleSessionOwnerPromoted(ulong a_sessionOwnerPromoted)
        {
            LogConsole.Log($"[NET] Session owner promoted value : {a_sessionOwnerPromoted}");
        }
    }
}