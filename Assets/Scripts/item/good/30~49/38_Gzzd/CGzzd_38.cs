using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGzzd_38 : Citem_33
{
    [NoActiveCard]
    public CardBase curse;
}
public class DGzzd_38 : DataBase
{

}
public class SGzzd_38 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(Sbag_40.mTMyFirstGetItem, MyFirstGetItem);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGzzd_38 card = _card as CGzzd_38;
        card.curse = CreateCard(Sitem_33.GetRandomItem(3, MyTag.CardTag.good, MyTag.CardTag.Pcurse));
    }
    void MyFirstGetItem(CardBase _card, MsgBase _msg)
    {
        CGzzd_38 card = _card as CGzzd_38;
        MsgGetItem msg = _msg as MsgGetItem;
        DGzzd_38 config = basicConfig as DGzzd_38;

        Sbag_40.LiveGetItem(msg.container, NewCopy(card.curse));
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGzzd_38 card = _card as CGzzd_38;
        MsgOnItem msg = _msg as MsgOnItem;
        DGzzd_38 config = basicConfig as DGzzd_38;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj.myMagic == null) return;

        Smagic_14.ChangeHoldMax(cobj.myMagic, msg.op);
        Smagic_14.ChangeHoldMax(cobj.myMagic, msg.op);
    }
}