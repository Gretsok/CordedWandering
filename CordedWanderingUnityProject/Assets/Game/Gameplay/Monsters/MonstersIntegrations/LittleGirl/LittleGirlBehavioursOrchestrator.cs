using Game.Gameplay.Monsters.BehaviourSystem;
using Game.Gameplay.Monsters.Sensors.PlayerDetection;
using Game.Gameplay.Player.Pawn;
using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Monsters.MonstersIntegrations.LittleGirl
{
    public class LittleGirlBehavioursOrchestrator : AMonsterBehavioursOrchestrator
    {
        [SerializeField]
        private MonsterBrain m_brain;
        
        [Header("Sensors")]
        [SerializeField]
        private MonsterPlayerInAllDirectionsDetector m_playerDetector;
        
        [Header("Behaviours Anchors")]
        [SerializeField]
        private MonsterBehaviourAnchor m_idleBehaviourAnchor;
        [SerializeField]
        private MonsterBehaviourAnchor m_moveBehaviourAnchor;

        // Working values
        private readonly NetworkVariable<NetworkBehaviourReference> m_target = new ();

        public PlayerCharacterPawn PlayerCharacterPawn
        {
            get
            {
                if (m_target != null && m_target.Value.TryGet(out PlayerCharacterPawn player))
                    return player;
                return null;
            }
        }

        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsServer)
                return;

            NetworkManager.NetworkTickSystem.Tick += HandleTick_Server;
            m_brain.QueueNextBehaviour(m_idleBehaviourAnchor);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (!IsServer)
                return;
            
            NetworkManager.NetworkTickSystem.Tick -= HandleTick_Server;
        }

        private void HandleTick_Server()
        {
            UpdateTarget();

            if (m_target.Value.TryGet(out Player.Pawn.PlayerCharacterPawn player))
            {
                m_brain.QueueNextBehaviour(m_moveBehaviourAnchor, false);
            }
            else
            {
                m_brain.QueueNextBehaviour(m_idleBehaviourAnchor, false);
            }
        }

        private void UpdateTarget()
        {
            var playersInSight = m_playerDetector.PlayersInSight;
            if (m_target.Value.TryGet(out Player.Pawn.PlayerCharacterPawn player))
                if (playersInSight.Contains(player))
                    return;
            
            var closestDistance = float.MaxValue;
            PlayerCharacterPawn closestPlayer = null;
            for (int i = 0; i < playersInSight.Count; ++i)
            {
                if (!playersInSight[i].TryGet(out closestPlayer))
                    continue;
                var distanceToPlayer = Vector3.Distance(closestPlayer.transform.position, transform.position);
                if (!closestPlayer
                    || distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    closestPlayer = closestPlayer;
                }
            }
            m_target.Value = closestPlayer;
        }
    }
}