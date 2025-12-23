using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMyzz_46 : Cmagicbase_17
{

}
public class DMyzz_46 : DataBase
{
    public float cdRecover;
    public float distance;
    public float moveTime;
    public float damage;
}
public class SMyzz_46 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CByzz_27));
        AddHandle(MsgType.UseMagicBefore, UseMagicBefore);
    }
    void UseMagicBefore(CardBase _card, MsgBase _msg)
    {
        MsgMagicUse msg = _msg as MsgMagicUse;

        if (msg.magic.id == id) return;

        CardBase buff = ScolAddBuff_43.MakeCard<CFyzzbj_18>(msg.live, Sbuff_35.BuffSpeTime.ToFightEnd, Sbuff_35.BeBuffMode.coverByBig);
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMyzz_46 card = _card as CMyzz_46;
        DMyzz_46 config = basicConfig as DMyzz_46;

        Vector3 dir = SMcc_10.GetSprintDir(card, msg.pos);

        SMcc_10.GiveSprint(card, config.distance * msg.pow, config.moveTime, dir, true);

        MsgBullet bmsg = new MsgBullet(msg, true);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        SMcz_45.GiveCollisonBullet(card, bmsg, config.moveTime);

        CGQCyzz_62 buff = CreateCard<CGQCyzz_62>();
        buff.cdRecover = config.cdRecover;
        buff.key = msg.costKey;
        bmsg.AddCard(buff);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}