using System.Reflection;
using PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(OnValueChanged))]
    public class OnValueChangedPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            var onValueChangedAttribute = attribute as OnValueChanged;
            
            // Draw the property field dynamically
            object value = EditorUtils.GetPropertyValue(property);
            object newValue = EditorUtils.EditorField(position, label, value);
            
            if (EditorGUI.EndChangeCheck() && !newValue.Equals(value))
            {
                EditorUtils.SetPropertyValue(property, newValue);
                property.serializedObject.ApplyModifiedProperties();

                // Call the callback function
                var targetObject = property.serializedObject.targetObject;
                if (onValueChangedAttribute != null)
                {
                    var method = targetObject.GetType().GetMethod(
                        onValueChangedAttribute.CallbackFunctionName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (method != null)
                    {
                        method.Invoke(targetObject, null);
                    }
                }
            }

            EditorGUI.EndProperty();
        }
    }
}

