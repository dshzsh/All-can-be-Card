using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMfsly_57 : Cmagicbase_17
{

}
public class DMfsly_57 : DataBase
{
    public DbasicAtt.AttAndReviseData attData;

    public AttAndRevise att;
    public float heal, time, radius;
    public float interval;
    public override void Init(int id)
    {
        base.Init(id);
        att = attData.ToRevise();
    }
}
public class SMfsly_57 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBfsly_32));
    }
    
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMfsly_57 card = _card as CMfsly_57;
        DMfsly_57 config = basicConfig as DMfsly_57;

        MsgBullet bmsg = new MsgBullet(msg, true);

        CFfsly_26 buff = CreateCard<CFfsly_26>();// 这里不要写上和pow有关的内容，因为本身会受bulletPow影响
        buff.att = config.att;
        buff.heal = config.heal * Shealth_4.GetAttf(msg.live, BasicAttID.healthMax);
        buff.interval = config.interval;
        bmsg.AddCard(buff);

        bmsg.baseScale *= config.radius * 2;
        bmsg.time = config.time;
        if (TryGetCobj(msg.live, out var cobj)) // 直接生成在脚下
            bmsg.initPos = cobj.obj.transform.position;

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}