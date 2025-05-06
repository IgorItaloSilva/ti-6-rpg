using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveSO", menuName = "ScriptableObjects/ObjectiveSO")]
public class ObjectiveSO : ScriptableObject
{
    [field:SerializeField] public string Id {get;private set;}
    [field:SerializeField] public string displaytext {get;private set;}
    [field:SerializeField] public string displayTextProgress {get;private set;}
    
    






    private void OnValidate(){
        #if UNITY_EDITOR
        Id = name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
