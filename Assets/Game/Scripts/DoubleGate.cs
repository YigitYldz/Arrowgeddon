using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleGate : MonoBehaviour
{
    [SerializeField] private Gate leftGate;
    [SerializeField] private Gate rightGate;

    public bool IsUsed { get; private set; }

    public void SetIsUsed()
    {
        IsUsed = true;
    }
}
