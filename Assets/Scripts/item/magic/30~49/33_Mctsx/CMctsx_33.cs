using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMctsx_33 : Cmagicbase_17
{

}
public class DMctsx_33 : DataBase
{
    public float damage;
    public int cnt;
}
public class SMctsx_33 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMctsx_33 card = _card as CMctsx_33;
        DMctsx_33 config = basicConfig as DMctsx_33;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = Shealth_4.GetAttf(card, BasicAttID.atk) * config.damage;
        for (int i = 0; i < config.cnt; i++)
        {
            Sbullet_10.GiveBullet(SBjg_8.bid, bmsg);
        }
    }
}