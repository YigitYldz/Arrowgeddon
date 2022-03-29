using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] confettiParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ArrowController"))
        {
            GetComponent<Collider>().enabled = false;
            GameManager.Instance.EndGame(true);
        
            //for (int i = 0; i < confettiParticles.Length; i++)
            //{
            //    confettiParticles[i].Play();
            //}
        }
    }
}
