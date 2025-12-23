using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CGwjhz_17 : Citem_33
{
    public string content = "空";
    public int tag = 0;

    [JsonIgnore]
    public List<CardBase> cards = new List<CardBase>();
    [JsonIgnore]
    public BoxUI boxUI;
}
public class DGwjhz_17 : DataBase
{
    public string boxUIName;

    public GameObject boxUI;
    public override void Init(int id)
    {
        boxUI = DataManager.LoadResource<GameObject>(id, boxUIName);
    }
}
public class SGwjhz_17 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.MyInteractItem, MyInteractItem);

        AddHandle(MsgType.SelfContainerJudge, MyContainerJudge);
        AddHandle(MsgType.SelfContainerGet, SelfContainerGet);
        AddHandle(MsgType.SelfContainerAllItem, SelfContainerAllItem);
    }
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        CGwjhz_17 card = _card as CGwjhz_17;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (card != msg.gmsg.container) return;

        if (msg.gmsg.op == 1)
        {
            msg.ok = true;
        }
        else
        {
            msg.gmsg.pos = card.cards.IndexOf(msg.gmsg.item);
            msg.ok = msg.gmsg.pos != -1;
        }
    }
    void SelfContainerGet(CardBase _card, MsgBase _msg)
    {
        CGwjhz_17 card = _card as CGwjhz_17;
        MsgGetItem msg = _msg as MsgGetItem;

        if (msg.op == 1)
        {
            
        }
        else
        {
            msg.pos = card.cards.IndexOf(msg.item);

            if (msg.pos == -1) return;
            msg.item.container = null;
            CardBase item = CreateCard(msg.item.id);
            item.container = card;
            card.cards.Insert(msg.pos, item);
            card.cards.RemoveAt(msg.pos + 1);
        }
    }
    void MyInteractItem(CardBase _card, MsgBase _msg)
    {
        CGwjhz_17 card = _card as CGwjhz_17;
        MsgInteractItem msg = _msg as MsgInteractItem;
        DGwjhz_17 config = basicConfig as DGwjhz_17;
        // 刷新盒子内部的物体
        card.cards.Clear();
        HashSet<int> pool = new HashSet<int>(MyTag.GetPool(card.tag));
        if(card.tag == MyTag.CardTag.good)
        {
            pool.ExceptWith(MyTag.GetPool(MyTag.CardTag.build));
            pool.ExceptWith(MyTag.GetPool(MyTag.CardTag.rule));
        }
        foreach (int id in pool)
        {
            CardBase item = CreateCard(id);
            if(card.tag== MyTag.CardTag.magic)
            {
                AddComponent(item, Sqhc_38.QhcWithSlot(3));
            }
            item.container = card;
            card.cards.Add(item);
        }

        // 打开一个类似背包的界面，展示所有物品
        if (card.boxUI == null)
            card.boxUI = UIBasic.GiveUI(config.boxUI, true).GetComponent<BoxUI>();
        else UIBasic.SetPerfectPos(card.boxUI.gameObject);
        card.boxUI.SetBox(card);
    }
    void SelfContainerAllItem(CardBase _card, MsgBase _msg)
    {
        CGwjhz_17 card = _card as CGwjhz_17;
        MsgContainerAllItem msg = _msg as MsgContainerAllItem;

        msg.items = card.cards;
    }

    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGwjhz_17 card = _card as CGwjhz_17;
        MsgOnItem msg = _msg as MsgOnItem;

    }
    public override Color GetColor(CardBase _card)
    {
        CGwjhz_17 wjhz = _card as CGwjhz_17;
        if (wjhz.tag == MyTag.CardTag.magic)
            return GoodUIColor.Magic;
        if (wjhz.tag == MyTag.CardTag.good)
            return GoodUIColor.Item;
        if (wjhz.tag == MyTag.CardTag.qhstone)
            return GoodUIColor.QhStone;
        if (wjhz.tag == MyTag.CardTag.build)
            return GoodUIColor.Rule;
        return GoodUIColor.Item;
    }
}