using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMtcj_28 : Cmagicbase_17
{

}
public class DMtcj_28 : DataBase
{
    public float damage;
    public int cnt;
    public float intervalDis;
    public float stunTime;
}
public class SMtcj_28 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBtcj_16));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMtcj_28 card = _card as CMtcj_28;
        DMtcj_28 config = basicConfig as DMtcj_28;
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;

        if (TryGetCobj(card, out var cobj))
        {
            pos = cobj.obj.transform.position;
            dir = MyMath.V3zeroYNor(msg.pos - pos);
        }

        for (int i = 0; i < config.cnt; i++)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            ScolAddBuff_43.AddBuff<CFyx_11>(bmsg, config.stunTime, Sbuff_35.BeBuffMode.coverByBig);
            pos += config.intervalDis * dir;
            bmsg.initPos = pos;
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);

            Sbullet_10.GiveBullet(bid, bmsg);
        }
    }
}