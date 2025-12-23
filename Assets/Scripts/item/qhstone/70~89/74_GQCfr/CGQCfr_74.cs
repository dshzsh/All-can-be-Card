using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCfr_74 : CGqhsbase_11
{
    public float zsPow;
}
public class DGQCfr_74 : DataBase
{

}
public class SGQCfr_74 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageBefore, GiveDamageBefore);
    }
    void GiveDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGQCfr_74 card = _card as CGQCfr_74;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGQCfr_74 config = basicConfig as DGQCfr_74;

        Sbuff_35.BuffInfo buffInfo = Sbuff_35.GetBuff(msg.to, SFzs_1.zsID);
        if (buffInfo == null) return;
        CFzs_1 citem = buffInfo.buff as CFzs_1;
        msg.damage += citem.damage * citem.pow * card.zsPow * Sbullet_10.GetBulletPow(card);
    }
}