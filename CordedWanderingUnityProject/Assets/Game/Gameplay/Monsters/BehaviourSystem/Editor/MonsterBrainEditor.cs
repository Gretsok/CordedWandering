using UnityEditor;
using UnityEngine;

namespace Game.Gameplay.Monsters.BehaviourSystem.Editor
{
    [CustomEditor(typeof(MonsterBrain))]
    public class MonsterBrainEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
                return;
            
            var castedTarget = (target as MonsterBrain)!;
            
            GUILayout.Label($"Current behaviour: {(castedTarget.BehaviourAnchor ? castedTarget.BehaviourAnchor.name : "None")}");
        }
    }
}