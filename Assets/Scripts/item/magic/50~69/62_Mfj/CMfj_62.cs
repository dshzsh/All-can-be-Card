using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMfj_62 : Cmagicbase_17
{

}
public class DMfj_62 : DataBase
{
    public float bzDamage, bdRadius, radius, damage, fireTime, fireDamage;
}
public class SMfj_62 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(SFzs_1.mTgiveZsDamageAfter, GiveZsDamageAfter);
    }
    void GiveZsDamageAfter(CardBase _card, MsgBase _msg)
    {
        CMfj_62 card = _card as CMfj_62;
        MsgGiveZsDamageAfter msg = _msg as MsgGiveZsDamageAfter;
        DMfj_62 config = basicConfig as DMfj_62;

        if (!TryGetCobj(msg.zsBuff, out var cobj)) return;
        Smagic_14.UseMagicWithBA(new MsgMagicUse(cobj, card, cobj.obj.Center, bdUse: 1));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMfj_62 card = _card as CMfj_62;
        DMfj_62 config = basicConfig as DMfj_62;

        if(msg.bdUse==0)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
            bmsg.baseScale *= config.radius * 2;

            SFzs_1.AddZs(bmsg, config.fireDamage * msg.pow, config.fireTime);

            Sbullet_10.GiveBullet(SBbz_24.bid, bmsg);
        }
        else if(msg.bdUse==1)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.damage = config.bzDamage * Shealth_4.GetAttf(card, BasicAttID.atk);
            bmsg.baseScale *= config.bdRadius * 2;
            bmsg.initPos = msg.pos;

            Sbullet_10.GiveBullet(SBbz_24.bid, bmsg);
        }
    }
}