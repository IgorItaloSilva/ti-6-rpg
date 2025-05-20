using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Speech", order = 1)]

[System.Serializable]
public class Speech : ScriptableObject
{
    [TextArea] public string text;
    public Sprite speakerIcon; 
    public string speakerName;
    [Header("Coisas caso tenha respostas")]
    public bool needsAnswer;
    public int amountAnswers;
    public string[] optionsTexts;
    public DialogAnswer dialogAnswer;

     void OnValidate(){
        if(amountAnswers!=0||needsAnswer){
            if(amountAnswers==0&&needsAnswer)Debug.LogError($"Mano vc marcou o needAnswer do {name} mas n colocou o numero de respostas");
            if(amountAnswers!=0&&!needsAnswer)Debug.LogError($"Mano vc colocou o numero de respostas do {name} mas não marcou o needAnswer");
            if(amountAnswers!=optionsTexts.Length)Debug.LogError($"Mano vc colocou um numero de respostas do {name} e n colou esse numero de opções");
            //if(dialogAnswer==null)Debug.LogError($"Animal vc esqueceu de colocar a porra da classe do dialogAnswer do {name}");
        }
    } 
}
