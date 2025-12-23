using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTcard : CardBase
{

}

public class STcard: SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CTcard card = _card as CTcard;
        MsgUpdate msg = _msg as MsgUpdate;

        Debug.Log(msg.time);
    }
}