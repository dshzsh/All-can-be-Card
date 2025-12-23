using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFmustMove_4 : Cbuffbase_36
{
    public Vector3 mustMoveVelocity;
}
public class SFmustMove_4 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFmustMove_4 card = _card as CFmustMove_4;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetClive(card, out var clive)) return;

        clive.mustMoveVelocity += card.mustMoveVelocity * msg.op;
    }
}