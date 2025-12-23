using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQbz_51 : CGqhsbase_11
{
    public MsgBullet explosinMsg;
}
public class DGQbz_51 : DataBase
{

}
public class SGQbz_51 : SGqhsbase_11
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.TrueDie, TrueDie, HandlerPriority.Before);
        bid = GetTypeId(typeof(CByhExplosion_5));
    }
    public void TrueDie(CardBase _card, MsgBase _msg)
    {
        CGQbz_51 card = _card as CGQbz_51;
        if (!TryGetCobj(card, out var obj))
            return;
        card.explosinMsg.initPos = obj.obj.Center;
        Sbullet_10.GiveBullet(bid, card.explosinMsg);
    }
}