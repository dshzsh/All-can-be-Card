using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMCpqbb_21 : Citem_33
{
    public float force;
    public float interval;
    public float time;
    public float costPerShot;

    public bool ok = true;
}
public class DMCpqbb_21 : DataBase
{
    
}
public class SMCpqbb_21 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMCpqbb_21 card = _card as CMCpqbb_21;
        MsgUpdate msg = _msg as MsgUpdate;

        if (TryGetClive(card, out var cobj) && card.ok)
        {
            Vector3 dir = cobj.moveWantDir * 0.5f + Vector3.up;
            MyPhysics.GiveForce(cobj.obj.rbody, dir.normalized, card.force * dir.magnitude);
            if(card._onTx!=null)
            {
                card._onTx.transform.forward = -(cobj.moveWantDir + Vector3.up).normalized;
            }
        }

        if (MyTool.IntervalTime(card.interval, ref card.time, msg.time))
        {
            if (card.costPerShot > 0)
            {
                MsgCostMana cmsg = new MsgCostMana(card.costPerShot);
                SendMsg(GetTop(card), MsgType.CostMana, cmsg);
                if (cmsg.ok == false) card.ok = false;
                else card.ok = true;
            }
        }
    }
}