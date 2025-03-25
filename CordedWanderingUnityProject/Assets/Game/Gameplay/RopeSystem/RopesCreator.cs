using System.Collections.Generic;
using Game.Gameplay.Player.Spawn;
using UnityEngine;

namespace Game.Gameplay.RopeSystem
{
    public class RopesCreator : MonoBehaviour
    {
        [SerializeField]
        private Rope m_ropePrefab;
        
        private CharacterSpawner m_characterSpawner;

        private List<Rope> m_instantiatedRopes = new List<Rope>();
        
        private void Awake()
        {
            m_characterSpawner = GetComponent<CharacterSpawner>();
            m_characterSpawner.OnCharacterCreated_Clients += HandleCharacterCreated_Clients;
        }

        private void HandleCharacterCreated_Clients(CharacterSpawner a_arg1, ulong a_arg2)
        {
            for (int i = m_instantiatedRopes.Count - 1; i >= 0; i--)
            {
                var rope = m_instantiatedRopes[i];
                Destroy(rope.gameObject);
            }
            m_instantiatedRopes.Clear();

            for (int i = 1; i < m_characterSpawner.PlayerToCharacterAssociations.Count; i++)
            {
                var character = m_characterSpawner.PlayerToCharacterAssociations[i].Character;
                var previousCharacter = m_characterSpawner.PlayerToCharacterAssociations[i - 1].Character;
                
                var rope = Instantiate(m_ropePrefab, previousCharacter.transform.position, previousCharacter.transform.rotation);
                rope.FirstLink.transform.parent = previousCharacter.transform;
                rope.LastLink.transform.parent = character.transform;
                rope.ComputeRope();
            }

        }
    }
}