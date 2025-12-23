using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMzrsq_59 : Cmagicbase_17
{
    public float time = 0;
}
public class DMzrsq_59 : DataBase
{
    public float damage, interval, fireTime, fireDamage, radius;
}
public class SMzrsq_59 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        bid = GetTypeId(typeof(CBzrsq_33));
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMzrsq_59 card = _card as CMzrsq_59;
        MsgUpdate msg = _msg as MsgUpdate;
        DMzrsq_59 config = basicConfig as DMzrsq_59;

        if(MyTool.IntervalTime(config.interval, ref card.time, msg.time))
        {
            Smagic_14.UseMagicWithBA(new MsgMagicUse(msg.cobj, card, msg.cobj.obj.Center + msg.cobj.obj.transform.forward
                , bdUse: 1));
        }
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMzrsq_59 card = _card as CMzrsq_59;
        DMzrsq_59 config = basicConfig as DMzrsq_59;

        if(msg.bdUse == 1)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.damage = config.damage * Shealth_4.GetAttf(msg.live, BasicAttID.healthMax);
            bmsg.baseScale *= config.radius * 2;
            SFzs_1.AddZs(bmsg, config.fireDamage * Shealth_4.GetAttf(msg.live, BasicAttID.healthMax) * msg.pow
                , config.fireTime);
            bmsg.id = bid;
            Sbullet_10.GiveBullet(bmsg);
        }
    }
}