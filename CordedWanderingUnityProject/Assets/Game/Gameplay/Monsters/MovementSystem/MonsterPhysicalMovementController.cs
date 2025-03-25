using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Monsters.MovementSystem
{
    public class MonsterPhysicalMovementController : AMonsterMovementController
    {
        [field: SerializeField]
        public float Acceleration { get; private set; } = 60f;
        [field: SerializeField]
        public float Deceleration { get; private set; } = 20f;
        [field: SerializeField]
        public float MaxSpeed { get; private set; } = 7f;
        
        public readonly NetworkVariable<Vector3> Destination = new ();
        
        public override void MoveToward(Vector3 a_destination)
        {
            if (!IsServer)
                return;
            
            Destination.Value = a_destination;
        }

        public override void Move(Vector3 a_direction)
        {
            if (!IsServer)
                return;
            
            Destination.Value = ReferencesContainer.Rigidbody.transform.position + a_direction;
        }

        public override void TeleportTo(Vector3 a_destination)
        {
            if (!IsServer)
                return;
            
            Destination.Value = a_destination;
            ReferencesContainer.Rigidbody.transform.position = a_destination;
        }

        protected override void HandleServerTick()
        {
            base.HandleServerTick();
            ApplyMovement();
        }
        
        private void ApplyMovement()
        {
            var rigidbody = ReferencesContainer.Rigidbody;
            var planarVelocity = new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z);
            var heightVelocity = rigidbody.linearVelocity.y * Vector3.up;
            
            var worldInput = (Destination.Value - rigidbody.position).normalized;

            if (worldInput.magnitude > 0.01f)
            {
                var newVelocity = planarVelocity + Acceleration * Time.deltaTime * worldInput;

                var currentMaxSpeed = MaxSpeed * worldInput.magnitude;

                if (currentMaxSpeed < planarVelocity.magnitude)
                {
                    currentMaxSpeed = planarVelocity.magnitude - Deceleration * Time.deltaTime;
                }

                if (newVelocity.magnitude > currentMaxSpeed)
                {
                    newVelocity = newVelocity.normalized * currentMaxSpeed;
                }
                planarVelocity = newVelocity;
            }
            else
            {
                var newVelocity = planarVelocity - planarVelocity.normalized * (Deceleration * Time.deltaTime);
                if (Vector3.Angle(planarVelocity, newVelocity) > 90f)
                {
                    newVelocity = Vector3.zero;
                }
                planarVelocity = newVelocity;
            }
            
            
            rigidbody.linearVelocity = planarVelocity + heightVelocity;
        }
    }
}