using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cjump_11 : CardBase
{
    public float force = 5f;
}

public class Sjump_11 : SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Jump, Jump);
    }
    void Jump(CardBase _card, MsgBase _msg)
    {
        Cjump_11 card = _card as Cjump_11;

        if (TryGetCobj(card, out var cobj) && cobj.obj.rbody != null)
        {
            cobj.obj.rbody.AddForce(card.force * Vector3.up, ForceMode.Impulse);
        }
    }
}