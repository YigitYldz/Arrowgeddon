using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
{
    [SerializeField] private float maxDistance = 100f;

    public PointerEventData eventData;
    public Vector2 Input { get; private set; }

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

    private void Update()
    {
        if (eventData == null)
        {
            return;
        }

        delta = eventData.position - startPos;
        delta.x = Mathf.Clamp(delta.x, -maxDistance, maxDistance);
        delta.y = Mathf.Clamp(delta.y, -maxDistance, maxDistance);
        Input = delta / maxDistance;
        startPos = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.eventData = eventData;
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        eventData = null;
        Input = Vector2.zero;
        startPos = Vector2.zero;
        delta = Vector2.zero;
    }
}
