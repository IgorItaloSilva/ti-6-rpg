using UnityEngine;

public class KitsuneDeathAux : DeathAux
{
    [SerializeField] int pillarID;
    public override void OnDeath()
    {
        Debug.Log($"O kitsune Dath Aux {pillarID} está enviando um evendo com o ID dele");
        GameEventsManager.instance.levelEvents.KitsuneDeath(pillarID);
    }

}
