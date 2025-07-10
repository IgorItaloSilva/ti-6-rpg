using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveSO", menuName = "ScriptableObjects/ObjectiveSO")]
public class ObjectiveSO : ScriptableObject
{
    [field:SerializeField] public string Id {get;private set;}
    [field:SerializeField] public int ExpGain {get;private set;}
    [field:SerializeField] public string objectiveTitle {get;private set;}
    [field:SerializeField] public string objectiveTextProgress {get;private set;}
    [field:SerializeField] public bool RequiresRecompletion {get;private set;}
    [field:SerializeField] public GameObject ObjectivePrefab {get;private set;}
    
    






    private void OnValidate(){
        #if UNITY_EDITOR
        Id = name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
