using System;
using Game.Gameplay.Monsters.ReferencesContainer;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Gameplay.Monsters.MovementSystem
{
    public abstract class AMonsterMovementController : NetworkBehaviour
    {
        public abstract void MoveToward(Vector3 a_destination);
        public abstract void Move(Vector3 a_direction);
        public abstract void TeleportTo(Vector3 a_destination);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsServer)
                NetworkManager.NetworkTickSystem.Tick += HandleServerTick;
            if (IsClient)
                NetworkManager.NetworkTickSystem.Tick += HandleClientTick;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsServer)
                NetworkManager.NetworkTickSystem.Tick += HandleServerTick;
            if (IsClient)
                NetworkManager.NetworkTickSystem.Tick += HandleClientTick;
        }

        private readonly NetworkVariable<bool> m_isActivated = new();
        public bool IsActivated => m_isActivated.Value;

        [SerializeField]
        private UnityEvent m_onActivated_Clients;
        [SerializeField]
        private UnityEvent m_onActivated_Server;
        public event Action<AMonsterMovementController> OnActivated_Clients;
        public event Action<AMonsterMovementController> OnActivated_Server;

        [SerializeField]
        private UnityEvent m_onDeactivated_Clients;
        [SerializeField]
        private UnityEvent m_onDeactivated_Server;
        public event Action<AMonsterMovementController> OnDeactivated_Clients;
        public event Action<AMonsterMovementController> OnDeactivated_Server;

        public void Activate_ServerOnly()
        {
            if (!IsServer)
                return;

            HandleActivation();
            m_isActivated.Value = true;
            m_onActivated_Server?.Invoke();
            OnActivated_Server?.Invoke(this);
            Activate_ClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void Activate_ClientRpc()
        {
            m_onActivated_Clients?.Invoke();
            OnActivated_Clients?.Invoke(this);
        }

        protected virtual void HandleActivation()
        { }

        public void Deactivate_ServerOnly()
        {
            if (!IsServer)
                return;

            HandleDeactivation();
            m_isActivated.Value = false;
            m_onDeactivated_Server?.Invoke();
            OnDeactivated_Server?.Invoke(this);
            Deactivate_ClientRpc();
        }

        protected virtual void HandleDeactivation()
        { }

        [Rpc(SendTo.ClientsAndHost)]
        private void Deactivate_ClientRpc()
        {
            m_onDeactivated_Clients.Invoke();
            OnDeactivated_Clients?.Invoke(this);
        }

        protected virtual void HandleClientTick()
        { }

        protected virtual void HandleServerTick()
        { }

        public MonsterReferencesContainer ReferencesContainer { get; private set; }

        public void SetReferencesContainer(MonsterReferencesContainer a_references)
        {
            ReferencesContainer = a_references;
        }
    }
}