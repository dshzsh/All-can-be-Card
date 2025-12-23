using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CFksyd_2 : Cbuffbase_36
{
    public BasicAtt speedUp;

}
public class DFksyd_2 : DataBase
{

}
public class SFksyd_2 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFksyd_2 card = _card as CFksyd_2;
        MsgOnItem msg = _msg as MsgOnItem;

        new AttAndRevise(BasicAttID.speed, card.speedUp).UseOnLive(card, msg.op, card.pow);
    }
}