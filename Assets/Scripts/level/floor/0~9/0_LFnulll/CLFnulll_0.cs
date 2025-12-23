using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFnulll_0 : CardBase
{

}

public class SLFnulll_0: SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CLFnulll_0 card = _card as CLFnulll_0;
        MsgUpdate msg = _msg as MsgUpdate;

        Debug.Log(msg.time);
    }
}