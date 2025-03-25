using System.Collections.Generic;
using Unity.Netcode;

namespace Game.Gameplay.Monsters.BehaviourSystem
{
    public class MonsterBrain : NetworkBehaviour
    {
        private readonly NetworkVariable<MonsterBehaviourAnchor> m_monsterBehaviour =
            new NetworkVariable<MonsterBehaviourAnchor>();
        public MonsterBehaviourAnchor BehaviourAnchor => m_monsterBehaviour.Value;

        private readonly Queue<MonsterBehaviourAnchor> m_behaviourQueue = new();
        public void QueueNextBehaviour(MonsterBehaviourAnchor a_nextBehaviourAnchor)
        {
            if (!IsServer)
                return;
            
            m_behaviourQueue.Enqueue(a_nextBehaviourAnchor);
        }

        private void Update()
        {
            if (!IsServer)
                return;

            if (m_behaviourQueue.Count > 0)
            {
                var currentState = m_monsterBehaviour.Value;
                var nextState = m_behaviourQueue.Dequeue();
                currentState.StopBehaviour_ServerOnly();
                m_monsterBehaviour.Value = nextState;
                nextState.StartBehaviour_ServerOnly();
            }
        }
    }
}