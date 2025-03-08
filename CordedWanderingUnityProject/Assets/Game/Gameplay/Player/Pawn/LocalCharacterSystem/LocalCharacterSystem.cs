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
            
            ChangeLayerRecursively( PlayerCharacterPawn.gameObject, LayerMask.NameToLayer("LocalCharacter"));
        }

        private void ChangeLayerRecursively(GameObject a_gameObject, int a_layer)
        {
            a_gameObject.layer = a_layer;
            for (int i = 0; i < a_gameObject.transform.childCount; i++)
            {
                var child = a_gameObject.transform.GetChild(i);
                ChangeLayerRecursively(child.gameObject, a_layer);
            }
        }
    }
}