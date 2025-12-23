using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using static MyTag;

public class CBnzlc_14 : Cbullet_10
{
    public List<CObj_2> beBuffLives = new List<CObj_2>();
}

public class SBnzlc_14 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
        AddHandle(MsgType.CollisionExit, CollisionExit, HandlerPriority.After);
    }
    private void GiveBuff(CBnzlc_14 card, MsgCollision msg, int op)
    {
        if (msg.robj == null) return;

        BasicAtt att = SMnzlc_23.speedChange;
        float pow = 1/Sbullet_10.GetBulletPow(card);
        att = att.WithPow(pow);

        msg.robj.velocity = msg.robj.velocity * att.UseAttTo(1, op);

        //todo：改成buff的形式而不是不安全的此形式
        if(msg.other!=null)
        {
            new AttAndRevise(BasicAttID.speed, att).UseOnLive(msg.other, op);
            if (op == 1)
            {
                card.beBuffLives.Add(msg.other);

            }
            else
            {
                card.beBuffLives.Remove(msg.other);

            }
        }
        
    }
    public void OnItem(CardBase _card, MsgBase _msg)
    {
        CBnzlc_14 card = _card as CBnzlc_14;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op == 1)
        {

        }
        else
        {
            List<CObj_2> toRemove = new List<CObj_2>(card.beBuffLives);
            TryGetCobj(card, out CObj_2 myObj);
            foreach (CObj_2 live in toRemove)
            {
                if (!CardValid(live)) continue;
                GiveBuff(card, new MsgCollision(myObj, live, live.obj.gameObject, Vector3.zero, live.obj.rbody), -1);
            }
            card.beBuffLives = null;
        }
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CBnzlc_14 card = _card as CBnzlc_14;
        MsgCollision msg = _msg as MsgCollision;

        GiveBuff(card, msg, 1);
    }
    public void CollisionExit(CardBase _card, MsgBase _msg)
    {
        CBnzlc_14 card = _card as CBnzlc_14;
        MsgCollision msg = _msg as MsgCollision;

        GiveBuff(card, msg, -1);
    }
}