using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CCylyd_56 : Cbuffbase_36
{
    public float time = 0;
    public float timeMax = 1;
    public BasicAtt sizeAdd, powAdd;
    public BasicAtt accPowAdd = new();
    public BasicAtt initSizeAdd = new();
}
public class DCylyd_56 : DataBase
{

}
public class SCylyd_56 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.BulletStart, BulletStart);
    }
    void BulletStart(CardBase _card, MsgBase _msg)
    {
        CCylyd_56 card = _card as CCylyd_56;
        MsgBulletCreate msg = _msg as MsgBulletCreate;
        
        if (TryGetCobj(card, out var cobj) && cobj is Cbullet_10 cbullet)
        {
            cbullet.bulletPow = card.accPowAdd.UseAttTo(cbullet.bulletPow, 1);
            card.timeMax = cbullet.timeDes.timeMax;
            card.time = 0;
        }
        new AttAndRevise(BasicAttID.size, card.initSizeAdd).UseOnLive(card, 1);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CCylyd_56 card = _card as CCylyd_56;
        MsgUpdate msg = _msg as MsgUpdate;

        if (card.time >= card.timeMax) return;

        card.time += msg.time;
        float dtime = msg.time;
        if (card.time > card.timeMax) dtime -= card.time - card.timeMax;// 多出来的那一点点时间刨去

        float pow = card.pow * dtime / card.timeMax;

        if (TryGetCobj(card, out var cobj) && cobj is Cbullet_10 cbullet)
        {
            cbullet.bulletPow = card.accPowAdd.UseAttTo(cbullet.bulletPow, -1);
            card.accPowAdd = card.powAdd.WithPow(pow).UseAttTo(card.accPowAdd, 1);
            cbullet.bulletPow = card.accPowAdd.UseAttTo(cbullet.bulletPow, 1);
        }

        new AttAndRevise(BasicAttID.size, card.sizeAdd).UseOnLive(card, 1, pow);
    }
}