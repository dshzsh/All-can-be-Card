using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SGAluck_13;
using static SystemManager;

public class CGblyb_54 : Citem_33
{

}
public class DGblyb_54 : DataBase
{
    public float odds;
}
public class SGblyb_54 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTLuckedOdds, LuckedOdds);
    }
    void LuckedOdds(CardBase _card, MsgBase _msg)
    {
        CGblyb_54 card = _card as CGblyb_54;
        MsgLuckedOdds msg = _msg as MsgLuckedOdds;
        DGblyb_54 config = basicConfig as DGblyb_54;

        msg.luckedOdds = config.odds;
    }
}