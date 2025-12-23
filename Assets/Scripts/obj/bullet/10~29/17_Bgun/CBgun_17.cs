using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBgun_17 : Cbullet_10
{

}

public class SBgun_17 : Sbullet_10
{
    public static int bid = 0;
    public static float gunBulletSpeed = 0;
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
        bid = id;
        Dbullet_10 dbullet_10 = DataManager.GetConfig<Dbullet_10>(id);
        gunBulletSpeed = dbullet_10.bulletSpeed;
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CBgun_17 card = _card as CBgun_17;
        MsgCollision msg = _msg as MsgCollision;

    }
}