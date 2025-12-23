using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMtsqb_31 : Cmagicbase_17
{

}
public class DMtsqb_31 : DataBase
{
    public float force;
}
public class SMtsqb_31 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMtsqb_31 card = _card as CMtsqb_31;
        DMtsqb_31 config = basicConfig as DMtsqb_31;

        if (!TryGetCobj(card, out var cobj, true)) return;

        Vector3 dir = MyMath.V3zeroYNor(msg.pos - cobj.obj.Center);
        if (cobj is Clive_19 clive)
        {
            dir = clive.moveWantDir; dir.y = 0; dir.Normalize();
        }
        if (dir.magnitude <= MyMath.SmallFloat)
            dir = MyMath.V3zeroYNor(msg.pos - cobj.obj.Center);

        MyPhysics.GiveImpulseForce(cobj.obj.rbody, dir, config.force * msg.pow);
        GameObject.Instantiate(DMty_16.jumpParPrefab, cobj.obj.transform.position, cobj.obj.transform.rotation);
    }
}