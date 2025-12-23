using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGbyld_55 : Citem_33
{
    public CGattItemBase_40 att;
}
public class DGbyld_55 : DataBase
{
    public DbasicAtt.AttAndReviseData att1Data, att2Data;

    public AttAndRevise att1, att2;
    public override void Init(int id)
    {
        att1 = att1Data.ToRevise();
        att2 = att2Data.ToRevise();
    }
}
public class SGbyld_55 : Sitem_33
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGbyld_55 card = _card as CGbyld_55;
        card.att = CreateCard<CGattItemBase_40>();
        ActiveComponent(card, card.att);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGbyld_55 card = _card as CGbyld_55;
        MsgUpdate msg = _msg as MsgUpdate;
        DGbyld_55 config = basicConfig as DGbyld_55;

        if(SFwfsf_12.InWfsf(msg.cobj))
        {
            SGattItemBase_40.ChangeAtt(card.att, config.att1.WithPow(card.pow), config.att2.WithPow(card.pow));
        }
        else
        {
            SGattItemBase_40.ChangeAtt(card.att);
        }
    }
}