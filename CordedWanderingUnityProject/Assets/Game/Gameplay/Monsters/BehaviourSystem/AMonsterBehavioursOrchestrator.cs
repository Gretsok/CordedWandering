using Unity.Netcode;

namespace Game.Gameplay.Monsters.BehaviourSystem
{
    public class AMonsterBehavioursOrchestrator : NetworkBehaviour
    {
        private readonly NetworkVariable<bool> m_isActive = new();
        public bool IsActive => m_isActive.Value;

        public void Activate()
        {
            if (!IsServer)
                return;
            if (IsActive)
                return;
            
            HandleActivation();
            m_isActive.Value = true;
        }
        
        protected virtual void HandleActivation()
        { }

        public void Deactivate()
        {
            if (!IsServer)
                return;
            if (!IsActive)
                return;
            
            HandleDeactivation();
            m_isActive.Value = false;
        }
        
        protected virtual void HandleDeactivation()
        { }
    }
}