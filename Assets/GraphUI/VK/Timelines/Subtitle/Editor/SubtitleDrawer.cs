using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SubtitleBehaviour))]
public class SubtitleDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty textProp = property.FindPropertyRelative("Text");
        EditorGUI.BeginProperty(position, new GUIContent("Text"), textProp);
        textProp.stringValue = EditorGUI.TextArea(position, textProp.stringValue);
        EditorGUI.EndProperty();
    }
}
