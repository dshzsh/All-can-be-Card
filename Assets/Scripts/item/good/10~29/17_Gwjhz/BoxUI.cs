using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxUI : MonoBehaviour,IEscClose
{
    public bool escClose = true;
    public GameObject goodsField;
    public TextMeshProUGUI text;

    private List<BagGoodUI> goodUIs = new();
    private CardBase box;
    private static List<BoxUI> boxUIList = new();
    public static void RefreshAll()
    {
        foreach(BoxUI boxUI in boxUIList)
        {
            boxUI.SetBox(boxUI.box);
        }
    }
    public void SetBox(CardBase box)
    {
        this.box = box;
        List<CardBase> goods = MsgContainerAllItem.GetAllItem(box);

        text.text = CardManager.Cstr(box, noUnderline:true);
        text.GetComponent<ExText>().Refresh();

        for (int i = 0; i < goods.Count; i++)
        {
            while (goodUIs.Count <= i)
            {
                goodUIs.Add(GameObject.Instantiate(BagUI.bagGoodUIPrefab, goodsField.transform).GetComponent<BagGoodUI>());
            }
            goodUIs[i].SetCard(goods[i]);
            if (goodUIs[i].gameObject.activeSelf == false)
                goodUIs[i].gameObject.SetActive(true);
        }
        for (int i = goods.Count; i < goodUIs.Count; i++)
        {
            goodUIs[i].gameObject.SetActive(false);
        }
    }
    public void OnEnable()
    {
        if (escClose)
            SControl_3.AddEscClose(this);
        boxUIList.Add(this);
    }
    private void OnDisable()
    {
        if (escClose)
            SControl_3.RemoveEscClose(this);
        boxUIList.Remove(this);
    }
    public void Close()
    {
        Destroy(gameObject);
    }
}
