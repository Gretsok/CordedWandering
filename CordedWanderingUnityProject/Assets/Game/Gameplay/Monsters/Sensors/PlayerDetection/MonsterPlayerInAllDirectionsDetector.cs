using System;
using Game.Gameplay.Player.Pawn;
using Game.Gameplay.Player.Pawn.Collision;
using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Monsters.Sensors.PlayerDetection
{
    public class MonsterPlayerInAllDirectionsDetector : NetworkBehaviour
    {
        [SerializeField]
        private float m_detectionDistance = 10f;

        [SerializeField]
        private Vector3 m_localOffset;
        
        public readonly NetworkList<NetworkBehaviourReference> PlayersInSight = new NetworkList<NetworkBehaviourReference>();
        
        private void FixedUpdate()
        {
            if (!IsServer)
                return;
            
            var detectedColliders = Physics.OverlapSphere(transform.InverseTransformPoint(m_localOffset), m_detectionDistance);
            PlayersInSight.Clear();
            foreach (var detectedCollider in detectedColliders)
                if (detectedCollider.TryGetComponent(out PlayerCharacterPawnCollider playerCharacterPawnCollider))
                    PlayersInSight.Add(playerCharacterPawnCollider.PlayerCharacterPawn);
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = PlayersInSight is { Count: > 0 } ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.InverseTransformPoint(m_localOffset), m_detectionDistance);
        }
#endif
    }
}