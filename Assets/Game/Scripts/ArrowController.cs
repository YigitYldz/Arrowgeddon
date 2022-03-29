using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float sideSpeed = 20f;
    [SerializeField] private float sideBounds = 3f;
    [SerializeField] private float distanceBetweenArrows = 0.15f;
    [SerializeField] private float scaleDownRatio = 0.4f;
    [SerializeField] private float startDistance = 10f;

    private ObjectPool<GameObject> arrowPool;
    private SplineFollower splineFollower;
    private GameObject arrowPrefab;
    private List<GameObject> activeArrowsList = new List<GameObject>();

    public int ArrowCount => activeArrowsList.Count;

    public static ArrowController Instance { get; private set; }

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

        GameManager.Instance.onGameStartEvent.AddListener(OnGameStart);
        GameManager.Instance.endGameEvent.AddListener(HandleEndGameEvent);

        splineFollower = GetComponentInParent<SplineFollower>();

        SetSplineStartDistance();
        CreateArrowPool();

        SpawnArrow(DataManager.Instance.Arrow);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) &&GameManager.Instance.IsGameActive)
        {
            HandleMovement();
            HandleScale();
        }
    }

    private void HandleEndGameEvent(bool success)
    {
        splineFollower.follow = false;
    }
        
    private void SetSplineStartDistance()
    {
        if (splineFollower == null)
        {
            return;
        }

        splineFollower.startPosition = splineFollower.spline.Travel(0, startDistance);
    }

    private void CreateArrowPool()
    {

        arrowPrefab = Resources.Load<GameObject>("Arrow");
        arrowPool = new ObjectPool<GameObject>(
            500,
            CreateFunction: () =>
            {
                GameObject _arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, transform);
                _arrow.SetActive(false);
                return _arrow;
            },
            OnPush: _arrow =>
            {
                _arrow.SetActive(false);
            },
            OnPop: _arrow =>
            {
                _arrow.transform.localPosition = Vector3.zero;
            }
        );
    }

    private void OnGameStart()
    {
        splineFollower.followSpeed = forwardSpeed;
    }

    private void HandleMovement()
    {
        Vector3 pos = transform.localPosition;

        pos += transform.right * InputManager.instance.Input.x * sideSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -sideBounds, sideBounds);

        transform.localPosition = pos;
    }

    private void HandleScale()
    {
        float x = Mathf.Abs(transform.localPosition.x);
        float scaleFactor = sideBounds == 0 ? 0f : 1 - ((sideBounds - x) / sideBounds);

        Vector3 newScale = transform.localScale;
        newScale.x = 1f - scaleDownRatio * scaleFactor;
        transform.localScale = newScale;
    }

    public void DivideArrows(int divider)
    {
        if (divider < 1)
        {
            return;
        }

        float reduceAmount = ArrowCount * (divider - 1) / (float)divider;
        float remaining = ArrowCount - reduceAmount;

        if (remaining < 1)
        {
            GameManager.Instance.EndGame(false);
            return;
        }

        ReduceArrow(Mathf.CeilToInt(reduceAmount));
    }

    public void MultiplyArrows(int times)
    {
        if (times < 2)
        {
            return;
        }

        SpawnArrow(ArrowCount * (times - 1));
    }

    public void ReduceArrow(int amount)
    {
        if (amount >= ArrowCount)
        {
            GameManager.Instance.EndGame(false);
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject _arrow = activeArrowsList[0];
            activeArrowsList.Remove(_arrow);
            arrowPool.Push(_arrow);
        }

        ReorderArrows();
    }

    public void SpawnArrow(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject _arrow = arrowPool.Pop();
            _arrow.SetActive(true);
            activeArrowsList.Add(_arrow);
        }

        ReorderArrows();
    }

    private void ReorderArrows()
    {
        if (ArrowCount == 0)
        {
            return;
        }

        CapsuleCollider col = GetComponent<CapsuleCollider>();

        activeArrowsList[0].transform.localPosition = Vector3.zero;
        int arrowIndex = 1;
        int circleOrder = 1;

        while (true)
        {
            col.radius = 0.14f * circleOrder;
            float radius = circleOrder * distanceBetweenArrows;

            for (int i = 0; i < (circleOrder + 1) * 4; i++)
            {
                if (arrowIndex == ArrowCount)
                {
                    return;
                }

                float radians = 2 * Mathf.PI / (circleOrder + 1) / 4 * i;
                float vertical = Mathf.Sin(radians);
                float horizontal = Mathf.Cos(radians);

                Vector3 dir = new Vector3(horizontal, vertical, 0f);
                Vector3 newPosition = dir * radius;

                GameObject _arrow = activeArrowsList[arrowIndex];

                if (_arrow != null)
                {
                    _arrow.transform.DOKill();
                    _arrow.transform.DOLocalMove(newPosition, 0.25f);
                }

                arrowIndex++;
            }

            circleOrder++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gold"))
        {
            DataManager.Instance.SetGold(DataManager.Instance.Gold + 1);
            UIManager.Instance.SetGoldText(DataManager.Instance.Gold);

            other.transform.DOScale(0f, 0.15f).OnComplete(() => other.gameObject.SetActive(false));
        }
    }
}
