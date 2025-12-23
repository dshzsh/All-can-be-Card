using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFqx_13 : Cbuffbase_36
{

}
public class DFqx_13 : DataBase
{

}
public class SFqx_13 : Sbuffbase_36
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        AddHandle(SLFfightBase_6.mTFightEnd, FightEnd);
        bid = id;
    }
    void FightEnd(CardBase _card, MsgBase _msg)
    {
        CFqx_13 card = _card as CFqx_13;
        
        card.buffInfo.time = 0;
    }
}