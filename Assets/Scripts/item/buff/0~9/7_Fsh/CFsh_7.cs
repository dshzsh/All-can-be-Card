using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFsh_7 : Cbuffbase_36
{

}
public class DFsh_7 : DataBase
{

}
public class SFsh_7 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MoveControl, MoveControl, HandlerPriority.Highest);
    }
    
    void MoveControl(CardBase _card, MsgBase _msg)
    {
        _msg.valid = false;
    }
}