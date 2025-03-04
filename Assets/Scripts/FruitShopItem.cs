using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitShopItem : MonoBehaviour
{
    [SerializeField] Button button1;
    [SerializeField] int num1;
    [SerializeField] float money1;
    [SerializeField] ItemType type1;
    [SerializeField] BuyType buyType1;

    private void Awake()
    {
        FruitShop fruitShop = FindObjectOfType<FruitShop>();
        button1.onClick.AddListener(() =>
        {
            fruitShop.BuyItem(type1, buyType1, num1, 0 , money1);
        });

    }

}
[Serializable]
public enum ItemType
{
    Boom,
    Sword,
    Pack
}
[Serializable]
public enum BuyType
{
    Money,
    Coin,
    WatchAd
}
