using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int hitPoint = 3;

    private bool isAlive = true;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Die()
    {
        isAlive = false;
        animator.SetTrigger("Death");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAlive && other.CompareTag("ArrowController"))
        {
            ArrowController.Instance.ReduceArrow(hitPoint);
            Die();
        }
    }
}
