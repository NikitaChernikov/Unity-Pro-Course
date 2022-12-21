using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] List<GameObject> canvas;
    bool isOpen;
    PlayerController playerController;
    //отображение стоимости скина
    [SerializeField] List<Text> costText = new List<Text>();
    //текущий скин
    [SerializeField] GameObject currentSkin;
    //массив с данными для наших скинов
    public Skin[] skin;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        for (var i = 0; i < skin.Length; i++)
        {
            costText[i].text = skin[i].price.ToString() + " Gold";
        }
    }

    public void BuyAmmo(int count)
    {
        if (playerController.money >= count * 2)
        {
            playerController.AddAmmo(count);
            playerController.GetMoney(-count * 2);
        }
    }

    public void BuySkin(int count)
    {
        if (count > skin.Length)
        {
            return;
        }
        if (skin[count].price <= playerController.money && skin[count].isBuy == false)
        {
            //Покупаем
            costText[count].text = "Sold";
            skin[count].isBuy = true;
            playerController.GetMoney(-skin[count].price);
        }
        if (skin[count].isBuy == true)
        {
            //Переключаемся
            currentSkin.SetActive(false);
            currentSkin = skin[count].skinToBuy;
            currentSkin.SetActive(true);
        }
    }

    public void OpenShop()
    {
        if (!isOpen)
        {
            canvas[0].SetActive(false);
            canvas[1].SetActive(true);
            isOpen = true;
        }
        else
        {
            canvas[1].SetActive(false);
            canvas[0].SetActive(true);
            isOpen = false;
        }
    }
}

[System.Serializable]
public class Skin
{
    //скин, который мы хотим купить
    public GameObject skinToBuy;
    //стоимость скина
    public int price;
    //куплен ли скин?
    public bool isBuy;
}


