using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBehaviour))]
public class EnemyDebug : Editor
{
    SerializedProperty _value, _coolDown, _actionsReady;


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
        GUILayout.Label("COMPONENTS: \n -Rigidbody \n -Animator");

        //DrawDefaultInspector();
        _value = serializedObject.FindProperty("whichAction");
        _coolDown = serializedObject.FindProperty("coolDownActions");
        _actionsReady = serializedObject.FindProperty("actionsReady");

        EditorGUILayout.PropertyField(_coolDown);
        EditorGUILayout.PropertyField(_actionsReady);

        GUILayout.Label("\n----- DEBUG -----");
        EditorGUILayout.PropertyField(_value);

        serializedObject.ApplyModifiedProperties();
        EnemyBehaviour eb = (EnemyBehaviour)target;
        if (GUILayout.Button("Ativar Acao"))
        {
            eb.DebugAction();
        }
    }

}
