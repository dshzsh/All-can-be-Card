using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFzs_1 : Cbuffbase_36
{
    public float time = 0.2f;
    public float interval = 1f;
    public float damage = 1f;

    public float speedUp = 0;
}
public class DFzs_1 : DataBase
{

}
public class MsgGiveZsDamageAfter : MsgBase
{
    public CFzs_1 zsBuff;
    public MsgBeDamage dmsg;

}
public class SFzs_1 : Sbuffbase_36
{
    public static int zsID;
    public static int mTgiveZsDamageAfter = MsgType.ParseMsgType(GetTypeId(typeof(CFzs_1)), 0);
    public static void AddZs(MsgBullet bmsg, float damage, float time = 3)
    {
        CFzs_1 buff = CreateCard<CFzs_1>();
        buff.interval = 1;
        buff.damage = 1;
        buff.pow = damage;
        MsgBeBuff bebuff = new MsgBeBuff(buff, time, 0, Sbuff_35.BeBuffMode.coverByBig);
        bebuff.from = bmsg.from;
        CcolAddBuff_43 colbuff = CreateCard<CcolAddBuff_43>();
        colbuff.bebuff = bebuff;
        colbuff.useBulletPow = false;
        bmsg.addCards.Add(colbuff);
    }
    public static void GiveZsDamage(CardBase _card, float exPow = 1)
    {
        CFzs_1 card = _card as CFzs_1;
        CardBase from = card.buffInfo == null ? null : card.buffInfo.from;
        MsgBeDamage dmsg = new MsgBeDamage(card.damage * card.pow * exPow, SGRwx_4.WxTag.huo);
        Shealth_4.GiveDamage(from, GetTop(card), dmsg);
        SendMsg(from, mTgiveZsDamageAfter, new MsgGiveZsDamageAfter() { dmsg = dmsg, zsBuff = card });
    }
    public override void Init()
    {
        base.Init();
        zsID = id;
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CFzs_1 card = _card as CFzs_1;
        MsgUpdate msg = _msg as MsgUpdate;

        if(MyTool.IntervalTime(card.interval/(1+card.speedUp), ref card.time, msg.time))
        {
            GiveZsDamage(card);
        }
    }
}