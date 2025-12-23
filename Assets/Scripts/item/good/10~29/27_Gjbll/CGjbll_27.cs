using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGjbll_27 : Citem_33
{
    public AttAndRevise attAndRevise = new AttAndRevise(BasicAttID.atk, new BasicAtt());

    public float time;
}
public class DGjbll_27 : DataBase
{
    public DbasicAtt.AttAndValueData attPerCoinData;

    public AttAndRevise attPerCoin;
    public override void Init(int id)
    {
        base.Init(id);
        attPerCoin = attPerCoinData.ToRevise();
    }
}
public class SGjbll_27 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGjbll_27 card = _card as CGjbll_27;
        MsgOnItem msg = _msg as MsgOnItem;

        card.attAndRevise.UseOnLive(card, msg.op);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGjbll_27 card = _card as CGjbll_27;
        MsgUpdate msg = _msg as MsgUpdate;
        DGjbll_27 config = basicConfig as DGjbll_27;

        if (!MyTool.IntervalTime(1, ref card.time, msg.time)) return;

        card.attAndRevise.UseOnLive(card, -1);

        card.attAndRevise.revise = config.attPerCoin.revise.WithPow(card.pow * SGqb_24.GetCoinCnt(card));

        card.attAndRevise.UseOnLive(card, 1);
    }
}