using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMshot_1 : Cmagicbase_17
{

}
public class DMshot_1 : DataBase
{
    public float damage;
}
public class SMshot_1 : Smagicbase_17
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(Cbullet_10));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMshot_1 card = _card as CMshot_1;
        DMshot_1 config = basicConfig as DMshot_1;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        Sbullet_10.GiveBullet(bid, bmsg);
    }
}