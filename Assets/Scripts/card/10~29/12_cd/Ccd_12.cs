using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Ccd_12 : CardBase
{
    public float cur = 0f;
    public float max = 0f;
}

public class Scd_12 : SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Ccd_12 card = _card as Ccd_12;
        MsgUpdate msg = _msg as MsgUpdate;

        card.cur += msg.time;
        
    }
}