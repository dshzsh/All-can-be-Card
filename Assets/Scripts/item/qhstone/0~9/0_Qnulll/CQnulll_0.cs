using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CQnulll_0 : CGqhsbase_11
{

}
public class DQnulll_0 : DataBase
{

}
public class SQnulll_0 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CQnulll_0 card = _card as CQnulll_0;
        MsgOnItem msg = _msg as MsgOnItem;

        Debug.Log(msg.op);
    }
}