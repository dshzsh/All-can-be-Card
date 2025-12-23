using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTmapObj : CObj_2
{

}
public class DTmapObj : DataBase
{

}
public class STmapObj : SObj_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CTmapObj card = _card as CTmapObj;
        MsgCollision msg = _msg as MsgCollision;

    }
}