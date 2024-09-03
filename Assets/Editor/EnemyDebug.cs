using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBehaviour))]
public class EnemyDebug : Editor
{
    SerializedProperty _value;


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("----- DEBUG -----");
        _value = serializedObject.FindProperty("whichAction");
        EnemyBehaviour eb = (EnemyBehaviour)target;
        serializedObject.Update();
        EditorGUILayout.PropertyField(_value);
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Ativar Acao"))
        {
            eb.DebugAction();
        }
    }

}
