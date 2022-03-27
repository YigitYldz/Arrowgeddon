using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float sideSpeed = 20f;
    [SerializeField] private float sideBounds = 3f;
    private SplineFollower splineFollower;

    private void Awake()
    {
        splineFollower = GetComponentInParent<SplineFollower>();
        GameManager.Instance.onGameStartEvent.AddListener(OnGameStart);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameActive)
        {
            HandleMovement();
        }
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
}
