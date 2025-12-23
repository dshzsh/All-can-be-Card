using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBphq_13 : Cbullet_10
{
    public float time = 1;
}
public class DBphq_13: DataBase
{
    public float bigRate;
    public float maxBigTime;
    public float speedUpRate;
    public float maxSpeedUpTime;
}
public class SBphq_13 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CBphq_13 card = _card as CBphq_13;
        MsgUpdate msg = _msg as MsgUpdate;
        DBphq_13 config = basicConfig as DBphq_13;

        float oldSize = card.time;
        card.time += msg.time;
        card.obj.transform.localScale *= Mathf.Pow(Mathf.Min(1 + config.maxBigTime, card.time) / oldSize, config.bigRate);
    }
}