using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCyzz_62 : CGqhsbase_11
{
    public int key = -1;
    public float cdRecover = 0;
}
public class DGQCyzz_62 : DataBase
{

}
public class SGQCyzz_62 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageAfter, GiveDamageAfter);
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGQCyzz_62 card = _card as CGQCyzz_62;
        MsgBeDamage msg = _msg as MsgBeDamage;

        var bif = Sbuff_35.GetBuff(msg.to, SFyzzbj_18.bid);
        if (bif == null) return;
        bif.time = -1;

        CardBase from = Sysw_26.GetYsw(card);
        Smagic_14.RecoverMagicCd(card.key, from, card.cdRecover, true);
    }
}