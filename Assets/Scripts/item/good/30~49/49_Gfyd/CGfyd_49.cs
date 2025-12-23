using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGfyd_49 : Citem_33
{
    public float time;
}
public class DGfyd_49 : DataBase
{
    public float interval, recoverMana;
}
public class SGfyd_49 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGfyd_49 card = _card as CGfyd_49;
        MsgOnItem msg = _msg as MsgOnItem;
        DGfyd_49 config = basicConfig as DGfyd_49;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj.myMagic == null) return;

        Smagic_14.ChangeHoldMax(cobj.myMagic, msg.op);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGfyd_49 card = _card as CGfyd_49;
        MsgUpdate msg = _msg as MsgUpdate;
        DGfyd_49 config = basicConfig as DGfyd_49;

        if (MyTool.IntervalTime(config.interval / card.pow, ref card.time, msg.time))
        {
            float recover = config.recoverMana * Shealth_4.GetAttf(card, BasicAttID.manaMax);
            SendMsg(GetTop(card), MsgType.RestoreMana, new MsgRestoreMana(recover));
        }
    }
}