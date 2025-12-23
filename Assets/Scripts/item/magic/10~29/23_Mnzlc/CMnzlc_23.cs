using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMnzlc_23 : Cmagicbase_17
{

}
public class DMnzlc_23 : DataBase
{
    public BasicAtt speedChange;
    public float time;
    public float distance;
}
public class SMnzlc_23 : Smagicbase_17
{
    public static BasicAtt speedChange;
    static int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBnzlc_14));

        DMnzlc_23 config = basicConfig as DMnzlc_23;
        speedChange = config.speedChange;
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMnzlc_23 card = _card as CMnzlc_23;
        DMnzlc_23 config = basicConfig as DMnzlc_23;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.baseScale = config.distance * 2;
        bmsg.initPos = msg.pos;
        Sbullet_10.GiveBullet(bid, bmsg);
    }
}