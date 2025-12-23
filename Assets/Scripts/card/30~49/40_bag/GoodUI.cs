using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MsgMyShowGoodUI: MsgBase
{
    public GoodUI goodUI;
    public GameObject UIobj;
    public int op;

    public MsgMyShowGoodUI(GoodUI goodUI, GameObject UIobj, int op)
    {
        this.goodUI = goodUI;
        this.UIobj = UIobj;
        this.op = op;
    }
}
public class GoodUI : MonoBehaviour
{
    public CardBase good;
    public Image image;
    public TMPro.TextMeshProUGUI text;
    public bool ShowGoodUI = false;

    public GameObject upPanel;
    private Dictionary<CardBase, GameObject> cardToObj;

    public void SetCard(CardBase good)
    {
        if (this.good == good) return;
        if (ShowGoodUI)
            SystemManager.SendMsg(this.good, MsgType.MyShowGoodUI, new MsgMyShowGoodUI(this, gameObject, -1));
        
        this.good = good;
        text.text = good.id == 0 ? "" : DataManager.GetName(good.id);
        image.color = CardManager.GetCardColor(good);
        
        if (ShowGoodUI)
            SystemManager.SendMsg(good, MsgType.MyShowGoodUI, new MsgMyShowGoodUI(this, gameObject, 1));
    }

    public void GiveUI(CardBase fromCard, GameObject ui)
    {
        if (cardToObj == null) cardToObj = new();
        ui.transform.SetParent(upPanel.transform, false);
        cardToObj[fromCard] = ui;
    }
    public void RemoveUI(CardBase fromCard)
    {
        if (cardToObj == null) cardToObj = new();
        if(cardToObj.ContainsKey(fromCard))
        {
            GameObject.Destroy(cardToObj[fromCard]);
            cardToObj[fromCard] = null;
        }
    }
}
