using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLFIgrass_1 : CLFfightBase_6
{

}

public class SLFIgrass_1 : SLFfightBase_6
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLFIgrass_1 card = _card as CLFIgrass_1;
        MsgOnItem msg = _msg as MsgOnItem;

        
    }
}
