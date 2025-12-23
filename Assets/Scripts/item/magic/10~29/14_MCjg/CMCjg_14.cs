using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMCjg_14 : CardBase
{
    public MsgBullet bmsg;
    public Vector3 initLiveDir;//初始施法时的方向，用于发射时判定dir的该旋转的方向
    public Vector3 initBulletDir;
    public float interval;
    public float time;
    public float costPerShot;
    public int bid;
}
public class SMCjg_14 : SystemBase
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CMCjg_14 card = _card as CMCjg_14;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            card.initBulletDir = card.bmsg.dir;
            if (TryGetCobj(card, out var cobj))
                card.initLiveDir = (SShoulderCamera_37.GetLookPos(card) - cobj.obj.Center).normalized;
        }
        else
        {

        }
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMCjg_14 card = _card as CMCjg_14;
        MsgUpdate msg = _msg as MsgUpdate;

        if (MyTool.IntervalTime(card.interval, ref card.time, msg.time))
        {
            //更新位置和初始dir
            if(card.costPerShot>0)
            {
                MsgCostMana cmsg = new MsgCostMana(card.costPerShot);
                SendMsg(GetTop(card), MsgType.CostMana, cmsg);
                if (cmsg.ok == false) return;
            }
            

            if (TryGetCobj(card, out var cobj))
            {
                Vector3 dir = (SShoulderCamera_37.GetLookPos(card) - cobj.obj.Center).normalized;
                card.bmsg.dir = Quaternion.FromToRotation(card.initLiveDir, dir) * card.initBulletDir;
                card.bmsg.initPos = cobj.obj.Center + card.bmsg.dir;
            }

            Sbullet_10.GiveBullet(card.bid, new MsgBullet(card.bmsg));
        }
    }
    
}