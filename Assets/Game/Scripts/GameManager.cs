using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent onGameStartEvent = new UnityEvent();
    [HideInInspector] public UnityEvent<bool> endGameEvent = new UnityEvent<bool>();

    public bool IsGameActive { get; private set; }

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

    public void OnStartButtonClicked()
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
}
