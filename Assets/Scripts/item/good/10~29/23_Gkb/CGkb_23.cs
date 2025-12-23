using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGkb_23 : Citem_33
{
    public BasicAtt atkAdd = new();
}
public class DGkb_23 : DataBase
{
    public float healthRate;
    public BasicAtt atkAddMax;
}
public class SGkb_23 : Sitem_33
{
    public static float GetNowHealthRate(CardBase card)
    {
        float nowHealth = Shealth_4.GetNowHealth(card);
        float maxHealth = Shealth_4.GetAttf(card, BasicAttID.healthMax);
        if (maxHealth == 0) return 1;
        return Mathf.Clamp(nowHealth / maxHealth, 0, 1);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGkb_23 card = _card as CGkb_23;
        MsgOnItem msg = _msg as MsgOnItem;

        new AttAndRevise(BasicAttID.atk, card.atkAdd).UseOnLive(card, msg.op);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGkb_23 card = _card as CGkb_23;
        MsgUpdate msg = _msg as MsgUpdate;
        DGkb_23 config = basicConfig as DGkb_23;

        new AttAndRevise(BasicAttID.atk, card.atkAdd).UseOnLive(card, -1);

        card.atkAdd.DirectMul = config.atkAddMax.DirectMul *
            MyMath.LinerMap(1, 1 - config.healthRate, 0, 1, GetNowHealthRate(card));

        new AttAndRevise(BasicAttID.atk, card.atkAdd).UseOnLive(card, 1);
    }
}