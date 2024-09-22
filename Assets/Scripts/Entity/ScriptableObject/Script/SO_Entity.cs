using Unity.VisualScripting;
using UnityEngine;


public enum EntityType
{
    player,
    companion,
    enemy
}


[CreateAssetMenu(fileName = "Entity", menuName = "SO/New Entity")]
public class SO_Entity : ScriptableObject
{
    //[InspectorLabel("Base Entidade")]
    [field: SerializeField]
    [SerializeField] public string entityName { get; private set; }

    public EntityType type;

    //[InspectorLabel("Parametros da entidade")]
    [field: SerializeField]
    public int cons { get; private set; }
    [field: SerializeField]
    public int stre { get; private set; }
    [field: SerializeField]
    public int inte { get; private set; }
    [field: SerializeField]
    public int dext { get; private set; }
    
    // ----- Getters ----- \\

}