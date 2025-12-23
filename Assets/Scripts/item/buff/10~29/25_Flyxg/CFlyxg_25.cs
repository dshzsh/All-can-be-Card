using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CFlyxg_25 : Cbuffbase_36
{
    public float interval = 1f;
    public float time = 0f;

    public List<CObj_2> cobjStays = new List<CObj_2>();
}
public class DFlyxg_25 : DataBase
{

}
public class SFlyxg_25 : Sbuffbase_36
{
    public const float buffIntervalScale = 1.01f;
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, FieldOnItem);
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.Collision, FieldCollision, HandlerPriority.After);
        AddHandle(MsgType.CollisionExit, FieldCollisionExit, HandlerPriority.After);
    }

    public virtual void EnterField(CardBase _card, float bulletPow, CObj_2 obj, int op, MsgCollision msg)
    {

    }
    public virtual void UpdateField(CardBase _card, float bulletPow, CObj_2 obj, MsgUpdate msg)
    {

    }
    private void Update(CardBase _card, MsgBase _msg)
    {
        CFlyxg_25 card = _card as CFlyxg_25;
        MsgUpdate msg = _msg as MsgUpdate;

        if (!MyTool.IntervalTime(card.interval, ref card.time, msg.time)) return;

        float bulletPow = Sbullet_10.GetBulletPow(card);
        for (int i = 0; i < card.cobjStays.Count; i++)
        {
            if (!CardValid(card.cobjStays[i]))
            {
                card.cobjStays.RemoveAt(i);
                i--;
                continue;
            }
            UpdateField(_card, bulletPow, card.cobjStays[i], msg);
        }
    }
    void FieldOnItem(CardBase _card, MsgBase _msg)
    {
        CFlyxg_25 card = _card as CFlyxg_25;
        MsgOnItem msg = _msg as MsgOnItem;

        if (msg.op == 1)
        {

        }
        else
        {
            List<CObj_2> toRemove = new List<CObj_2>(card.cobjStays);
            float bulletPow = Sbullet_10.GetBulletPow(card);
            TryGetCobj(card, out CObj_2 myObj);
            foreach (CObj_2 cobj in toRemove)
            {
                if (!CardValid(cobj)) continue;
                EnterField(card, bulletPow, cobj, -1, new MsgCollision(myObj, cobj, cobj.obj.gameObject, Vector3.zero, cobj.obj.rbody));
            }
            card.cobjStays = null;
        }
    }
    private void FieldCollision(CardBase _card, MsgBase _msg)
    {
        CFlyxg_25 card = _card as CFlyxg_25;
        MsgCollision msg = _msg as MsgCollision;

        if (msg.other == null) return;

        card.cobjStays.Add(msg.other);
        EnterField(card, Sbullet_10.GetBulletPow(card), msg.other, 1, msg);
    }
    private void FieldCollisionExit(CardBase _card, MsgBase _msg)
    {
        CFlyxg_25 card = _card as CFlyxg_25;
        MsgCollision msg = _msg as MsgCollision;

        if (msg.other == null) return;

        card.cobjStays.Remove(msg.other);
        EnterField(card, Sbullet_10.GetBulletPow(card), msg.other, -1, msg);
    }
}