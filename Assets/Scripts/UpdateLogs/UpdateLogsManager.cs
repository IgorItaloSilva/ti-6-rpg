using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLogsManager : MonoBehaviour
{
    public static UpdateLogsManager instance;
    [Header("Coisas que precisam ser colocadas da UI")]
    [SerializeField]TextMeshProUGUI versionTMP;
    [SerializeField]TextMeshProUGUI contributionAliceTMP;
    [SerializeField]TextMeshProUGUI contributionArthurTMP;
    [SerializeField]TextMeshProUGUI contributionFelipeTMP;
    [SerializeField]TextMeshProUGUI contributionGabrielTMP;
    [SerializeField]TextMeshProUGUI contributionGustavoTMP;
    [SerializeField]TextMeshProUGUI contributionHenriqueTMP;
    [SerializeField]TextMeshProUGUI contributionIgorTMP;
    [SerializeField]TextMeshProUGUI contributionPedroTMP;
    [SerializeField]TextMeshProUGUI contributionTiagoTMP;
    [SerializeField]GameObject painelUpdates;
    [SerializeField]GameObject horizontalLayoutGroup;
    [SerializeField]GameObject prefabVersionButton;
    [field:SerializeField]List<UpdateInfoSO>updates;
    bool isPainelUpdatesOpen = false;

    int indexMostRecent;
    void Awake(){
        if(instance==null)instance=this;
        else Destroy(gameObject);
    }
    void Start()
    {
        painelUpdates.SetActive(false);
        isPainelUpdatesOpen = false;
        if(updates.Count==0)return;
        DateTime mostRecent = new DateTime(updates[0].Ano,updates[0].Mes,updates[0].Dia);
        indexMostRecent=0;
        if(updates.Count!=1){
            for(int i=1;i<updates.Count;i++){
                DateTime dateTime = new DateTime(updates[i].Ano,updates[i].Mes,updates[i].Dia);
                if(DateTime.Compare(mostRecent,dateTime)<0){
                    mostRecent=dateTime;
                    indexMostRecent=i;
                }
            }
        }
        for(int i=0;i<updates.Count;i++){
            GameObject newButton = Instantiate(prefabVersionButton,horizontalLayoutGroup.transform);
            VersionButton versionButton = newButton.GetComponent<VersionButton>();
            versionButton.version = updates[i].Version;
            versionButton.index=i;
            versionButton.Settup();
        }
        versionTMP.text = updates[indexMostRecent].Version;
        SelectVersion(indexMostRecent);
    }
    public void OpenCloseLogs(){//publico pois vai ser chaamdo por um botão
        if(isPainelUpdatesOpen){
            painelUpdates.SetActive(false);
            isPainelUpdatesOpen=false;
        }
        else{
            painelUpdates.SetActive(true);
            isPainelUpdatesOpen=true;
        }
    }
    public void SelectVersion(int index){//sera chamado pelo botao da versão
        UpdateInfoSO su = updates[index];
        contributionAliceTMP.text="Alice Andrade Santos:"+su.ContributionsAlice;
        contributionArthurTMP.text="Arthur Araujo Moura: "+su.ContributionsArthur;
        contributionFelipeTMP.text="Felipe Campos Alves Pereira: "+su.ContributionsFelipe;
        contributionGabrielTMP.text="Gabriel Nogueira Lourenço: "+su.ContributionsGabriel;
        contributionGustavoTMP.text="Gustavo Cândido da Silva e Souza: "+su.ContributionsGustavo;
        contributionHenriqueTMP.text="Henrique Durães Monteiro: "+su.ContributionsHenrique;
        contributionIgorTMP.text="Igor Italo Silva "+su.ContributionsIgor;
        contributionPedroTMP.text="Pedro Henrique Galvão Paschoal: "+su.ContributionsPedro;
        contributionTiagoTMP.text="Tiago Augusto Simionato Tozo: "+su.ContributionsTiago;
    }
}
