using Unity.Netcode;
using UnityEngine;

namespace Game.Gameplay.Monsters.Character
{
    public class MonsterCharacter : NetworkBehaviour
    {
        [field: SerializeField]
        public AMonsterAnimationsController AnimationsController { get; private set; }
        
        private readonly NetworkVariable<bool> m_isShown = new();
        public bool IsShown => m_isShown.Value;

        public void Show()
        {
            if (!IsServer)
                return;
            if (IsShown)
                return;
            
            Show_ClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void Show_ClientRpc()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (!IsServer)
                return;
            if (!IsShown)
                return;
            
            Hide_ClientRpc();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void Hide_ClientRpc()
        {
            gameObject.SetActive(false);
        }
    }
}