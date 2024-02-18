using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent onGameStartEvent = new UnityEvent();
    [HideInInspector] public UnityEvent<bool> endGameEvent = new UnityEvent<bool>();
    public bool IsGameActive { get; private set; }
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject camera;
    #region Singleton
    // This is a property.
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DG.Tweening.DOTween.SetTweensCapacity(500, 50);
    }
    #endregion
    public void ShowStart()
    {
        StartCoroutine(DelayBeforeStart());
        animator.SetBool("start", true);
        StartCoroutine(WaitForShootArrow());
    }
    public void StartGame()
    {
        onGameStartEvent?.Invoke();
        IsGameActive = true;
    }

    public void EndGame(bool success)
    {
        IsGameActive = false;
        endGameEvent?.Invoke(success);

        Debug.Log(success ? "Success" : "Fail");
    }
    private IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSeconds(2);
        cinemachineBrain.enabled = true;
        StartGame();
    }
    private IEnumerator WaitForShootArrow()
    {
        yield return new WaitForSeconds(0.9f);
        arrow.transform.DOMove(arrow.transform.forward * 1.55f, 0.8f).OnComplete(DestroyArrow);
        StartCoroutine(WaitForRotateCamera());
    }
    private IEnumerator WaitForRotateCamera()
    {
        yield return new WaitForSeconds(0.6f);
        camera.transform.DORotate(new Vector3(10, 0, 0), 0.5f);
    }
    private void DestroyArrow()
    {
        
        
        arrow.SetActive(false);
    }
}
