using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTrule : CGRbase_1
{

}
public class DTrule : DataBase
{

}
public class STrule : SGRbase_1
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTrule card = _card as CTrule;
        MsgOnItem msg = _msg as MsgOnItem;
        DTrule config = basicConfig as DTrule;

        Debug.Log(msg.op);
    }
}