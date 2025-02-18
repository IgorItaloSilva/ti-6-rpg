public interface IDamagable 
{
    public void TakeDamage(float damage,Enums.DamageType damageType,bool wasCrit);
    public void Die();
}
