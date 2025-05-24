using UnityEngine;

public class PortalDialogueAnswers : DialogAnswer
{
    [SerializeField]string levelToGo;
    [SerializeField]Vector3 startingPosNextLevel;
    public override void Option1()
    {
        GameManager.instance.ChangeLevel(levelToGo,startingPosNextLevel);
    }

    public override void Option2()
    {
        //does not teleport
    }

    public override void Option3()
    {
       //does no exist
    }
}
