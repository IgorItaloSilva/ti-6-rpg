using System.Collections;

public interface IEnemyAction
{
    public void StartAction();
    public void UpdateAction();
    public void ExitAction();

    IEnumerator WaitTime(float value, string method);
}
