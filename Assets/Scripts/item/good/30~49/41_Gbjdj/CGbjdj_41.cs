using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGbjdj_41 : Citem_33
{
    public CGattItemBase_40 att;
    public int cen = 0;
}
public class DGbjdj_41 : DataBase
{
    public DbasicAtt.AttAndValueData critAddData;

    public AttAndRevise critAdd;
    public override void Init(int id)
    {
        base.Init(id);
        critAdd = critAddData.ToRevise();
    }
}
public class SGbjdj_41 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageAfter, GiveDamageAfter);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGbjdj_41 card = _card as CGbjdj_41;
        card.att = CreateCard<CGattItemBase_40>();
        ActiveComponent(card, card.att);
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGbjdj_41 card = _card as CGbjdj_41;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGbjdj_41 config = basicConfig as DGbjdj_41;

        if(msg.TryGetTag<CTTcrit_1>(out var _))
        {
            card.cen = 0;
            SGattItemBase_40.ChangeAtt(card.att);
        }
        else
        {
            card.cen++;
            SGattItemBase_40.ChangeAtt(card.att, config.critAdd.WithPow(card.pow * card.cen));
        }
    }
}