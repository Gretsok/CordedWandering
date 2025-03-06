using Unity.Netcode.Components;
using UnityEngine;

namespace Game.Gameplay.Player.Pawn
{
    [DisallowMultipleComponent]
    public class PlayerCharacterPawnNetworkTransform : NetworkTransform
    {
        [SerializeField]
        private bool m_isServerAuthoritative;
        
        protected override bool OnIsServerAuthoritative()
        {
            return m_isServerAuthoritative;
        }
    }
}