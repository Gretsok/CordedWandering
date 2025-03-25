using Game.Gameplay.Monsters.BehaviourSystem;
using UnityEngine;

namespace Game.Gameplay.Monsters.MonstersIntegrations.LittleGirl
{
    public class LittleGirlBehavioursOrchestrator : AMonsterBehavioursOrchestrator
    {
        [SerializeField]
        private MonsterBrain m_brain;
        
        [Header("Behaviours Anchors")]
        [SerializeField]
        private MonsterBehaviourAnchor m_idleBehaviourAnchor;
        [SerializeField]
        private MonsterBehaviourAnchor m_moveBehaviourAnchor;
    }
}