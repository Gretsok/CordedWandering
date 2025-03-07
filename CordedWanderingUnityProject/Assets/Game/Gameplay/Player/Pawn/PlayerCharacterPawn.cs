using Game.Gameplay.Player.Pawn.Movement;
using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Player.Pawn
{
    public class PlayerCharacterPawn : NetworkBehaviour
    {
        [field: SerializeField]
        public CharacterMovementController MovementController { get; private set; }
        [field: SerializeField]
        public CharacterGravityApplier GravityApplier { get; private set; }
        [field: SerializeField]
        public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField]
        public Transform CameraContainer { get; private set; }

        private void Start()
        {
            MovementController.SetDependencies(Rigidbody);
            GravityApplier.SetDependencies(Rigidbody);
        }
    }
}