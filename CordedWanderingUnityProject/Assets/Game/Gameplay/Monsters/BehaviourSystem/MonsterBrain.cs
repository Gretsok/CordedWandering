using System.Collections.Generic;
using Unity.Netcode;

namespace Game.Gameplay.Monsters.BehaviourSystem
{
    public class MonsterBrain : NetworkBehaviour
    {
        private readonly NetworkVariable<NetworkBehaviourReference > m_monsterBehaviour =
            new NetworkVariable<NetworkBehaviourReference >();
        public MonsterBehaviourAnchor BehaviourAnchor
        {
            get
            {
                if (m_monsterBehaviour.Value.TryGet(out MonsterBehaviourAnchor anchor))
                    return anchor;
                return null;
            }
        }

        private readonly Queue<MonsterBehaviourAnchor> m_behaviourQueue = new();
        public void QueueNextBehaviour(MonsterBehaviourAnchor a_nextBehaviourAnchor, bool a_queueIfEqualCurrentBehaviour = true)
        {
            if (!IsServer)
                return;

            if (!a_queueIfEqualCurrentBehaviour && a_nextBehaviourAnchor == BehaviourAnchor)
                return;
            
            m_behaviourQueue.Enqueue(a_nextBehaviourAnchor);
        }

        private void Update()
        {
            if (!IsServer)
                return;

            if (m_behaviourQueue.Count > 0)
            {
                var currentState = BehaviourAnchor;
                var nextState = m_behaviourQueue.Dequeue();
                currentState?.StopBehaviour_ServerOnly();
                m_monsterBehaviour.Value = nextState;
                nextState?.StartBehaviour_ServerOnly();
            }
        }
    }
}