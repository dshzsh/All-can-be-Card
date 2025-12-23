using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBmfhd_1 : Cbullet_10
{
    public float restoreSpeed = 10;

    public Vector3 initPos;
    public Quaternion initRot;
}

public class SBmfhd_1 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BulletStart, Start);
        AddHandle(MsgType.FixedUpdate, Update);
    }
    void Start(CardBase _card, MsgBase _msg)
    {
        CBmfhd_1 card = _card as CBmfhd_1;

        if(TryGetCobj(_card, out var cobj) && TryGetCobj(Sysw_26.GetYsw(_card), out var fromobj))
        {
            card.initPos = cobj.obj.transform.position - fromobj.obj.transform.position;
            card.initRot = card.obj.transform.rotation;
        }
    }
    public void Update(CardBase _card, MsgBase _msg)
    {
        CBmfhd_1 card = _card as CBmfhd_1;
        MsgUpdate msg= _msg as MsgUpdate;

        if(TryGetCobj(_card, out var cobj) && TryGetCobj(Sysw_26.GetYsw(_card), out var fromobj))
        {
            cobj.obj.transform.position = Vector3.Lerp(cobj.obj.transform.position
                , card.initPos + fromobj.obj.transform.position, msg.time * card.restoreSpeed);
            cobj.obj.transform.rotation = Quaternion.Lerp(cobj.obj.transform.rotation, card.initRot, msg.time * card.restoreSpeed);
        }
    }
}