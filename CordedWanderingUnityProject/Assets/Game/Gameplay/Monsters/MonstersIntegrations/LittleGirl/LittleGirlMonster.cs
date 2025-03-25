using Game.Gameplay.Monsters.ReferencesContainer;
using UnityEngine;

namespace Game.Gameplay.Monsters.MonstersIntegrations.LittleGirl
{
    public class LittleGirlMonster : MonoBehaviour
    {
        [field: SerializeField]
        public MonsterReferencesContainer ReferencesContainer { get; private set; }
    }
}