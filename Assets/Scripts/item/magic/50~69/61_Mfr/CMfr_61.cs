using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMfr_61 : Cmagicbase_17
{

}
public class DMfr_61 : DataBase
{
    public float damage, zsPow;
    public float rotAngle;
    public float upAngle;
    public float fireTime, fireDamage;
}
public class SMfr_61 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBfr_34));
    }
    
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMfr_61 card = _card as CMfr_61;
        DMfr_61 config = basicConfig as DMfr_61;

        for (int lj = -1; lj <= 1; lj += 2)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
            bmsg.isMelee = true;

            SFzs_1.AddZs(bmsg, config.fireDamage * msg.pow, config.fireTime);

            CGQCfr_72 buff = CreateCard<CGQCfr_72>();
            buff.zsPow = config.zsPow;
            bmsg.AddCard(buff);

            bmsg.AddRotate(Quaternion.Euler(0, 0, -config.upAngle * lj));
            bmsg.AddRotate(Quaternion.Euler(0, -config.rotAngle / 2 * lj, 0));

            CGQCxj_43 xj = CreateCard<CGQCxj_43>();
            xj.rotAngle = lj * config.rotAngle;
            bmsg.addCards.Add(xj);

            Sbullet_10.GiveBullet(bid, bmsg);
        }

        
    }
}