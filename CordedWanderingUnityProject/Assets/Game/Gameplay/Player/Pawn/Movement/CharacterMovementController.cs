using System;
using System.Collections.Generic;
using Game.Gameplay.Player.Pawn.Core;
using UnityEngine;

namespace Game.Gameplay.Player.Pawn.Movement
{
    public class CharacterMovementController : ACharacterComponent
    {
        [field: SerializeField]
        public MovementDataAsset MovementDataAsset { get; private set; }
        [field: SerializeField]
        public Transform VerticalSightTransform { get; private set; }
        
        private Rigidbody m_rigidbody;

        public void SetDependencies(Rigidbody a_rigidbody)
        {
            m_rigidbody = a_rigidbody;
        }
        
        #region Inputs Receiving 
        public Vector2 MovementInput { get; private set; }
        public void SetMovementInput(Vector2 a_movementInput)
        {
            MovementInput = a_movementInput;
        }

        public Vector2 LookAroundInput { get; private set; }
        public void SetLookAroundInput(Vector2 a_lookAroundInput)
        {
            LookAroundInput = a_lookAroundInput;
        }
        #endregion

        private void FixedUpdate()
        {
            if (!IsOwner)
                return;

            ApplyMovement();
        }
        
        private void ApplyMovement()
        {
            var planarVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0, m_rigidbody.linearVelocity.z);
            var heightVelocity = m_rigidbody.linearVelocity.y * Vector3.up;
            
            var worldInput = m_rigidbody.transform.forward * MovementInput.y + m_rigidbody.transform.right * MovementInput.x;

            if (worldInput.magnitude > 0.01f)
            {
                var newVelocity = planarVelocity + MovementDataAsset.Acceleration * Time.deltaTime * worldInput;

                var currentMaxSpeed = MovementDataAsset.MaxSpeed * worldInput.magnitude;

                if (currentMaxSpeed < planarVelocity.magnitude)
                {
                    currentMaxSpeed = planarVelocity.magnitude - MovementDataAsset.Deceleration * Time.deltaTime;
                }

                if (newVelocity.magnitude > currentMaxSpeed)
                {
                    newVelocity = newVelocity.normalized * currentMaxSpeed;
                }
                planarVelocity = newVelocity;
            }
            else
            {
                var newVelocity = planarVelocity - planarVelocity.normalized * (MovementDataAsset.Deceleration * Time.deltaTime);
                if (Vector3.Angle(planarVelocity, newVelocity) > 90f)
                {
                    newVelocity = Vector3.zero;
                }
                planarVelocity = newVelocity;
            }
            
            
            m_rigidbody.linearVelocity = planarVelocity + heightVelocity;
        }
        
        private void Update()
        {
            if (!IsOwner)
                return;

            ApplyLookAround();
        }

        private void ApplyLookAround()
        {
            m_rigidbody.transform.Rotate(Vector3.up * (LookAroundInput.x * Time.deltaTime * MovementDataAsset.Sensitivities.x), Space.Self);
            
            var eulerAngles = VerticalSightTransform.localEulerAngles;
            eulerAngles.x += (LookAroundInput.y * Time.deltaTime * MovementDataAsset.Sensitivities.y);
            //Debug.Log(eulerAngles.x);
            VerticalSightTransform.localEulerAngles = eulerAngles;
        }
        
        public void RequestJump()
        {
            var planarVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0, m_rigidbody.linearVelocity.z);
            
            var heightVelocity = MovementDataAsset.JumpInitialSpeed * Vector3.up;
            
            m_rigidbody.linearVelocity = planarVelocity + heightVelocity;
        }
    }
}