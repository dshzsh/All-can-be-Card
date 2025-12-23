using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBnulll_0 : Cbullet_10
{

}

public class SBnulll_0 : Sbullet_10
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    public void Update(CardBase _card, MsgBase _msg)
    {
        CBnulll_0 card = _card as CBnulll_0;
        MsgUpdate msg = _msg as MsgUpdate;

        Debug.Log(msg.time);
    }
}