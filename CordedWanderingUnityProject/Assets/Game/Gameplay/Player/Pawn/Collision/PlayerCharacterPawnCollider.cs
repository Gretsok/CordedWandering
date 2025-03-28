using UnityEngine;

namespace Game.Gameplay.Player.Pawn.Collision
{
    public class PlayerCharacterPawnCollider : MonoBehaviour
    {
        [field: SerializeField]
        public PlayerCharacterPawn PlayerCharacterPawn { get; private set; }
    }
}