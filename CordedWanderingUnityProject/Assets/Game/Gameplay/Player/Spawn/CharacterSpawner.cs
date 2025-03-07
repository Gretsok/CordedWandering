using System;
using System.Collections;
using System.Collections.Generic;
using Game.Gameplay.Player.Control;
using Game.Gameplay.Player.Pawn;
using Game.Gameplay.Player.Pawn.LocalCharacterSystem;
using Game.RuntimeDebugTools.Logs;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Gameplay.Player.Spawn
{
    public class CharacterSpawner : NetworkBehaviour
    {
        [SerializeField]
        private PlayerCharacterPawn m_characterPrefab;
        
        [SerializeField]
        private List<Transform> m_spawnPoints;

        [SerializeField]
        private PlayerInputController m_playerInputController;

        [SerializeField]
        private LocalCharacterSystem m_localCharacterSystem;

        [Serializable]
        public class PlayerToCharacterAssociation
        {
            public ulong ClientID;
            public PlayerCharacterPawn Character;
        }
        
        public List<PlayerToCharacterAssociation> PlayerToCharacterAssociations { get; private set; }= new ();

        protected override void OnNetworkPostSpawn()
        {
            base.OnNetworkPostSpawn();

            if (IsServer)
                InitializeAsServer();

            if (IsClient)
                InitializeAsClient();

        }


        private void InitializeAsServer()
        {
            NetworkManager.OnClientConnectedCallback += HandleNewClientConnected;

            for (int i = 0; i < NetworkManager.ConnectedClientsList.Count; i++)
            {
                var client = NetworkManager.ConnectedClientsList[i];
                CreateCharacterFor(client.ClientId);
            }
        }

        private void HandleNewClientConnected(ulong a_obj)
        {
            if (PlayerToCharacterAssociations.Exists(x => x.ClientID == a_obj))
                return;
            CreateCharacterFor(a_obj);
        }

        private void CreateCharacterFor(ulong a_clientId)
        {
            var spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
            var character = Instantiate(m_characterPrefab, spawnPoint.position, spawnPoint.rotation);
            character.GetComponent<NetworkObject>().SpawnWithOwnership(a_clientId);
            
            PlayerToCharacterAssociations.Add(new PlayerToCharacterAssociation() { Character = character, ClientID = a_clientId });
            
            LogConsole.Log($"[CHARACTER][SERVER] Character created for {a_clientId}");
        }
        
        private void InitializeAsClient()
        {
            Debug.Log("Initializing as client");
            AskServerForCharacter_ServerRpc(NetworkManager.LocalClient.ClientId);
        }

        [Rpc(SendTo.Server)]
        private void AskServerForCharacter_ServerRpc(ulong a_clientId)
        {
            Debug.Log("Asking server for character");
            LogConsole.Log($"[CHARACTER][SERVER] Asking character for client {a_clientId}.");
            StartCoroutine(AskServerForCharacter_ServerRpc_Routine(a_clientId));
        }

        private IEnumerator AskServerForCharacter_ServerRpc_Routine(ulong a_clientId)
        {
            yield return new WaitUntil(() =>
            PlayerToCharacterAssociations.Exists(a_association => a_association.ClientID == a_clientId));
            LogConsole.Log($"[CHARACTER][SERVER] Character found for client {a_clientId}.");
            SendCharacterToPlayer_ClientRpc( 
                PlayerToCharacterAssociations.Find(a_association => a_association.ClientID == a_clientId).Character.NetworkObjectId,
                RpcTarget.Single(a_clientId, RpcTargetUse.Temp));
        }

        [Rpc(SendTo.SpecifiedInParams)]
        private void SendCharacterToPlayer_ClientRpc(ulong a_characterId, RpcParams a_params = default)
        {
            LogConsole.Log($"[CHARACTER][CLIENT] Character ID received : {a_characterId}.");
            StartCoroutine(SendCharacterToPlayer_ClientRpc_Routine(a_characterId));
        }

        private IEnumerator SendCharacterToPlayer_ClientRpc_Routine(ulong a_characterId)
        {
            yield return new WaitUntil(() => NetworkManager.SpawnManager.SpawnedObjects.ContainsKey(a_characterId));
            LogConsole.Log("[CHARACTER][CLIENT] Character found locally.");

            var character = NetworkManager.SpawnManager.SpawnedObjects[a_characterId]
                .GetComponent<PlayerCharacterPawn>();
            
            m_playerInputController.SetPlayerCharacterPawn(character);
            m_localCharacterSystem.SetPlayerCharacterPawn(character);
        }

        
    }
}