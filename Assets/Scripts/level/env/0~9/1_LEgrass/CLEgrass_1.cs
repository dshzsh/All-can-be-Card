using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLEgrass_1 : CLEenvBase_2
{

}
public class DLEgrass_1 : DataBase
{

}
public class SLEgrass_1 : SLEenvBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLEgrass_1 card = _card as CLEgrass_1;
        MsgOnItem msg = _msg as MsgOnItem;
        DLEgrass_1 config = basicConfig as DLEgrass_1;

    }
}