using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onGameStartEvent = new UnityEvent();

    // Singleton Pattern *
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Singleton Pattern Ends !

    public void OnStartButtonClicked()
    {
        onGameStartEvent?.Invoke();
    }
}
