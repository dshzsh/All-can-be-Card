using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGcjsfq_36 : Citem_33
{

}
public class DGcjsfq_36 : DataBase
{

}
public class SGcjsfq_36 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.UseMagicAfter, UseMagicAfter);
    }
    void UseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGcjsfq_36 card = _card as CGcjsfq_36;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGcjsfq_36 config = basicConfig as DGcjsfq_36;

        if (!msg.isConUse) return;

        Smagic_14.RecoverMagicCd(msg.magic, 1, true);
    }
}