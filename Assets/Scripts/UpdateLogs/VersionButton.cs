using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VersionButton : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]TextMeshProUGUI versionText;
    public string version;
    public int index;
    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateLogsManager.instance.SelectVersion(index);
    }
    public void Settup(){
        versionText.text = version;
    }
}
