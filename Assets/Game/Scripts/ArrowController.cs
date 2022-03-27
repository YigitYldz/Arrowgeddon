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

    private ObjectPool<GameObject> arrowPool;
    private SplineFollower splineFollower;
    private GameObject arrowPrefab;
    private List<GameObject> activeArrowsList = new List<GameObject>();

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

        splineFollower = GetComponentInParent<SplineFollower>();

        GameManager.Instance.onGameStartEvent.AddListener(OnGameStart);

        CreateArrowPool();

        SpawnArrow(1);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            HandleMovement();
            HandleScale();
        }
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
        if (activeArrowsList.Count == 0)
        {
            return;
        }

        SphereCollider col = GetComponent<SphereCollider>();

        activeArrowsList[0].transform.localPosition = Vector3.zero;
        int arrowIndex = 1;
        int circleOrder = 1;

        while (true)
        {
            col.radius = 0.11f * circleOrder;
            float radius = circleOrder * distanceBetweenArrows;

            for (int i = 0; i < (circleOrder + 1) * 4; i++)
            {
                if (arrowIndex == activeArrowsList.Count)
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
}
