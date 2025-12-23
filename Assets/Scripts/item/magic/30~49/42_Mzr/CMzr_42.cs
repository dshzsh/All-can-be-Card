using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMzr_42 : Cmagicbase_17
{
    public int lj = 1;
}
public class DMzr_42 : DataBase
{
    public float damage;
    public float exBulletPow;

    public float rotAngle;
    public float upAngle;
    public float size;
}
public class SMzr_42 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBxj_15));
    }
    
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMzr_42 card = _card as CMzr_42;
        DMzr_42 config = basicConfig as DMzr_42;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.bulletPow *= config.exBulletPow;
        bmsg.damage = config.damage/config.exBulletPow * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.isMelee = true;
        bmsg.baseScale *= config.size;

        bmsg.AddRotate(Quaternion.Euler(0, 0, -config.upAngle * card.lj));
        bmsg.AddRotate(Quaternion.Euler(0, -config.rotAngle / 2 * card.lj, 0));

        CGQCxj_43 xj = CreateCard<CGQCxj_43>();
        xj.rotAngle = card.lj * config.rotAngle;
        card.lj = -card.lj;
        bmsg.addCards.Add(xj);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}