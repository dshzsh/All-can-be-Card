using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static Sbuff_35;
using static SystemManager;

public class CcolAddBuff_43 : CardBase
{
    public MsgBeBuff bebuff;
    public bool useBulletPow = false;
    public Slive_19.FindLiveMode colMode = Slive_19.FindLiveMode.enemy;
}

public class ScolAddBuff_43: SystemBase
{
    public static CcolAddBuff_43 MakeCard<T>(CardBase from, float time, Sbuff_35.BeBuffMode beBuffMode = Sbuff_35.BeBuffMode.add, bool useBulletPow = true) where T : CardBase
    {
        T buff = CreateCard<T>();
        MsgBeBuff bebuff = new MsgBeBuff(buff, time, 0, beBuffMode);
        bebuff.from = from;
        CcolAddBuff_43 colbuff = CreateCard<CcolAddBuff_43>();
        colbuff.bebuff = bebuff;
        colbuff.useBulletPow = useBulletPow;
        return colbuff;
    }
    public static CcolAddBuff_43 MakeCard(MsgBeBuff beBuff, bool useBulletPow = true)
    {
        CcolAddBuff_43 colbuff = CreateCard<CcolAddBuff_43>();
        colbuff.bebuff = beBuff;
        colbuff.useBulletPow = useBulletPow;
        return colbuff;
    }
    public static void AddBuff<T>(MsgBullet bmsg, float time, Sbuff_35.BeBuffMode beBuffMode = Sbuff_35.BeBuffMode.add, bool useBulletPow = true) where T : Cbuffbase_36
    {
        bmsg.addCards.Add(MakeCard<T>(bmsg.from, time, beBuffMode, useBulletPow));
    }
    public static void AddBuff(MsgBullet bmsg, MsgBeBuff msgBeBuff, bool useBulletPow = false)
    {
        msgBeBuff.from = bmsg.from;
        CcolAddBuff_43 colbuff = CreateCard<CcolAddBuff_43>();
        colbuff.bebuff = msgBeBuff;
        colbuff.useBulletPow = useBulletPow;
        bmsg.addCards.Add(colbuff);
    }
    public override void Clone(CardBase _from, CardBase _to)
    {
        base.Clone(_from, _to);
        CcolAddBuff_43 card = _from as CcolAddBuff_43;
        CcolAddBuff_43 ocard = _to as CcolAddBuff_43;
        card.bebuff = ocard.bebuff;
    }
    public override void Init()
    {
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        CcolAddBuff_43 card = _card as CcolAddBuff_43;
        MsgCollision msg = _msg as MsgCollision;

        if (msg.other == null) return;

        if (TryGetCobj(card, out var obj1) && TryGetCobj(msg.other, out var obj2))
        {
            if (!Slive_19.TeamSatisfy(obj1.team, obj2.team, card.colMode)) return;
        }

        // 如果是item类，额外吃到bullet pow的加成
        MsgBeBuff nmsg = new MsgBeBuff(NewCopy(card.bebuff.buff), card.bebuff.time, card.bebuff.itemFrom, card.bebuff.beBuffMode);

        if (card.useBulletPow && nmsg.buff is Citem_33 citem)
        {
            citem.pow *= Sbullet_10.GetBulletPow(card);
        }

        Sbuff_35.GiveBuff(card.bebuff.from, msg.other, nmsg);
    }
}