using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CMsmzx_50 : Cmagicbase_17
{
    public float record;
}
public class DMsmzx_50 : DataBase
{
    public float recordRate, damage, heal;
}
public class SMsmzx_50 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeHeal, BeHeal);
        bid = GetTypeId(typeof(CBsmzx_29));
    }
    void BeHeal(CardBase _card, MsgBase _msg)
    {
        CMsmzx_50 card = _card as CMsmzx_50;
        MsgBeHeal msg = _msg as MsgBeHeal;
        DMsmzx_50 config = basicConfig as DMsmzx_50;

        card.record += msg.heal;
        float recordMax = config.recordRate * Shealth_4.GetAttf(card, BasicAttID.healthMax);
        int repeat = (int)(card.record / recordMax);
        card.record = card.record - repeat * recordMax;
        for (int i = 0; i < repeat; i++)
        {
            Vector3 pos = Vector3.zero;
            if (TryGetCobj(msg.to, out var cobj)) pos = cobj.obj.Center;
            pos += MyRandom.UpSphere();
            Smagic_14.UseMagicWithBA(new MsgMagicUse(cobj, card, pos, bdUse: 1, noCost: true));
        }
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMsmzx_50 card = _card as CMsmzx_50;
        DMsmzx_50 config = basicConfig as DMsmzx_50;

        if (msg.bdUse == 0) // 主动效果
        {
            Shealth_4.GiveHeal(msg.live, msg.live,
                new MsgBeHeal(config.heal * Shealth_4.GetAttf(card, BasicAttID.healthMax) * msg.pow));
        }
        else if (msg.bdUse == 1) // 被动效果
        {
            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.healthMax);
            SGgzqhAdd_5.AddGz(bmsg);
            Sbullet_10.GiveBullet(bid, bmsg);
        }
    }
}