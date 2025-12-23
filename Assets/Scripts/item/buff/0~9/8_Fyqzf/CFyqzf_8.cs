using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFyqzf_8 : Cbuffbase_36
{
    public BasicAtt atkAdd;
}
public class DFyqzf_8 : DataBase
{

}
public class SFyqzf_8 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFyqzf_8 card = _card as CFyqzf_8;
        MsgOnItem msg = _msg as MsgOnItem;

        new AttAndRevise(BasicAttID.atk, card.atkAdd).UseOnLive(GetTop(card), msg.op, card.pow);
    }
}