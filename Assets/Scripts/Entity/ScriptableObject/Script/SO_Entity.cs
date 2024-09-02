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
    [Header("Base Entidade")]
    [SerializeField] string name;
    [SerializeField] EntityType type;

    [Header("Parametros da entidade")]
    [SerializeField] int maxHp;
    [SerializeField] int def;
    [SerializeField] int maxSpeed;
    
    // ----- Getters ----- \\
    public string Getname() { return name; }
    public EntityType GetMaterial() { return type; }
    public int GetmaxHp() {  return maxHp; }
    public int Getdef() { return def; }
    public int GetmaxSpeed() {  return maxSpeed; }

}