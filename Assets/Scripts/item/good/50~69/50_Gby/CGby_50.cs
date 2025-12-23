using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CGby_50 : Citem_33
{
    public CGattItemBase_40 att;
}
public class DGby_50 : DataBase
{

}
public class SGby_50 : Sitem_33
{
    public static int mTAddPermanentAtt = MsgType.ParseMsgType(CardField.good, 50, 0);
    public class MsgAddPermanentAtt: MsgBase
    {
        public AttAndRevise[] atts;
        public bool ok = false;

        public MsgAddPermanentAtt(AttAndRevise[] atts)
        {
            this.atts = atts;
        }
    }

    public static void AddPermanentAtt(CardBase card, params AttAndRevise[] atts)
    {
        if (!TryGetClive(card, out var clive)) return;

        if (clive.myBag == null) // 没有背包，直接把永久属性变化塞身上
        {
            foreach (var att in atts)
                att.UseOnLive(clive, 1);
            return;
        }

        MsgAddPermanentAtt msg = new(atts);
        SendMsg(clive, mTAddPermanentAtt, msg);
        if (msg.ok == false)
        {
            CGby_50 qb = CreateCard<CGby_50>();
            Sbag_40.LiveGetItem(clive, qb);
            msg = new(atts);
            SendMsg(clive, mTAddPermanentAtt, msg);
        }
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        AddComponent(_card, CreateCard<CGQwfxx_15>());
        CGby_50 card = _card as CGby_50;
        card.att = CreateCard<CGattItemBase_40>();
        ActiveComponent(card, card.att);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(mTAddPermanentAtt, AddPermanentAtt);
    }
    void AddPermanentAtt(CardBase _card, MsgBase _msg)
    {
        CGby_50 card = _card as CGby_50;
        MsgAddPermanentAtt msg = _msg as MsgAddPermanentAtt;

        msg.ok = true;
        SGattItemBase_40.AddAtt(card.att, msg.atts);
    }
}