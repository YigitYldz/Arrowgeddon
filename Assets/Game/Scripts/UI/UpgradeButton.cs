using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;

    private Button buttonComponent;
    private int basePrice = 20;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
        buttonComponent.onClick.AddListener(Upgrade);

        GameManager.Instance.endGameEvent.AddListener(success => HandleButtonLockStatus());

        SetPriceText(CalculatePrice());
    }
    private void SetPriceText(int price)
    {
        priceText.text = price.ToString();
    }

    public void Upgrade()
    {
        int price = CalculatePrice();

        if (DataManager.Instance.Gold >= price)
        {
            DataManager.Instance.SetGold(DataManager.Instance.Gold - price);
            DataManager.Instance.SetArrow(DataManager.Instance.Arrow + 1);
        }

        UIManager.Instance.SetGoldText(DataManager.Instance.Gold);
        SetPriceText(CalculatePrice());

        HandleButtonLockStatus();
    }

    private void HandleButtonLockStatus()
    {
        if (DataManager.Instance.Gold < CalculatePrice())
        {
            buttonComponent.interactable = false;
        }
        else
        {
            buttonComponent.interactable = true;
        }
    }

    private int CalculatePrice()
    {
        int arrowLevel = DataManager.Instance.Arrow;
        return basePrice + arrowLevel * 2;
    }
}
