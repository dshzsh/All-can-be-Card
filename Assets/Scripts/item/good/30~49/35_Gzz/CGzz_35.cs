using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGzz_35 : Citem_33
{

}
public class DGzz_35 : DataBase
{
    public float lestCd;
    public float stunTime;
}
public class SGzz_35 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.UseMagicAfter, UseMagicAfter);
    }
    void UseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGzz_35 card = _card as CGzz_35;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGzz_35 config = basicConfig as DGzz_35;

        if (msg.mdata.cd < config.lestCd) return;

        Sbuff_35.GiveBuff(GetTop(card), GetTop(card), new MsgBeBuff(CreateCard<CFcm_14>(), 
            config.stunTime * card.pow, 0, Sbuff_35.BeBuffMode.coverByBig));
    }
}