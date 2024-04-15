using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CharacterData))]
public class CharacterDataCustomPropertyDrawer : PropertyDrawer
{
    private bool _isActive;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
        position.x += EditorGUIUtility.labelWidth;
        position.width -= EditorGUIUtility.labelWidth;

        if (property.isExpanded && GUI.Button(new Rect(position.x, position.y + position.height - 20, position.width, 18), "Get All Property"))
        {
            property.FindPropertyRelative("Name").stringValue = (property.serializedObject.targetObject as Character)?.name;

            property.FindPropertyRelative("Rig2D").objectReferenceValue
                = (property.serializedObject.targetObject as Character)?.GetComponent<Rigidbody2D>();

            property.FindPropertyRelative("Coll2D").objectReferenceValue
                = (property.serializedObject.targetObject as Character)?.GetComponent<Collider2D>();

            property.FindPropertyRelative("SpriteRenderer").objectReferenceValue
                = (property.serializedObject.targetObject as Character)?.GetComponent<SpriteRenderer>();

            property.serializedObject.ApplyModifiedProperties();
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + 20 * 5f;
    }
}

