using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCfr_72 : CGqhsbase_11
{
    public float zsPow;
}
public class DGQCfr_72 : DataBase
{

}
public class SGQCfr_72 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        CGQCfr_72 card = _card as CGQCfr_72;
        MsgCollision msg = _msg as MsgCollision;
        DGQCfr_72 config = basicConfig as DGQCfr_72;

        if (msg.other == null) return;

        if (TryGetCobj(card, out var obj1) && TryGetCobj(msg.other, out var obj2))
        {
            if (Slive_19.TeamSatisfy(obj1.team, obj2.team, Slive_19.FindLiveMode.friend)) return;
        }
        
        Sbuff_35.BuffInfo buffInfo = Sbuff_35.GetBuff(msg.other, SFzs_1.zsID);
        if(buffInfo == null) return;
        SFzs_1.GiveZsDamage(buffInfo.buff, card.zsPow * card.pow * Sbullet_10.GetBulletPow(card));
    }
}