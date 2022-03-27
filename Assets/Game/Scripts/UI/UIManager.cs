using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainPanelCanvasGroup;

    private void Awake()
    {
        GameManager.instance.onGameStartEvent.AddListener(OnGameStart);
    }

    private void OnGameStart()
    {
        mainPanelCanvasGroup.DOFade(0f, 0.3f).OnComplete(() => mainPanelCanvasGroup.gameObject.SetActive(false));
    }
}
