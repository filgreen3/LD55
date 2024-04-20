using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;


[CustomEditor(typeof(Entity), true)]
public class EntityCustomInspector : Editor
{
    private Entity TargetEntity
    {
        get
        {
            if (_targetEntity == null)
                _targetEntity = target as Entity;
            return _targetEntity;
        }
        set => _targetEntity = value;
    }

    private Vector2 _scroll;
    protected Assembly[] _assembles = new Assembly[0];
    protected int _assemblyID;
    protected int _typeID;
    private Entity _targetEntity;
    private ReorderableList _reordableList;
    private int _selectedIndex;

    public override void OnInspectorGUI()
    {
        _reordableList.DoLayoutList();
        GUILayout.Space(10);


        var properties = serializedObject.GetIterator();
        properties.NextVisible(true);
        while (properties.NextVisible(false))
        {
            if (properties.name != "_componentsList")
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(properties, true);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }

    private void OnEnable()
    {
        _assemblyID = 0;
        _assembles = System.AppDomain.CurrentDomain.GetAllAssembles((type) => ComponentFinder.isSystemAcceptable(type, TargetEntity));
        TargetEntity = (Entity)target;
        _reordableList = new ReorderableList(serializedObject, serializedObject.FindProperty("_componentsList"), true, false, false, false);
        OnListReordered(_reordableList);
        _reordableList.onReorderCallback = OnListReordered;
        _reordableList.drawHeaderCallback = DrawHeader;
        _reordableList.drawElementCallback = DrawElement;
        _reordableList.elementHeightCallback = GetElementHeight;
        _reordableList.drawFooterCallback = DrawFooter;
        _reordableList.drawElementBackgroundCallback = DrawBackground;
        _reordableList.onChangedCallback = (t) => ReorderableListRepaint();
        _reordableList.onSelectCallback = (t) => ReorderableListRepaint();

    }

    private void DrawFooter(Rect rect)
    {
        rect.y += 2;
        var buttonRect = rect;
        buttonRect.width = 30;
        buttonRect.height -= 2f;
        if (_assembles.Length > 0 && GUI.Button(buttonRect, "+"))
        {
            AddSystem();
        }

        rect.x += 30;

        if (_assembles.Length > 0)
        {
            rect.width /= 2;
            rect.width -= 15;
            EditorGUI.BeginChangeCheck();
            _typeID = EditorGUI.Popup(rect, _typeID, ComponentFinder.GetSystems(_assembles[_assemblyID], _targetEntity));
            rect.x += rect.width;
            rect.width -= 30;
            _assemblyID = EditorGUI.Popup(rect, _assemblyID, ComponentFinder.GetAssembles(_assembles));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        rect.x += rect.width;
        rect.width = 30;
        rect.height -= 2f;
        if (_assembles.Length > 0 && GUI.Button(rect, "-"))
        {
            foreach (var index in _reordableList.selectedIndices)
            {
                _reordableList.serializedProperty.DeleteArrayElementAtIndex(index);
            }
            _reordableList.ClearSelection();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(TargetEntity);
        }
    }

    private void DrawBackground(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (isActive)
        {
            EditorGUI.DrawRect(rect, Color.Lerp(Color.blue, Color.gray, 0.8f));
            return;
        }

        if (index % 2 == 0)
        {
            EditorGUI.DrawRect(rect, Color.Lerp(Color.gray, Color.clear, 0.6f));
        }
        else
        {
            EditorGUI.DrawRect(rect, Color.Lerp(Color.gray, Color.clear, 0.45f));
        }
    }

    private void ReorderableListRepaint()
    {
        Repaint();
    }

    private void OnListReordered(ReorderableList list)
    {
        ReorderableListRepaint();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(TargetEntity);
    }

    private float GetElementHeight(int index)
    {
        var component = serializedObject.FindProperty("_componentsList").GetArrayElementAtIndex(index);
        return component.isExpanded ? EditorGUI.GetPropertyHeight(component, true) : EditorGUIUtility.singleLineHeight; ;
    }

    private void DrawHeader(Rect headerRect)
    {

    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        rect.x += 15;
        rect.width -= 15;
        rect.y += 2;

        var component = serializedObject.FindProperty("_componentsList").GetArrayElementAtIndex(index);
        EditorGUI.BeginChangeCheck();
        if (component == null)
        {
            Debug.LogWarning("Component is null");
            GUI.Box(rect, "null");
            EditorGUI.EndChangeCheck();
            return;
        }

        EditorGUI.PropertyField(rect, component, new GUIContent(component.managedReferenceValue.GetType().Name), true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(TargetEntity);
        }
    }

    private void BottomMenu()
    {
        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();
    }

    protected void AddSystem()
    {
        var assembly = _assembles[_assemblyID];
        if (ComponentFinder.GetSystems(_assembles[_assemblyID], _targetEntity).Length <= 0)
        {
            Debug.LogWarning("Type is null");
            return;
        }
        var type = ComponentFinder.GetTypeFromAssembley(ComponentFinder.GetSystems(_assembles[_assemblyID], _targetEntity)[_typeID], assembly);
        if (type == null)
        {
            Debug.LogWarning("Type is null");
            return;
        }
        _assembles = System.AppDomain.CurrentDomain.GetAllAssembles((type) => ComponentFinder.isSystemAcceptable(type, TargetEntity));
        _typeID = 0;
        _assemblyID = 0;
        TargetEntity.AddEntityComponent(type);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(TargetEntity);
    }


}
