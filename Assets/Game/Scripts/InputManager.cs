using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
{
    [SerializeField] private float swipeTreshold = 100f;

    public PointerEventData eventData;

    [HideInInspector] public UnityEvent onSwipeRight = new UnityEvent();
    [HideInInspector] public UnityEvent onSwipeLeft = new UnityEvent();

    private Vector2 startPos;
    private Vector2 delta;
    
    // Singleton Pattern *
    public static InputManager instance;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        this.eventData = eventData;
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        delta = eventData.position - startPos;

        if (delta.x >= swipeTreshold)
        {
            onSwipeRight?.Invoke();
        }
        if (delta.x <= -swipeTreshold)
        {
            onSwipeLeft?.Invoke();
        }

        this.eventData = null;

        startPos = Vector2.zero;
        delta = Vector2.zero;
    }
}
