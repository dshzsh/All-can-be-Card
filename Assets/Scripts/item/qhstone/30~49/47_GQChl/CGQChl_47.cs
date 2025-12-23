using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQChl_47 : Cbuffbase_36
{
    public DGQhl_46 config;
}
public class DGQChl_47 : DataBase
{

}
public class SGQChl_47 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BulletStart, BulletStart);
    }
    void BulletStart(CardBase _card, MsgBase _msg)
    {
        CGQChl_47 card = _card as CGQChl_47;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj is not Cbullet_10 cbullet) return;

        // 这个额外效果被销毁
        card.buffInfo.time = 0f;
        float size = Sbullet_10.GetBulletSize(cobj);

        for (int i = 0; i < card.config.cnt; i++)
        {
            MsgBullet bmsg = new MsgBullet(cbullet);
            bmsg.MulPow(card.config.bulletPow);
            bmsg.dir = MyTool.RandScatter(bmsg.dir, card.config.dirShift);
            bmsg.initPos = bmsg.initPos + size * card.config.posShift * Random.insideUnitSphere;
            Sbullet_10.GiveBullet(cbullet.id, bmsg);
        }

        DestroyCard(cbullet);
    }
}