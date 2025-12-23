using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CEBmove_31 : CardBase
{
    public float findLiveMaxDis = 50f;
    public float mbMinDis = 2.2f;
    public float mbMaxDis = 2.5f;

    public float findLiveTime = 0.3f;
    public float time = 0f;
    public Vector3 dir = Vector3.zero;
    public Clive_19 mb;
}
public class DenemyMoveBrain : DataBase
{
    public float findEnemyDis = 50f;
    public float atkDis = -1f;
    public float mbMinDis;
    public float mbMaxDis;
}
public class SEBmove_31: SystemBase
{
    public virtual Clive_19 FindLive(CEBmove_31 card, CObj_2 live)
    {
        Clive_19 mb = Slive_19.FindLive(live, Slive_19.FindLiveMode.enemy, card.findLiveMaxDis);

        if (mb == null) return null;
        if (Slive_19.GetDistance(mb, live) > card.findLiveMaxDis) return null;
        return mb;
    }
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CEBmove_31 card = _card as CEBmove_31;
        MsgUpdate msg = _msg as MsgUpdate;

        if (!TryGetCobj(card, out var live)) return;

        card.time += msg.time;
        if (card.time > card.findLiveTime)
        {
            card.time = 0;
            card.mb = FindLive(card, live);
        }

        if (CardValid(card.mb))
        {
            //计算直线的方向
            card.dir = card.mb.obj.transform.position - live.obj.transform.position;
            card.dir.y = 0; card.dir = card.dir.normalized;
            Vector3 sendDir = card.dir;
            //整合最近最远设置
            float dis = Slive_19.GetDistance(card.mb, live);
            if (dis < card.mbMinDis) sendDir = -sendDir;
            else if (dis < card.mbMaxDis) sendDir = Vector3.zero;

            SendMsg(live, MsgType.MoveControl, new MsgMoveControl() { dir = sendDir });
        }
        else
        {
            card.mb = null;
            SendMsg(live, MsgType.MoveControl, new MsgMoveControl() { dir = Vector3.zero });
        }
    }
}