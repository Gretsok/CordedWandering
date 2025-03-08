using Game.Gameplay.Player.Pawn.Core;
using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Player.Pawn.Movement
{
    public class CharacterGravityApplier : ACharacterComponent
    {
        [SerializeField]
        private float m_gravity = 40f;
        
        private Rigidbody m_rigidbody;

        public void SetDependencies(Rigidbody a_rigidbody)
        {
            m_rigidbody = a_rigidbody;
        }
        
        private void FixedUpdate()
        {
            if (!IsOwner)
                return;
            
            m_rigidbody.AddForce(Vector3.down * m_gravity, ForceMode.Acceleration);
        }
    }
}