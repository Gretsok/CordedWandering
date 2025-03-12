using System;
using System.Collections.Generic;
using Game.Gameplay.Player.Pawn.Core;
using Unity.Netcode;
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
        private NetworkVariable<float> m_sightXValue 
            = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient && !IsOwner)
            {
                m_sightXValue.OnValueChanged += HandleSightXValueChangedOnOtherClients;
            }
        }

        private void HandleSightXValueChangedOnOtherClients(float a_previousvalue, float a_newvalue)
        {
            var eulerAngles = new Vector3(a_newvalue, 0f, 0f);
            VerticalSightTransform.localEulerAngles = eulerAngles;
        }

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
            ApplyLookAround();
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
  
        private void ApplyLookAround()
        {
            m_rigidbody.transform.Rotate(Vector3.up * (LookAroundInput.x * Time.deltaTime * MovementDataAsset.Sensitivities.x), Space.Self);
            
            var eulerAngles = VerticalSightTransform.localEulerAngles;
            eulerAngles.x += (-LookAroundInput.y * Time.deltaTime * MovementDataAsset.Sensitivities.y);
            if (eulerAngles.x > 180f)
            {
                if (eulerAngles.x < 360f - MovementDataAsset.SightVerticalLimits.x)
                {
                    eulerAngles.x = 360f - MovementDataAsset.SightVerticalLimits.x;
                }
            }
            else
            {
                if (eulerAngles.x > MovementDataAsset.SightVerticalLimits.y)
                {
                    eulerAngles.x = MovementDataAsset.SightVerticalLimits.y;
                }
                
            }
            //Debug.Log(eulerAngles.x);
            m_sightXValue.Value = eulerAngles.x;
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