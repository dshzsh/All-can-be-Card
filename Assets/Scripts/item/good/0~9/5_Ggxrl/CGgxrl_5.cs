using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGgxrl_5 : Citem_33
{

}
public class DGgxrl_5 : DataBase
{
    public float speedUp;
}
public class SGgxrl_5 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveBuff, GiveBuff);
    }
    void GiveBuff(CardBase _card, MsgBase _msg)
    {
        CGgxrl_5 card = _card as CGgxrl_5;
        MsgBeBuff msg = _msg as MsgBeBuff;
        DGgxrl_5 config = basicConfig as DGgxrl_5;

        if (msg.buff is CFzs_1 zsBuff)
        {
            zsBuff.speedUp += config.speedUp;
        }
    }
}