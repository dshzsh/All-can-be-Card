using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGylx_56 : Citem_33
{

}
public class DGylx_56 : DataBase
{
    public float exTime;
}
public class SGylx_56 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveBuff, GiveBuff);
    }
    void GiveBuff(CardBase _card, MsgBase _msg)
    {
        CGylx_56 card = _card as CGylx_56;
        MsgBeBuff msg = _msg as MsgBeBuff;
        DGylx_56 config = basicConfig as DGylx_56;

        if (msg.IsSpeTime()) return;

        msg.time *= 1 + config.exTime * card.pow;
    }
}