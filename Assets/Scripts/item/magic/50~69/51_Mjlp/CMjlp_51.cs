using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMjlp_51 : Cmagicbase_17
{
    public int lj = 1;
}
public class DMjlp_51 : DataBase
{
    public float damage, exDamage, delay;
    public float forwardDis, cornerDis, angle;
}
public class SMjlp_51 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBhz_30));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMjlp_51 card = _card as CMjlp_51;
        DMjlp_51 config = basicConfig as DMjlp_51;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.isMelee = true;

        // 横斩
        // A+B=180
        float angle = config.angle;
        if (card.lj == -1) angle = 180 - angle;
        (bmsg.initPos, bmsg.dir) = MyMath.CalculateViewport(bmsg.initPos, bmsg.dir
            , config.forwardDis, config.cornerDis, angle);
        card.lj = -card.lj;

        // 添加落雷
        CFjlp_21 buff = CreateCard<CFjlp_21>();
        buff.damageRate = config.exDamage / config.damage;
        buff.delay = config.delay;
        bmsg.AddCard(buff);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}