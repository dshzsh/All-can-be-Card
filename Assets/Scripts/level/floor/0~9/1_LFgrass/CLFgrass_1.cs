using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFgrass_1 : CfloorBase_2
{

}

public class SLFgrass_1: SfloorBase_2
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CLFgrass_1 card = _card as CLFgrass_1;
        MsgUpdate msg = _msg as MsgUpdate;

        card.obj.transform.position += Vector3.up * msg.time;
    }
}