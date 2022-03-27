using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onGameStartEvent = new UnityEvent();
    public bool IsGameActive { get; private set; }

    #region Singleton
    // This is a property.
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void OnStartButtonClicked()
    {
        onGameStartEvent?.Invoke();
        IsGameActive = true;
    }
}
