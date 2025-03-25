using System;
using Game.Gameplay.Monsters.BehaviourSystem;
using Game.Gameplay.Monsters.Character;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Gameplay.Monsters
{
    public class MonsterPawn : NetworkBehaviour
    {
        public enum EActivationState
        {
            Deactivated = 0,
            ShownAndActivated = 1,
            Hidden = 2
        }

        [Header("Generic References")]
        [SerializeField]
        private MonsterCharacter m_character;

        [SerializeField]
        private AMonsterBehavioursOrchestrator m_behavioursOrchestrator;

        
        private readonly NetworkVariable<EActivationState> m_activationState = new();
        public EActivationState ActivationState => m_activationState.Value;

        [Header("Events")]
        [SerializeField]
        private UnityEvent m_onShownAndActivated_Server;
        public event Action<MonsterPawn> OnShownAndActivated_Server;
        [SerializeField]
        private UnityEvent m_onShownAndActivated_Clients;
        public event Action<MonsterPawn> OnShownAndActivated_Clients;
        public void ActivateAndShow()
        {
            if (!IsServer)
                return;
            
            m_activationState.Value = EActivationState.ShownAndActivated;
            m_onShownAndActivated_Server?.Invoke();
            OnShownAndActivated_Server?.Invoke(this);
            ActivateAndShow_ClientRpc();
            
            if (m_behavioursOrchestrator)
                m_behavioursOrchestrator.Activate();
            if (m_character)
                m_character.Show();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void ActivateAndShow_ClientRpc()
        {
            m_onShownAndActivated_Clients?.Invoke();
            OnShownAndActivated_Clients?.Invoke(this);
        }
        
        [SerializeField]
        private UnityEvent m_onDeactivated_Server;
        public event Action<MonsterPawn> OnDeactivated_Server;
        [SerializeField]
        private UnityEvent m_onDeactivated_Clients;
        public event Action<MonsterPawn> OnDeactivated_Clients;
        public void Deactivate()
        {
            if (!IsServer)
                return;
            
            m_activationState.Value = EActivationState.Deactivated;
            m_onDeactivated_Server?.Invoke();
            OnDeactivated_Server?.Invoke(this);
            Deactivate_ClientRpc();
            
            if (m_behavioursOrchestrator)
                m_behavioursOrchestrator.Deactivate();
            if (m_character)
                m_character.Hide();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void Deactivate_ClientRpc()
        {
            m_onDeactivated_Clients?.Invoke();
            OnDeactivated_Clients?.Invoke(this);
        }
        
        [SerializeField]
        private UnityEvent m_onHidden_Server;
        public event Action<MonsterPawn> OnHidden_Server;
        [SerializeField]
        private UnityEvent m_onHidden_Clients;
        public event Action<MonsterPawn> OnHidden_Clients;
        public void Hide()
        {
            if (!IsServer)
                return;
            
            m_activationState.Value = EActivationState.Hidden;
            m_onHidden_Server?.Invoke();
            OnHidden_Server?.Invoke(this);
            Hide_ClientRpc();
            
            if (m_behavioursOrchestrator)
                m_behavioursOrchestrator.Activate();
            if (m_character)
                m_character.Hide();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void Hide_ClientRpc()
        {
            m_onHidden_Clients?.Invoke();
            OnHidden_Clients?.Invoke(this);
        }
    }
}