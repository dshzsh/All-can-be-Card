using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMphq_22 : CMpressUseMagic_20
{
    public List<CardBase> jgs = new List<CardBase>();
}
public class DMphq_22 : DataBase
{
    public float interval;
    public float damage;
    public float bulletPow;
    public float fireDamage;
    public float fireTime;
}
public class SMphq_22 : SMpressUseMagic_20
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        //bid = GetTypeId(typeof(CBjg_8));
        bid = GetTypeId(typeof(CBphq_13));
    }
    public override void EndPressUse(CardBase _card, MsgBase _msg)
    {
        CMphq_22 card = _card as CMphq_22;
        foreach (CardBase com in card.jgs)
            RemoveComponent(card, com);
        card.jgs.Clear();
    }
    public override void BeginPressUse(CardBase _card, MsgMagicUse msg)
    {
        CMphq_22 card = _card as CMphq_22;
        DMphq_22 config = basicConfig as DMphq_22;

        CMCjg_14 jg = CreateCard<CMCjg_14>();
        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.bulletPow *= config.bulletPow;
        SFzs_1.AddZs(bmsg, config.fireDamage * Shealth_4.GetAttf(card, BasicAttID.atk) * msg.pow, config.fireTime);

        bmsg.damage = Shealth_4.GetAttf(card, BasicAttID.atk) * config.damage / config.bulletPow;
        jg.interval = config.interval;
        jg.bmsg = bmsg;
        jg.costPerShot = msg.mdata.cost;
        jg.bid = bid;
        card.jgs.Add(jg);
        AddComponent(card, jg);
    }
}