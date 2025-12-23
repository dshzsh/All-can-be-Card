using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour, IEscClose
{
    public static GameObject bagGoodUIPrefab;

    public GameObject goodsField;
    public GoodSelectUI selectUI;

    private List<BagGoodUI> goodUIs = new();
    public CardBase bag;
    //public List<Button> buttons = new List<Button>();

    public void SetBag(CardBase bag, List<CardBase> goods)
    {
        this.bag = bag;
        SelectLive();
        for (int i = 0; i < goods.Count; i++)
        {
            while (goodUIs.Count <= i)
            {
                goodUIs.Add(GameObject.Instantiate(bagGoodUIPrefab, goodsField.transform).GetComponent<BagGoodUI>());
            }
            goodUIs[i].SetCard(goods[i], this);
            if (goodUIs[i].gameObject.activeSelf == false)
                goodUIs[i].gameObject.SetActive(true);
        }
        for (int i = goods.Count; i < goodUIs.Count; i++)
        {
            goodUIs[i].gameObject.SetActive(false);
        }
    }
    public void OpenBag()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        //Stime_41.PauseTime(1);
        SControl_3.AddNoControl(1);
        SControl_3.AddEscClose(this);
    }
    public void Close()
    {
        gameObject.SetActive(false);
        //Stime_41.PauseTime(-1);
        SControl_3.AddNoControl(-1);
        SControl_3.RemoveEscClose(this);
    }
    public void Select(CardBase good)
    {
        if (selectUI != null)
            selectUI.SetCard(good);
    }
    public void SelectLive()
    {
        if (CardManager.CardValid(bag))
            Select(CardManager.GetTop(bag));
    }
}
