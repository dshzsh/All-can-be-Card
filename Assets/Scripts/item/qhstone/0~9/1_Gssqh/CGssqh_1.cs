using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGssqh_1 : CGqhsbase_11
{
    public float exPow;
    public float scatterRange;
    public int exTimes;
}
public class DGssqh_1 : DataBase
{
    public float exPow;
    public float scatterRange;
    public int exTimes;
}
public class SGssqh_1 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, UseMagic, HandlerPriority.Before);
    }
    public override void Create(CardBase _card)
    {
        CGssqh_1 card = _card as CGssqh_1;
        DGssqh_1 config = basicConfig as DGssqh_1;
        card.scatterRange = config.scatterRange;
        card.exTimes = config.exTimes;
        card.exPow = config.exPow;
    }
    void UseMagic(CardBase _card, MsgBase _msg)
    {
        CGssqh_1 card = _card as CGssqh_1;
        MsgMagicUse msg = _msg as MsgMagicUse;
        //DGssqh_1 config = basicConfig as DGssqh_1;

        if (!Sqhc_38.IsQhMagic(card, msg.magic)) return;

        for(int i=0;i< card.exTimes;i++)
        {
            MsgMagicUse nmsg = new MsgMagicUse(msg);
            nmsg.mdata.windUp = nmsg.mdata.windDown = 0f;
            nmsg.pow *= card.exPow * card.pow;
            nmsg.ToNoCost();

            if (TryGetCobj(msg.live, out var cObj))
            {
                Vector3 dir = msg.pos - cObj.obj.Center;
                nmsg.pos = MyTool.RandScatter(dir, card.scatterRange) + cObj.obj.Center;
            }

            UseTriList(nmsg, msg.triList, msg.triPos + 1);
        }
    }
}