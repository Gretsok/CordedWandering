using Game.Gameplay.Monsters.BehaviourSystem;
using Game.Gameplay.Player.Pawn;
using UnityEditor;
using UnityEngine;

namespace Game.Gameplay.Monsters.Sensors.PlayerDetection.Editor
{
    [CustomEditor(typeof(MonsterPlayerInAllDirectionsDetector))]
    public class MonsterPlayerInAllDirectionsDetectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
                return;
            
            var castedTarget = target as MonsterPlayerInAllDirectionsDetector;
            
            GUILayout.Label("Detected players:");
            for (int i = 0; i < castedTarget.PlayersInSight.Count; i++)
            {
                if (castedTarget.PlayersInSight[i].TryGet(out PlayerCharacterPawn pawn))
                {
                    GUILayout.Label($"- {pawn.gameObject.name}");
                }
                else
                {
                    GUILayout.Label($"- None");
                }
            }
            
        }
    }
}
