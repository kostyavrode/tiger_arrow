using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainPanelCanvasGroup;
    [SerializeField] private CanvasGroup endPanelCanvasGroup;
    [SerializeField] private CanvasGroup commonPanelCanvasGroup;
    [SerializeField] private EndPanel endPanel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] public GameObject[] elements;
    [SerializeField] private GameObject blackWindow;
    [SerializeField] private AudioSource source;
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GameManager.Instance.onGameStartEvent.AddListener(OnGameStart);
        GameManager.Instance.endGameEvent.AddListener(HandleEndGameEvent);
        
        Active(mainPanelCanvasGroup, true);
        Active(endPanelCanvasGroup, false);
        Active(commonPanelCanvasGroup, true);

        // Set gold text before game starts
        SetGoldText(DataManager.Instance.Gold);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CloseUI();
        }
    }

    public void SetGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }

    private void OnGameStart()
    {
        ActiveSmooth(mainPanelCanvasGroup, false);
    }

    private void HandleEndGameEvent(bool success)
    {
        endPanel.winScreen.SetActive(success);
        endPanel.failScreen.SetActive(!success);

        ActiveSmooth(endPanelCanvasGroup, true);
    }

    private void Active(CanvasGroup cg, bool isActive)
    {
        cg.alpha = isActive ? 1f : 0f;
        cg.gameObject.SetActive(isActive);
    }

    private void ActiveSmooth(CanvasGroup cg, bool isActive)
    {
        if (isActive)
        {
            cg.gameObject.SetActive(true);
            cg.DOFade(1f, 0.3f);
        }
        else
        {
            cg.DOFade(0f, 0.3f).OnComplete(() => cg.gameObject.SetActive(false));
        }
    }
    public void CloseUI()
    {
        //source.Pause();
        foreach (GameObject obj in elements)
        {
            obj.SetActive(false);
        }
        blackWindow.SetActive(true);

    }
    public void OpenPrivacy(string ttex)
    {

    }
}
