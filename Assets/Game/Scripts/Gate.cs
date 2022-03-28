using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum GateType { Multiplier = 0, Additive = 1, Reducer = 2, Divider = 3 }

public class Gate : MonoBehaviour
{
    [SerializeField] private GateType type;
    [SerializeField] private int value;
    [SerializeField] private TMP_Text text;
    [SerializeField] private MeshRenderer gateRend;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;

    private DoubleGate doubleGate;

    private void Awake()
    {
        doubleGate = GetComponentInParent<DoubleGate>();

        UpdateColor();
        UpdateText();
    }

    private void UpdateColor()
    {
        gateRend.material = (type == GateType.Reducer || type == GateType.Divider) ? redMaterial : greenMaterial;
    }

    private void UpdateText()
    {
        if (type == GateType.Additive)
            text.text = $"+{value}";
        else if (type == GateType.Multiplier)
            text.text = $"x{value}";
        else if (type == GateType.Reducer)
            text.text = $"-{value}";
        else
            text.text = $"÷{value}";
    }

    private void Blink()
    {
        float a = gateRend.material.color.a;

        gateRend.DOKill(complete: true);

        gateRend.material.DOFade(0.8f, 0.01f).OnComplete(() => {
            gateRend.material.DOFade(a, 0.5f).SetEase(Ease.OutQuart);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doubleGate != null && !doubleGate.IsUsed && other.CompareTag("ArrowController"))
        {
            GetComponent<Collider>().enabled = false;

            Blink();

            doubleGate.SetIsUsed();

            switch (type)
            {
                case GateType.Additive:
                    ArrowController.Instance.SpawnArrow(value);
                    break;
                case GateType.Multiplier:
                    ArrowController.Instance.MultiplyArrows(value);
                    break;
                case GateType.Reducer:
                    ArrowController.Instance.ReduceArrow(value);
                    break;
                case GateType.Divider:
                    ArrowController.Instance.DivideArrows(value);
                    break;
            }
        }
    }

    private void OnValidate()
    {
        UpdateColor();
        UpdateText();
    }
}
