using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCxj_43 : Cbuffbase_36
{
    // 控制顺时针旋转
    public float rotAngle = 360;
    public float rotSpeed = 5;
}
public class DGQCxj_43 : DataBase
{
    
}
public class SGQCxj_43 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.FixedUpdate, Update);
    }
    public void Update(CardBase _card, MsgBase _msg)
    {
        CGQCxj_43 card = _card as CGQCxj_43;
        MsgUpdate msg = _msg as MsgUpdate;
        DGQCxj_43 config = basicConfig as DGQCxj_43;

        if (!TryGetCobj(card, out var cobj)) return;

        // 这个只要绕着自己的中心旋转就行了，已经提前准备好自己的位置了
        cobj.obj.transform.RotateAround(cobj.obj.transform.position, cobj.obj.transform.up,
            card.rotAngle * card.rotSpeed * msg.time);
    }
}