using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMbpd_43 : Cmagicbase_17
{

}
public class DMbpd_43 : DataBase
{
    public float exCd;
    public float damage;
    public float delay;
    public float radius;
}
public class SMbpd_43 : Smagicbase_17
{
    public int bid = 0;
    public override void Create(CardBase _card)
    {
        DMbpd_43 config = basicConfig as DMbpd_43;

        base.Create(_card);
        CardBase cnull = new CNull_0();
        CGQCewcc_63 qhs = CreateCard<CGQCewcc_63>();
        qhs.exTimeMax = config.exCd;
        AddComponent(cnull, qhs);
        AddComponent(_card, cnull);
    }
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBbpd_23));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMbpd_43 card = _card as CMbpd_43;
        DMbpd_43 config = basicConfig as DMbpd_43;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.AddInitShift(card);

        var buff = CreateCard<CGQCycbp_57>();
        buff.radius = config.radius;
        bmsg.AddCard(buff);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}