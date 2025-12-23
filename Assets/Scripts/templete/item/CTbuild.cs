using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTbuild : CGBbase_1
{

}
public class DTbuild : DataBase
{

}
public class STbuild : SGBbase_1
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTbuild card = _card as CTbuild;
        MsgOnItem msg = _msg as MsgOnItem;
        DTbuild config = basicConfig as DTbuild;

        Debug.Log(msg.op);
    }
}