using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 10f;
    private SplineFollower splineFollower;


    private void Awake()
    {
        splineFollower = GetComponentInParent<SplineFollower>();
        GameManager.instance.onGameStartEvent.AddListener(OnGameStart);
    }

    private void OnGameStart()
    {
        splineFollower.followSpeed = forwardSpeed;
    }
          
}
