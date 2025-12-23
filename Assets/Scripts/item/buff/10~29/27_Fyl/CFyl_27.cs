using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFyl_27 : Cbuffbase_36
{
    public BasicAtt usePowAdd;
}
public class DFyl_27 : DataBase
{

}
public class SFyl_27 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.UseMagicBefore, UseMagicBefore);
    }
    void UseMagicBefore(CardBase _card, MsgBase _msg)
    {
        CFyl_27 card = _card as CFyl_27;
        MsgMagicUse msg = _msg as MsgMagicUse;

        msg.pow = card.usePowAdd.WithPow(card.pow).UseAttTo(msg.pow, 1);
    }
}