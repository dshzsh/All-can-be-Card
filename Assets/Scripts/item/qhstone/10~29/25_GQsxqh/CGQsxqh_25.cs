using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQsxqh_25 : CGqhsbase_11
{

}
public class DGQsxqh_25 : DataBase
{

}
public class SGQsxqh_25 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQsxqh_25 card = _card as CGQsxqh_25;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQsxqh_25 config = basicConfig as DGQsxqh_25;

        for (int i = 0; i < 2; i++)
        {
            MsgMagicUse nmsg = new MsgMagicUse(msg);
            nmsg.ToNoCost();

            if (TryGetCobj(msg.live, out var cObj))
            {
                Vector3 dir = msg.pos - cObj.obj.Center;
                Quaternion rotation = Quaternion.Euler(0, (i == 0 ? 90 : -90), 0);
                nmsg.pos = rotation * dir + cObj.obj.Center;
            }

            UseTriList(nmsg, msg.triList, msg.triPos + 1);
        }
    }
}