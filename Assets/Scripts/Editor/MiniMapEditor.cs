using Game.Player.Camera;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MiniMapGenerator))]
    public class MiniMapEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Minimap"))
            {
                MiniMapGenerator minimap = target as MiniMapGenerator;

                minimap?.GenerateMiniMap();
            }
        }
    }
}