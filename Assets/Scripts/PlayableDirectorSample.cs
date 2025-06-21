using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class PlayableDirectorSample : MonoBehaviour
{
    public PlayableDirector m_Director;
    [SerializeField] GameObject playerCam;
    [SerializeField] GameObject targetCam;
    [SerializeField] GameObject atmospherics;
    [SerializeField] GameObject boss;
    void OnDisable()
    {
        m_Director.stopped -= Stoped;
    }
    void Awake()
    {
        m_Director = gameObject.GetComponent<PlayableDirector>();
        m_Director.stopped += Stoped;
    }
    void Stoped(PlayableDirector playableDirector)
    {
        if (playableDirector == m_Director)
        {
            playerCam.gameObject.SetActive(true);
            targetCam.gameObject.SetActive(true);
            atmospherics.SetActive(true);
            UIManager.instance?.SetCanvasActive(true);
            PlayerStateMachine.Instance.UnlockPlayer();
            GameEventsManager.instance.playerEvents.SetPosition(new Vector3(-21.7099991f, 14.6400003f, -124.099998f));
            boss.SetActive(true);
            gameObject.SetActive(false);
            
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayCutscene();
        }
    }
    public void PlayCutscene()
    {
        PlayerStateMachine.Instance.LockPlayer();
        playerCam.SetActive(false);
        targetCam.SetActive(false);
        atmospherics.SetActive(false);
        m_Director.Play();
        UIManager.instance?.SetCanvasActive(false);
    }
}
