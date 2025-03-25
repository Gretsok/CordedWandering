using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Gameplay.Monsters.BehaviourSystem
{
    public class MonsterBehaviourAnchor : NetworkBehaviour, IEquatable<MonsterBehaviourAnchor>
    {
        private readonly NetworkVariable<bool> m_isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        public bool IsActive => m_isActive.Value;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            m_isActive.OnValueChanged += HandleIsActiveValueChanged;


        }

        private void HandleIsActiveValueChanged(bool a_previousValue, bool a_newValue)
        {
            if (a_newValue)
            {
                if (IsServer)
                    NetworkManager.NetworkTickSystem.Tick += HandleServerTick;
                if (IsClient)
                    NetworkManager.NetworkTickSystem.Tick += HandleClientTick;
            }
            else
            {
                if (IsServer)
                    NetworkManager.NetworkTickSystem.Tick -= HandleServerTick;
                if (IsClient)
                    NetworkManager.NetworkTickSystem.Tick -= HandleClientTick;
            }
        }

        #region Start Behaviour
        [SerializeField]
        private UnityEvent m_onBehaviourStarted_Clients;
        [SerializeField]
        private UnityEvent m_onBehaviourStarted_Server;

        public event Action<MonsterBehaviourAnchor> OnBehaviourStarted_Clients;
        public event Action<MonsterBehaviourAnchor> OnBehaviourStarted_Server;
        
        public void StartBehaviour_ServerOnly()
        {
            if (!IsServer)
                return;
            
            HandleBehaviourStarted_Server();
            m_isActive.Value = true;
            m_onBehaviourStarted_Server?.Invoke();
            OnBehaviourStarted_Server?.Invoke(this);
            StartBehaviour_ClientsRpc();
        }

        protected virtual void HandleBehaviourStarted_Clients()
        { }
        protected virtual void HandleBehaviourStarted_Server()
        { }

        [Rpc(SendTo.ClientsAndHost)]
        private void StartBehaviour_ClientsRpc()
        {
            HandleBehaviourStarted_Clients();
            m_onBehaviourStarted_Clients?.Invoke();
            OnBehaviourStarted_Clients?.Invoke(this);
        }
        #endregion
        
        #region Stop Behaviour
        [SerializeField]
        private UnityEvent m_onBehaviourStopped_Clients;
        [SerializeField]
        private UnityEvent m_onBehaviourStopped_Server;

        public event Action<MonsterBehaviourAnchor> OnBehaviourStopped_Clients;
        public event Action<MonsterBehaviourAnchor> OnBehaviourStopped_Server;
        
        public void StopBehaviour_ServerOnly()
        {
            if (!IsServer)
                return;
            
            HandleBehaviourStopped_Server();
            m_isActive.Value = false;
            m_onBehaviourStopped_Server?.Invoke();
            OnBehaviourStopped_Server?.Invoke(this);
            StopBehaviour_ClientsRpc();
        }

        protected virtual void HandleBehaviourStopped_Clients()
        { }
        protected virtual void HandleBehaviourStopped_Server()
        { }

        [Rpc(SendTo.ClientsAndHost)]
        private void StopBehaviour_ClientsRpc()
        {
            HandleBehaviourStopped_Clients();
            m_onBehaviourStopped_Clients?.Invoke();
            OnBehaviourStopped_Clients?.Invoke(this);
        }
        #endregion
        
        #region Update Behaviour
        [SerializeField]
        private UnityEvent m_onBehaviourUpdated_Clients;
        [SerializeField]
        private UnityEvent m_onBehaviourUpdated_Server;
        public event Action<MonsterBehaviourAnchor> OnBehaviourUpdated_Clients;
        public event Action<MonsterBehaviourAnchor> OnBehaviourUpdated_Server;

        private void HandleServerTick()
        {
            m_onBehaviourUpdated_Server?.Invoke();
            OnBehaviourUpdated_Server?.Invoke(this);
        }

        private void HandleClientTick()
        {
            m_onBehaviourUpdated_Clients?.Invoke();
            OnBehaviourUpdated_Clients?.Invoke(this);
        }
        #endregion

        public bool Equals(MonsterBehaviourAnchor a_other)
        {
            if (a_other == null)
                return this == null;
            return a_other.NetworkBehaviourId == this.NetworkBehaviourId;
        }
    }
}