using Game.Gameplay.Player.Pawn.Core;
using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Player.Pawn.Animations
{
    public class CharacterPawnAnimationsController : ACharacterComponent
    {
        private readonly int m_forwardSpeedAnimationKey = Animator.StringToHash("ForwardSpeed");
        private readonly int m_lateralSpeedAnimationKey = Animator.StringToHash("LateralSpeed");
        
        [SerializeField]
        private float m_movementRoughness = 15f;
        [SerializeField]
        private Animator m_animator;
        private Rigidbody m_rigidbody;
        

        public void SetDependencies(Rigidbody a_rigidbody)
        {
            m_rigidbody = a_rigidbody;
        }

        private void Update()
        {
            if (!m_animator)
                return;

            var relativeVelocity = m_rigidbody.transform.InverseTransformDirection(m_rigidbody.linearVelocity);
            var forwardSpeed = relativeVelocity.z;
            var lateralSpeed = relativeVelocity.x;
            
            m_animator.SetFloat(m_forwardSpeedAnimationKey, Mathf.Lerp(m_animator.GetFloat(m_forwardSpeedAnimationKey), forwardSpeed, m_movementRoughness * Time.deltaTime));
            m_animator.SetFloat(m_lateralSpeedAnimationKey, Mathf.Lerp(m_animator.GetFloat(m_lateralSpeedAnimationKey), lateralSpeed, m_movementRoughness * Time.deltaTime));
        }
    }
}