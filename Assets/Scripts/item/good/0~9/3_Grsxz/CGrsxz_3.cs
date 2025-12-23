using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGrsxz_3 : Citem_33
{
    
}
public class DGrsxz_3 : DataBase
{
    public float heal;
}
public class SGrsxz_3 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(SFzs_1.mTgiveZsDamageAfter, GiveZsDamageAfter);
    }
    void GiveZsDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGrsxz_3 card = _card as CGrsxz_3;
        MsgGiveZsDamageAfter msg = _msg as MsgGiveZsDamageAfter;
        DGrsxz_3 config = basicConfig as DGrsxz_3;

        Shealth_4.GiveHeal(GetTop(card), GetTop(card),
            new MsgBeHeal(config.heal * Shealth_4.GetAttf(card, BasicAttID.healthMax) * card.pow));
    }
}