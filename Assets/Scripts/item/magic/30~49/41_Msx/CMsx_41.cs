using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMsx_41 : Cmagicbase_17
{

}
public class DMsx_41 : DataBase
{
    public float suckBlood;
}
public class SMsx_41 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageAfter, GiveDamageAfter);
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CMsx_41 card = _card as CMsx_41;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DMsx_41 config = basicConfig as DMsx_41;

        float suckBlood = config.suckBlood * card.pow *
            MyMath.LinerMap(0, Shealth_4.GetAttf(card, BasicAttID.healthMax), 2, 1, Shealth_4.GetNowHealth(card));
        Shealth_4.GiveHeal(msg.from, msg.from, new MsgBeHeal(suckBlood * msg.damage));
    }
}