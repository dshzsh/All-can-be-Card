using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCpzlq_61 : CGqhsbase_11
{
    public int key = -1;
    public float cdRecover = 0;
}

public class SGQCpzlq_61 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageAfter, GiveDamageAfter);
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGQCpzlq_61 card = _card as CGQCpzlq_61;
        MsgBeDamage msg = _msg as MsgBeDamage;

        CardBase from = Sysw_26.GetYsw(card);
        Smagic_14.RecoverMagicCd(card.key, from, card.cdRecover, true);
    }
}