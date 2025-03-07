using Unity.Cinemachine;
using UnityEngine;

namespace Game.Gameplay.Player.Pawn.LocalCharacterSystem
{
    public class LocalCharacterSystem : MonoBehaviour
    {
        [field: SerializeField]
        public CinemachineCamera Camera { get; private set; }

        public PlayerCharacterPawn PlayerCharacterPawn { get; private set; }

        public void SetPlayerCharacterPawn(PlayerCharacterPawn a_playerCharacterPawn)
        {
            PlayerCharacterPawn = a_playerCharacterPawn;
            
            Camera.transform.SetParent(PlayerCharacterPawn.CameraContainer);
            Camera.transform.localPosition = Vector3.zero;
            Camera.transform.localRotation = Quaternion.identity;
        }
    }
}