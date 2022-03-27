using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainPanelCanvasGroup;
    [SerializeField] private CanvasGroup endPanelCanvasGroup;

    private void Awake()
    {
        GameManager.Instance.onGameStartEvent.AddListener(OnGameStart);
        GameManager.Instance.endGameEvent.AddListener(HandleEndGameEvent);
        
        Active(mainPanelCanvasGroup, true);
        Active(endPanelCanvasGroup, false);
    }

    private void OnGameStart()
    {
        ActiveSmooth(mainPanelCanvasGroup, false);
    }

    private void HandleEndGameEvent(bool success)
    {
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
}
