using Cinemachine.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController))]
public class EnemyDebug : Editor
{
    SerializedProperty _meleeDistance, _target;


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
        GUILayout.Label("COMPONENTS: \n  -Rigidbody \n  -Animator \n  -AEnemyBehave \n  - AEnemyAction");

        _target = serializedObject.FindProperty("target");
        GUILayout.Label("\nAlvo que este personagem está lutando:");
        EditorGUILayout.PropertyField(_target);

        _meleeDistance = serializedObject.FindProperty("meleeDistance");
        GUILayout.Label("\nDistância max para ser considerada melee:");
        EditorGUILayout.PropertyField(_meleeDistance);

        serializedObject.ApplyModifiedProperties();

        //DrawDefaultInspector();
        //_value = serializedObject.FindProperty("whichAction");
        //_coolDown = serializedObject.FindProperty("coolDownActions");
        //_actionsReady = serializedObject.FindProperty("actionsReady");

        //EditorGUILayout.PropertyField(_coolDown);
        //EditorGUILayout.PropertyField(_actionsReady);
        //GUILayout.Label("\n----- DEBUG -----");
        //EditorGUILayout.PropertyField(_value);

        //EnemyController eb = (EnemyController)target;
        //if (GUILayout.Button("Ativar Acao"))
        //{
        //    eb.DebugAction();
        //}
    }

}
