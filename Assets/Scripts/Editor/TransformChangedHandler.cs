using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    [InitializeOnLoad]
    public static class TransformChangedHandler
    {
        static List<ITransformChanged> transformChangeListeners = new();
        
        static TransformChangedHandler()
        {
            EditorApplication.update += CheckTransformChanged;
            EditorApplication.hierarchyChanged += CollectTransformChangeListeners;
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        static void CheckTransformChanged()
        {
            foreach (var transformChangedListener in transformChangeListeners)
            {
                if (transformChangedListener.HasTransformChanged())
                {
                    transformChangedListener.OnTransformChanged();
                }
            }
        }

        static void OnPlaymodeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                {
                    CollectTransformChangeListeners();
                    EditorApplication.update += CheckTransformChanged;
                    Debug.Log("started tracking transform changes");
                    break;
                }
                case PlayModeStateChange.ExitingEditMode:
                {
                    EditorApplication.update -= CheckTransformChanged;
                    Debug.Log("stopped tracking transform changes");
                    break;
                }
            }
        }
        static void CollectTransformChangeListeners()
        {
            transformChangeListeners = Object
                .FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .OfType<ITransformChanged>().ToList();
        }
    }
}