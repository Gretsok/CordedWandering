using UnityEngine;

namespace Game.Gameplay.Player.Pawn.Movement
{
    [CreateAssetMenu(fileName = "Movement Data Asset", menuName = "Game/Gameplay/Player/Pawn/Movement Data Asset", order = 0)]
    public class MovementDataAsset : ScriptableObject
    {
        [field: Header("Grounded movement")]
        [field: SerializeField]
        public float Acceleration { get; private set; } = 60f;
        [field: SerializeField]
        public float Deceleration { get; private set; } = 20f;
        [field: SerializeField]
        public float MaxSpeed { get; private set; } = 7f;
        
        [field: Header("Jump movement")]
        [field: SerializeField]
        public float JumpInitialSpeed { get; private set; } = 10f;
        
        [field: Header("Looking Around")]
        [field: SerializeField]
        public Vector2 SightVerticalLimits { get; private set; } = new Vector2(-60f, 60f);
        [field: SerializeField]
        public Vector2 Sensitivities { get; private set; } = new Vector2(30f, 30f);
    }
}