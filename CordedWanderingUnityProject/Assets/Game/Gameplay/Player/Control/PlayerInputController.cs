using System;
using Game.Gameplay.Player.Pawn;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Gameplay.Player.Control
{
    public class PlayerInputController : MonoBehaviour
    {
        private GameplayInputControls m_controls;

        [SerializeField]
        private bool m_activateOnStart = true;

        public bool IsActivated { get; private set; } = false;

        private void Start()
        {
            m_controls = new GameplayInputControls();
            if (m_activateOnStart)
                Activate();
        }

        public void Activate()
        {
            m_controls.Enable();

            m_controls.Mobility.Jump.started += HandleJumpStarted;
            
            IsActivated = true;
        }

        public void Deactivate()
        {
            m_controls.Mobility.Jump.started -= HandleJumpStarted;

            m_controls.Disable();
            
            IsActivated = false;
        }

        private void Update()
        {
            if (!IsActivated)
                return;

            if (!PlayerCharacterPawn)
                return;
            
            PlayerCharacterPawn.MovementController.SetMovementInput(m_controls.Mobility.Move.ReadValue<Vector2>());
            PlayerCharacterPawn.MovementController.SetLookAroundInput(m_controls.Mobility.LookAround.ReadValue<Vector2>());
        }
        
        private void HandleJumpStarted(InputAction.CallbackContext a_obj)
        {
            PlayerCharacterPawn.MovementController.RequestJump();
        }
        
        public PlayerCharacterPawn PlayerCharacterPawn { get; private set; }

        public void SetPlayerCharacterPawn(PlayerCharacterPawn a_playerCharacterPawn)
        {
            PlayerCharacterPawn = a_playerCharacterPawn;
        }
    }
}