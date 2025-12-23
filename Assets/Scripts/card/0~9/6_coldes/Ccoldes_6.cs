using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using static Scoldes_6;

public class Ccoldes_6 : CardBase
{
    public ColDesType type = 0;
    public bool needDes = false;
}

public class Scoldes_6 : SystemBase
{
    public static int layer_bullet = LayerMask.NameToLayer("bullet");
    public enum ColDesType
    {
        None = 0,
        NotFriend = 1, // 碰到任意物体就销毁(除了layer为bullet的物体)，可以碰撞普通的物体
        Enemy = 2,// 仅碰撞怪物，可穿墙
    }
    public override void Init()
    {
        AddHandle(MsgType.Collision, CollisionJudge);
        AddHandle(MsgType.Collision, CollisionDes, HandlerPriority.Lowest);
    }
    void CollisionDes(CardBase _card, MsgBase _msg)
    {
        Ccoldes_6 card = _card as Ccoldes_6;
        if (card.needDes)
        {
            SendMsg(card.parent, MsgType.TrueDie, new MsgDie());
        }
    }
    void CollisionJudge(CardBase _card, MsgBase _msg)
    {
        Ccoldes_6 card = _card as Ccoldes_6;
        MsgCollision msg = _msg as MsgCollision;

        if (msg.obj.layer == layer_bullet)
            return;

        switch (card.type)
        {
            case ColDesType.NotFriend:
                {
                    if (GetTeam(msg.other) == GetTeam(card))
                    {
                        break;
                    }
                    card.needDes = true;
                    msg.hit = true;
                    break;
                }
            case ColDesType.Enemy:
                {
                    int team = GetTeam(msg.other);
                    if (team != -1 && team != GetTeam(card))
                    {
                        card.needDes = true;
                    }
                    break;
                }
        }
        
        //DestroyCard(cardAbandon);
    }
}