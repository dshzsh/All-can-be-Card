using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using System;

public class Cbuff_35 : CardBase
{
    public List<Sbuff_35.BuffInfo> buffInfos = new List<Sbuff_35.BuffInfo>();
}
public class MsgBeBuff : MsgBase
{
    public CardBase from;
    public CardBase to;

    public CardBase buff;
    public float time = 0;
    public int itemFrom = 0;
    public Sbuff_35.BeBuffMode beBuffMode = Sbuff_35.BeBuffMode.add;
    public bool IsSpeTime()
    {
        return time >= Sbuff_35.BuffSpeTime.SpeTimeBase;
    }
    public MsgBeBuff(CardBase buff, float time, int itemFrom=0, Sbuff_35.BeBuffMode beBuffMode=Sbuff_35.BeBuffMode.add)
    {
        this.buff = buff;
        this.time = time;
        this.itemFrom = itemFrom;
        this.beBuffMode = beBuffMode;
    }
    public MsgBeBuff() { }
}
public class Sbuff_35: SystemBase
{
    public static class BuffSpeTime
    {
        public static float SpeTimeBase = 10000f;

        public static float Inf = SpeTimeBase + 1f;
        public static float ToFightEnd = SpeTimeBase;
    }
    public class BuffInfo
    {
        public float time;
        public int itemFrom;
        public CardBase from;
        public CardBase buff;
    }
    public enum BeBuffMode
    {
        add,//直接加入
        coverByBig,//覆盖已有buff，选取威力和时间最大
        coverByNew,//覆盖已有buff，选取新的威力与时间
        stackPow,//叠加威力，时间取最大
        stackTime//叠加持续时间，威力取旧
    }
    public static void GiveBuff(CardBase from, CardBase to, MsgBeBuff msg)
    {
        msg.from = from;
        msg.to = to;
        SendMsg(from, MsgType.GiveBuff, msg);
        SendMsg(to, MsgType.BeBuff, msg);
    }
    public static BuffInfo GetBuff(CardBase card, int buffId, int itemFrom = -1)
    {
        if (!TryGetCobj(card, out var cObj)) return null;
        if (cObj.myBuff == null) return null;
        foreach(var buffInfo in cObj.myBuff.buffInfos)
        {
            if (buffInfo.time < 0 || !CardValid(buffInfo.buff)) continue;
            if (buffInfo.buff.id != buffId) continue;
            if (itemFrom != -1 && buffInfo.itemFrom != itemFrom) continue;
            return buffInfo;
        }
        return null;
    }
    public override void Init()
    {
        AddHandle(MsgType.BeBuff, BeBuff);
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(SLFfightBase_6.mTFightEnd, FightEnd);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        Cbuff_35 card = _card as Cbuff_35;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetCobj(card, out var cobj)) return;

        if(msg.op==1)
        {
            cobj.myBuff = card;
        }
        else
        {
            cobj.myBuff = null;
        }
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Cbuff_35 card = _card as Cbuff_35;
        MsgUpdate msg = _msg as MsgUpdate;

        for (int i = 0; i < card.buffInfos.Count; i++)
        {
            BuffInfo bInfo = card.buffInfos[i];
            if (bInfo.time >= BuffSpeTime.SpeTimeBase)
                continue;
            bInfo.time -= msg.time;
            if (bInfo.time < 0 || !CardValid(bInfo.buff))
            {
                OffBuff(card, i);
                i--;
            }
        }
    }
    void FightEnd(CardBase _card, MsgBase _msg)
    {
        Cbuff_35 card = _card as Cbuff_35;
        MsgUpdate msg = _msg as MsgUpdate;

        for (int i = 0; i < card.buffInfos.Count; i++)
        {
            BuffInfo bInfo = card.buffInfos[i];
            if (bInfo.time != BuffSpeTime.ToFightEnd)
                continue;
            bInfo.time = 0;
            OffBuff(card, i);
            i--;
        }
    }
    private void AddBuff(Cbuff_35 mybuff, MsgBeBuff msg)
    {
        CardBase buff = msg.buff;
        BuffInfo buffInfo = new BuffInfo() { from = msg.from, itemFrom = msg.itemFrom, time = msg.time, buff = msg.buff };
        mybuff.buffInfos.Add(buffInfo);
        if (buff is Cbuffbase_36 cbuff) cbuff.buffInfo = buffInfo;
        ActiveComponent(mybuff, buff);
        //SendMsg(buff, MsgType.OnItem, new MsgOnItem { item = buff, op = 1 });
    }
    private void OffBuff(Cbuff_35 mybuff, int index)
    {
        if (index == -1) return;
        InactiveComponent(mybuff, mybuff.buffInfos[index].buff);
        mybuff.buffInfos.RemoveAt(index);
    }
    void BeBuff(CardBase _card, MsgBase _msg)
    {
        Cbuff_35 card = _card as Cbuff_35;
        MsgBeBuff msg = _msg as MsgBeBuff;

        if(msg.beBuffMode == BeBuffMode.add)
        {
            AddBuff(card, msg);
            return;
        }

        int oldBuffIndex = -1;
        for (int i = 0; i < card.buffInfos.Count; i++)
        {
            BuffInfo bInfo = card.buffInfos[i];
            if (bInfo.itemFrom == msg.itemFrom && bInfo.buff.id == msg.buff.id)
            {
                oldBuffIndex = i;
                break;
            }
        }

        if (oldBuffIndex == -1)//没有目标buff，直接加入不额外操作
        {
            AddBuff(card, msg);
            return;
        }

        BuffInfo oldInfo = card.buffInfos[oldBuffIndex];
        switch (msg.beBuffMode)
        {
            case BeBuffMode.coverByNew:
                {
                    if (msg.buff is Citem_33 item)
                    {
                        Citem_33 oldCard = oldInfo.buff as Citem_33;
                        OffBuff(card, oldBuffIndex);
                        msg.buff = oldCard;
                        oldCard.pow = item.pow;// 将旧卡的时间和威力更新成新的状态，其余不变
                        AddBuff(card, msg);
                    }
                    else
                    {
                        OffBuff(card, oldBuffIndex);
                        AddBuff(card, msg);
                    }
                    break;
                }
            case BeBuffMode.coverByBig:
                {
                    if (msg.buff is Citem_33 item)
                    {
                        Citem_33 oldCard = oldInfo.buff as Citem_33;
                        OffBuff(card, oldBuffIndex);
                        msg.buff = oldCard;
                        // 将旧卡的时间和威力更新成最大
                        msg.time = Mathf.Max(oldInfo.time, msg.time);
                        oldCard.pow = Mathf.Max(item.pow, oldCard.pow);
                        AddBuff(card, msg);
                    }
                    else
                    {
                        OffBuff(card, oldBuffIndex);
                        AddBuff(card, msg);
                    }
                    break;
                }
            case BeBuffMode.stackPow:
                {
                    //优先保留老的card
                    if(msg.buff is Citem_33 item)
                    {
                        Citem_33 oldCard = oldInfo.buff as Citem_33;
                        OffBuff(card, oldBuffIndex);
                        msg.buff = oldCard;
                        oldCard.pow += item.pow;
                        msg.time = Mathf.Max(oldInfo.time, msg.time);
                        AddBuff(card, msg);
                    }
                    break;
                }
            case BeBuffMode.stackTime:
                {
                    oldInfo.time += msg.time;
                    break;
                }
        }
        
    }
}