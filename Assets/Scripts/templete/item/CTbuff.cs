using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTbuff : Cbuffbase_36
{

}
public class DTbuff : DataBase
{

}
public class STbuff : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTbuff card = _card as CTbuff;
        MsgOnItem msg = _msg as MsgOnItem;

        Debug.Log(msg.op);
    }
}