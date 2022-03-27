using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ArrowController"))
        {
            GetComponent<Collider>().enabled = false;
            GameManager.Instance.EndGame(true);
        }
    }
}
