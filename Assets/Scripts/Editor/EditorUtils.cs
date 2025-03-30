using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EditorUtils
    {
        public static object GetPropertyValue(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer: return property.intValue;
                case SerializedPropertyType.Float: return property.floatValue;
                case SerializedPropertyType.Boolean: return property.boolValue;
                case SerializedPropertyType.String: return property.stringValue;
                case SerializedPropertyType.ObjectReference: return property.objectReferenceValue;
                case SerializedPropertyType.Color: return property.colorValue;
                case SerializedPropertyType.Vector2: return property.vector2Value;
                case SerializedPropertyType.Vector3: return property.vector3Value;
                // Add more cases as needed
                default: return null;
            }
        }

        public static void SetPropertyValue(SerializedProperty property, object value)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer: property.intValue = (int)value; break;
                case SerializedPropertyType.Float: property.floatValue = (float)value; break;
                case SerializedPropertyType.Boolean: property.boolValue = (bool)value; break;
                case SerializedPropertyType.String: property.stringValue = (string)value; break;
                case SerializedPropertyType.ObjectReference: property.objectReferenceValue = (Object)value; break;
                case SerializedPropertyType.Color: property.colorValue = (Color)value; break;
                case SerializedPropertyType.Vector2: property.vector2Value = (Vector2)value; break;
                case SerializedPropertyType.Vector3: property.vector3Value = (Vector3)value; break;
                // Add more cases as needed
            }
        }

        // Generic field drawing method
        public static object EditorField(Rect position, GUIContent label, object value)
        {
            if (value is int) return EditorGUI.IntField(position, label, (int)value);
            if (value is float) return EditorGUI.FloatField(position, label, (float)value);
            if (value is bool) return EditorGUI.Toggle(position, label, (bool)value);
            if (value is string) return EditorGUI.TextField(position, label, (string)value);
            if (value is Object) return EditorGUI.ObjectField(position, label, (Object)value, typeof(Object), true);
            if (value is Color) return EditorGUI.ColorField(position, label, (Color)value);
            if (value is Vector2) return EditorGUI.Vector2Field(position, label, (Vector2)value);
            if (value is Vector3) return EditorGUI.Vector3Field(position, label, (Vector3)value);
            // Add more as needed
            return value;
        }
    }
}