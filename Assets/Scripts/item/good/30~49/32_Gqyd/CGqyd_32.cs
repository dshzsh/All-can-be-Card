using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGqyd_32 : Citem_33
{

}
public class DGqyd_32 : DataBase
{

}
public class SGqyd_32 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGqyd_32 card = _card as CGqyd_32;
        MsgOnItem msg = _msg as MsgOnItem;
        DGqyd_32 config = basicConfig as DGqyd_32;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj.myMagic == null) return;
        
        Smagic_14.ChangeHoldMax(cobj.myMagic, msg.op);
    }
}