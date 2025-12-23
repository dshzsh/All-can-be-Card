using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

//蓄力魔法实质：
//激光：蓄力松开后断开魔法，后摇无穷大
//蓄力加强：前摇大，蓄力松开后立刻释放魔法进入后摇
//除非你能蓄力1000s，不然就不会有问题！
public class CMjg_13 : CMpressUseMagic_20
{
    public List<CardBase> jgs = new List<CardBase>();
}
public class DMjg_13 : DataBase
{
    public float interval;
    public float damage;
    public float bulletPow;    
}
public class SMjg_13 : SMpressUseMagic_20
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBjg_8));
    }
    public override void EndPressUse(CardBase _card, MsgBase _msg)
    {
        CMjg_13 card = _card as CMjg_13;
        foreach (CardBase com in card.jgs)
            RemoveComponent(card, com);
        card.jgs.Clear();
    }
    public override void BeginPressUse(CardBase _card, MsgMagicUse msg)
    {
        CMjg_13 card = _card as CMjg_13;
        DMjg_13 config = basicConfig as DMjg_13;

        CMCjg_14 jg = CreateCard<CMCjg_14>();
        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = Shealth_4.GetAttf(card, BasicAttID.atk) * config.damage / config.bulletPow;
        bmsg.bulletPow *= config.bulletPow;
        jg.interval = config.interval;
        jg.bmsg = bmsg;
        jg.costPerShot = msg.mdata.cost;
        jg.bid = bid;
        card.jgs.Add(jg);
        AddComponent(card, jg);
    }
}