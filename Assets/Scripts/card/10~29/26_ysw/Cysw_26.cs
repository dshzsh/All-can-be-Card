using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static CardManager;
using static SfloorBase_2;
using static SLFfightBase_6;
using static Slive_19;
using static SystemManager;

public class Cysw_26 : CardBase
{
    public CardBase from;
}
public class Sysw_26: SystemBase
{
    public static int fromYswBuff;
    public static int mTSummonYsw = MsgType.ParseMsgType(CardField.card, 26, 0);
    public class MsgSummonYsw : MsgBase
    {
        public CardBase from;
        public MsgMagicUse fromMagicUse;
        public int team;
        public Vector3 pos;
        public Quaternion rot;
        public List<CardBase> addCards = new List<CardBase>();
        public float dis = 1.5f;

        public int id;
        public float health = -1, atk = -1, def = -1;
        public float time = -1;
        public MsgSummonYsw(MsgMagicUse useMsg)
        {
            from = useMsg.live;
            fromMagicUse = useMsg;

            if (TryGetCobj(useMsg.live, out var obj))
            {
                Vector3 dir = MyMath.V3zeroYNor(useMsg.pos - obj.obj.Center);
                rot = Quaternion.LookRotation(dir);

                pos = obj.obj.transform.position + dir * dis;
                team = obj.team;
            }

            if (useMsg.mks != null && useMsg.mks.TryGetValue(MsgMagicUse.AddCardType.ysw, out var add))
                addCards.AddRange(add);
        }
        public MsgSummonYsw(MsgMagicUse useMsg, DYswAtt yswAtt) : this(useMsg)
        {
            health = Shealth_4.GetAttf(from, BasicAttID.healthMax) * yswAtt.hPow * useMsg.pow;
            atk = Shealth_4.GetAttf(from, BasicAttID.atk) * yswAtt.aPow * useMsg.pow;
            def = Shealth_4.GetAttf(from, BasicAttID.def) * yswAtt.dPow * useMsg.pow;
            time = yswAtt.time;
        }
    
        public void AddCard(CardBase buff)
        {
            addCards.Add(buff);
        }
    }
    public static Clive_19 SummonYsw(MsgSummonYsw msg)
    {
        SendMsg(msg.from, mTSummonYsw, msg);

        Clive_19 live = CreateCard(msg.id) as Clive_19;
        AddToWorld(live);
        live.obj.transform.SetPositionAndRotation(msg.pos, msg.rot);
        live.team = msg.team;

        if(live.myHealth != null)
        {
            if (msg.health > 0)
                live.myHealth.GetAtt(BasicAttID.healthMax).value = msg.health;
            if (msg.atk > 0)
                live.myHealth.GetAtt(BasicAttID.atk).value = msg.atk;
            if (msg.def > 0)
                live.myHealth.GetAtt(BasicAttID.def).value = msg.def;
        }

        if (msg.from != null)
        {
            Cysw_26 cysw = CreateCard<Cysw_26>(); cysw.from = msg.from;
            AddComponent(live, cysw);
        }
        if (msg.addCards.Count > 0)
        {
            foreach (CardBase addcard in msg.addCards)
            {
                Sbuff_35.GiveBuff(null, live, new MsgBeBuff() { buff = NewCopy(addcard), itemFrom = fromYswBuff, time = Sbuff_35.BuffSpeTime.Inf });
            }
        }
        if(msg.time>0)
        {
            Ctimedes_7 ctimedes = CreateCard<Ctimedes_7>(); ctimedes.timeRes = msg.time;

            AddComponent(live, ctimedes);
        }

        // 衍生物默认满血
        if (live.myHealth != null)
        {
            live.myHealth.nowHealth = live.myHealth.GetAttf(BasicAttID.healthMax);
        }
        return live;
    }

    public static int mTGetYsw = MsgType.ParseMsgType(CardField.card, 26, 1);
    public class MsgGetYsw : MsgBase
    {
        public CardBase from;
    }

    public static CardBase GetYsw(CardBase card)
    {
        MsgGetYsw msg = new MsgGetYsw();
        SendMsg(GetTop(card), mTGetYsw, msg);
        return msg.from;
    }
    public override void Init()
    {
        base.Init();
        fromYswBuff = id;
        AddHandle(mTGetYsw, GetYsw);
    }
    void GetYsw(CardBase _card, MsgBase _msg)
    {
        Cysw_26 card = _card as Cysw_26;
        MsgGetYsw msg = _msg as MsgGetYsw;

        msg.from = card.from;
    }
}