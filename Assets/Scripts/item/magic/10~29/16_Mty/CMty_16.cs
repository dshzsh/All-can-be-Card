using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMty_16 : Cmagicbase_17
{

}
public class DMty_16 : DataBase
{
    public float force;
    public string jumpPar;

    public static GameObject jumpParPrefab;
    public override void Init(int id)
    {
        base.Init(id);
        jumpParPrefab = DataManager.LoadResource<GameObject>(id, jumpPar);
    }
}
public class SMty_16 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagicBefore, MyUseMagicBefore);
    }
    void MyUseMagicBefore(CardBase _card, MsgBase _msg)
    {
        CMty_16 card = _card as CMty_16;
        MsgMagicUse msg = _msg as MsgMagicUse;

        msg.valid = false;
        if (!TryGetCobj(card, out var cobj)) return;
        if (!cobj.obj.OnGround()) return;
        msg.valid = true;
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMty_16 card = _card as CMty_16;
        DMty_16 config = basicConfig as DMty_16;

        if (!TryGetCobj(msg.live, out var cobj, true)) return;

        MyPhysics.GiveImpulseForce(cobj.obj.rbody, cobj.obj.transform.up, config.force * msg.pow);
        GameObject.Instantiate(DMty_16.jumpParPrefab, cobj.obj.transform.position, cobj.obj.transform.rotation);
    }
}