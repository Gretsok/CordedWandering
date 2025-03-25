using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Monsters.BehaviourSystem
{
    public abstract class AMonsterBehaviour : NetworkBehaviour
    {
        [field: SerializeField]
        public MonsterBehaviourAnchor Anchor { get; private set; }

        private void Awake()
        {
            if (!Anchor)
                Anchor = GetComponent<MonsterBehaviourAnchor>();

            if (!Anchor)
                return;

            Anchor.OnBehaviourStarted_Clients += HandleBehaviourStarted_Clients;
            Anchor.OnBehaviourStarted_Server += HandleBehaviourStarted_Server;
            Anchor.OnBehaviourUpdated_Clients += HandleBehaviourUpdated_Clients;
            Anchor.OnBehaviourUpdated_Server += HandleBehaviourUpdated_Server;
            Anchor.OnBehaviourStopped_Clients += HandleBehaviourStopped_Clients;
            Anchor.OnBehaviourStopped_Server += HandleBehaviourStopped_Server;
        }

        protected virtual void HandleBehaviourStarted_Clients(MonsterBehaviourAnchor a_obj)
        { }

        protected virtual void HandleBehaviourStarted_Server(MonsterBehaviourAnchor a_obj)
        { }

        protected virtual void HandleBehaviourUpdated_Clients(MonsterBehaviourAnchor a_obj)
        { }

        protected virtual void HandleBehaviourUpdated_Server(MonsterBehaviourAnchor a_obj)
        { }

        protected virtual void HandleBehaviourStopped_Clients(MonsterBehaviourAnchor a_obj)
        { }

        protected virtual void HandleBehaviourStopped_Server(MonsterBehaviourAnchor a_obj)
        { }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (!Anchor)
                return;
            
            
            Anchor.OnBehaviourStarted_Clients -= HandleBehaviourStarted_Clients;
            Anchor.OnBehaviourStarted_Server -= HandleBehaviourStarted_Server;
            Anchor.OnBehaviourUpdated_Clients -= HandleBehaviourUpdated_Clients;
            Anchor.OnBehaviourUpdated_Server -= HandleBehaviourUpdated_Server;
            Anchor.OnBehaviourStopped_Clients -= HandleBehaviourStopped_Clients;
            Anchor.OnBehaviourStopped_Server -= HandleBehaviourStopped_Server;
        }
    }
}