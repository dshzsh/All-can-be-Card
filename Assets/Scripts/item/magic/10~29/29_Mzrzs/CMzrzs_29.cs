using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMzrzs_29 : Cmagicbase_17
{
    public float time;
}
public class DMzrzs_29 : DataBase
{
    public float healRate;
    public float interval;
}
public class SMzrzs_29 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMzrzs_29 card = _card as CMzrzs_29;
        MsgUpdate msg = _msg as MsgUpdate;
        DMzrzs_29 config = basicConfig as DMzrzs_29;

        if(MyTool.IntervalTime(config.interval, ref card.time, msg.time))
        {
            CardBase live = GetTop(card);
            Shealth_4.GiveHeal(live, live, 
                new MsgBeHeal(config.healRate * card.pow * Shealth_4.GetAttf(card, BasicAttID.healthMax)));
        }
    }
}