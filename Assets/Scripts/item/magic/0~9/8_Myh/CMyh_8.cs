using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMyh_8 : Cmagicbase_17
{

}
public class DMyh_8 : DataBase
{
    public float damage;
    public float damageExplosion;
    public float fireDamage;
    public float fireTime;
}
public class SMyh_8 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMyh_8 card = _card as CMyh_8;
        DMyh_8 config = basicConfig as DMyh_8;

        MsgBullet bmsg = new MsgBullet(msg);
        float zsDam = msg.pow * config.fireDamage * Shealth_4.GetAttf(card, BasicAttID.atk);
        SFzs_1.AddZs(bmsg, zsDam, config.fireTime);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        CGQbz_51 buff = CreateCard<CGQbz_51>();
        buff.explosinMsg = new MsgBullet(msg);
        SFzs_1.AddZs(buff.explosinMsg, zsDam, config.fireTime);
        buff.explosinMsg.damage = config.damageExplosion * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.AddCard(buff);
        Sbullet_10.GiveBullet(GetTypeId(typeof(CByh_4)), bmsg);
    }
}