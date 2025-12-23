using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CFwfyd_15 : Cbuffbase_36
{

}
public class DFwfyd_15 : DataBase
{
    public BasicAtt speedAdd;
}
public class SFwfyd_15 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFwfyd_15 card = _card as CFwfyd_15;
        MsgOnItem msg = _msg as MsgOnItem;
        DFwfyd_15 config = basicConfig as DFwfyd_15;

        new AttAndRevise(BasicAttID.speed, config.speedAdd).UseOnLive(card, msg.op);
        if(TryGetClive(card ,out var clive))
        {
            clive.rotateSpeed.BeOnAtt(config.speedAdd, msg.op);
        }
    }
}