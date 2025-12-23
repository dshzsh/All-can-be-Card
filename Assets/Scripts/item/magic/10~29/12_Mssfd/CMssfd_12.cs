using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMssfd_12 : Cmagicbase_17
{

}
public class DMssfd_12 : DataBase
{
    public float damage;
    public float bulletPow;
    public int ssTimes;
    public float scatterRange;
}
public class SMssfd_12 : Smagicbase_17
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBssfd_7));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMssfd_12 card = _card as CMssfd_12;
        DMssfd_12 config = basicConfig as DMssfd_12;

        for (int i = 0; i < config.ssTimes; i++)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.bulletPow *= config.bulletPow;
            bmsg.damage = config.damage / config.bulletPow * Shealth_4.GetAttf(card, BasicAttID.atk);
            if (TryGetCobj(msg.live, out var cObj))
            {
                Vector3 dir = msg.pos - cObj.obj.Center;
                bmsg.dir = MyTool.RandScatter(dir, config.scatterRange).normalized;
            }
            Sbullet_10.GiveBullet(bid, bmsg);
        }
        
    }
}