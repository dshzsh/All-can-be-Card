using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQhzl_35 : CGqhsbase_11
{

}
public class DGQhzl_35 : DataBase
{
    public float force;
}
public class SGQhzl_35 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagicAfter, MyUseMagicAfter);
    }
    void MyUseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGQhzl_35 card = _card as CGQhzl_35;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQhzl_35 config = basicConfig as DGQhzl_35;

        if (!TryGetCobj(card, out var cobj, true)) return;

        Vector3 dir = -(msg.pos - cobj.obj.Center).normalized;
        MyPhysics.GiveImpulseForce(cobj.obj.rbody, dir, config.force);
    }
}