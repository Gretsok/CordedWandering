using UnityEngine;

namespace Game.Gameplay.Monsters.ReferencesContainer
{
    public class MonsterReferencesContainer : MonoBehaviour
    {
        [field: SerializeField]
        public Rigidbody Rigidbody { get; private set; }
    }
}