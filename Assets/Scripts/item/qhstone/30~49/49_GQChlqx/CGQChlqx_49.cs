using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CGQChlqx_49 : Cbuffbase_36
{
    public DGQhlqx_48 config;
    public Vector3 usePos;
}
public class DGQChlqx_49 : DataBase
{

}
public class SGQChlqx_49 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BulletStart, BulletStart);
    }
    void BulletStart(CardBase _card, MsgBase _msg)
    {
        CGQChlqx_49 card = _card as CGQChlqx_49;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj is not Cbullet_10 cbullet) return;

        // 这个额外效果被销毁
        card.buffInfo.time = 0f;

        for (int i = 0; i < card.config.cnt; i++)
        {
            MsgBullet bmsg = new MsgBullet(cbullet);
            bmsg.MulPow(card.config.bulletPow);
            bmsg.initPos = bmsg.initPos + 2 * card.config.posShift * Random.insideUnitSphere;

            // 寻找距离 usePos 最近的敌人
            Clive_19 target = Slive_19.FindLive(card.usePos, cbullet.team);
            if(CardValid(target))
            {
                bmsg.dir = (target.obj.Center - bmsg.initPos).normalized;
            }

            Sbullet_10.GiveBullet(cbullet.id, bmsg);
        }

        DestroyCard(cbullet);
    }
}