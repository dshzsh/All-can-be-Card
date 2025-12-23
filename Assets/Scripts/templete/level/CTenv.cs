using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTenv : CLEenvBase_2
{

}
public class DTenv : DataBase
{

}
public class STenv : SLEenvBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTenv card = _card as CTenv;
        MsgOnItem msg = _msg as MsgOnItem;
        DTenv config = basicConfig as DTenv;

        Debug.Log(msg.op);
    }
}