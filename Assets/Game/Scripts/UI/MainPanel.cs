using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainPanel : MonoBehaviour
{
    [SerializeField] private RectTransform textRect;

    private void Awake()
    {
        textRect.DOScale(textRect.localScale * 0.95f, 0.8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
