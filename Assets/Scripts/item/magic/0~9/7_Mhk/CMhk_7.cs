using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMhk_7 : Cmagicbase_17
{
    public int lj = 1;
}
public class DMhk_7 : DataBase
{
    public float damage;
    public float rotAngle;
    public float size;
}
public class SMhk_7 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBxj_15));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMhk_7 card = _card as CMhk_7;
        DMhk_7 config = basicConfig as DMhk_7;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.isMelee = true;

        bmsg.AddRotate(Quaternion.Euler(0, -config.rotAngle/2 * card.lj, 0));
        bmsg.baseScale *= config.size;

        CGQCxj_43 xj = CreateCard<CGQCxj_43>();
        xj.rotAngle = card.lj * config.rotAngle;
        card.lj = -card.lj;
        bmsg.addCards.Add(xj);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}