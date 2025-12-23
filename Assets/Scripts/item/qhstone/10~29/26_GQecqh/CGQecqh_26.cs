using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQecqh_26 : CGqhsbase_11
{

}
public class DGQecqh_26 : DataBase
{
    public float angle;
}
public class SGQecqh_26 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQecqh_26 card = _card as CGQecqh_26;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQecqh_26 config = basicConfig as DGQecqh_26;

        // i为1为修改原有的内容
        for (int i = 0; i < 2; i++)
        {
            MsgMagicUse nmsg = i == 1 ? msg : new MsgMagicUse(msg);
            if (i == 0) nmsg.ToNoCost();

            if (TryGetCobj(msg.live, out var cObj))
            {
                Vector3 dir = msg.pos - cObj.obj.Center;
                Quaternion rotation = Quaternion.Euler(0, (i == 0 ? 1 : -1)*config.angle, 0);
                nmsg.pos = rotation * dir + cObj.obj.Center;
            }

            if (i == 0)
                UseTriList(nmsg, msg.triList, msg.triPos + 1);
        }
    }
}