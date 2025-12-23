using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMlj_5 : Cmagicbase_17
{

}
public class DMlj_5 : DataBase
{
    public float damage;
}
public class SMlj_5 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMlj_5 card = _card as CMlj_5;
        DMlj_5 config = basicConfig as DMlj_5;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.initPos = msg.pos;
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        Sbullet_10.GiveBullet(SBlj_2.bid, bmsg);
    }
}