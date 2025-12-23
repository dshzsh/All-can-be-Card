using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMxj_26 : Cmagicbase_17
{
    public int lj = 1;
}
public class DMxj_26 : DataBase
{
    public float damage;
    public float rotAngle;
    public float upAngle;
}
public class SMxj_26 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBxj_15));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMxj_26 card = _card as CMxj_26;
        DMxj_26 config = basicConfig as DMxj_26;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.isMelee = true;

        bmsg.AddRotate(Quaternion.Euler(0, 0, -config.upAngle * card.lj));
        bmsg.AddRotate(Quaternion.Euler(0, -config.rotAngle/2 * card.lj, 0));

        CGQCxj_43 xj = CreateCard<CGQCxj_43>();
        xj.rotAngle = card.lj * config.rotAngle;
        card.lj = -card.lj;
        bmsg.addCards.Add(xj);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}