using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGxjbs_15 : Citem_33
{

}
public class DGxjbs_15 : DataBase
{
    public float cdPercent;
}
public class SGxjbs_15 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.UseMagicAfter, UseMagicAfter);
    }
    void UseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGxjbs_15 card = _card as CGxjbs_15;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGxjbs_15 config = basicConfig as DGxjbs_15;

        Smagic_14.RecoverMagicCd(msg.magic, config.cdPercent * card.pow, true);
    }
}