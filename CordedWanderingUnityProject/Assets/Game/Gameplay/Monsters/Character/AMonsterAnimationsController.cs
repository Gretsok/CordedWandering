using UnityEngine;

namespace Game.Gameplay.Monsters.Character
{
    public class AMonsterAnimationsController : MonoBehaviour
    {
        [field: SerializeField]
        public Animator Animator { get; private set; }
    }
}