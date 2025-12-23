using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMshy_64 : Cmagicbase_17
{
    public int lj = 0;

    [NoActiveCard]
    public List<CObj_2> shys = new();
}
public class DMshy_64 : DataBase
{
    public float damage;
}
public class SMshy_64 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagicAfter, MyUseMagicAfter);
    }
    void MyUseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CMshy_64 card = _card as CMshy_64;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DMshy_64 config = basicConfig as DMshy_64;

        if (card.lj == 0)
        {
            Smagic_14.RecoverMagicCd(msg.magic, time: 1, isPercent: true);
        }

        card.lj = 1 - card.lj; // 切换技能效果
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMshy_64 card = _card as CMshy_64;
        DMshy_64 config = basicConfig as DMshy_64;

        if (card.lj == 0)
            UseMagic0(card, msg, config);
        else
            UseMagic1(card, msg, config);        
    }
    private void UseMagic0(CMshy_64 card, MsgMagicUse msg, DMshy_64 config)
    {
        MsgBullet bmsg = new MsgBullet(msg, true);
        bmsg.damage = config.damage * Shealth_4.GetAttf(msg.live, BasicAttID.atk);
        bmsg.initPos = msg.live.obj.transform.position;

        CGQCshy_75 buff = CreateCard<CGQCshy_75>();
        buff.fromMagic = card;
        bmsg.AddCard(buff);

        SBcy_35.GiveCyBullet(msg.live, bmsg);
    }
    private void UseMagic1(CMshy_64 card, MsgMagicUse msg, DMshy_64 config)
    {
        CObj_2 mb = null;
        for(int i=0;i<card.shys.Count;i++)
        {
            if (!CardValid(card.shys[i]))
            {
                card.shys.RemoveAt(i);
                i--;
                continue;
            }
            if(mb==null)
            {
                mb = card.shys[i];
                continue;
            }
            if (Slive_19.GetDistance(msg.live, mb) > Slive_19.GetDistance(msg.live, card.shys[i]))
                mb = card.shys[i];
        }
        if (mb == null)
        {
            card.lj = 0;
            UseMagic0(card, msg, config);
            return;
        }
        
        msg.live.obj.transform.position = mb.obj.transform.position;
        msg.live.obj.transform.forward = MyMath.V3zeroYNor(mb.obj.transform.forward);
        SShoulderCamera_37.RefreshView(card);

        Sbullet_10.DieBullet(mb);
        card.shys.Remove(mb);
    }
}