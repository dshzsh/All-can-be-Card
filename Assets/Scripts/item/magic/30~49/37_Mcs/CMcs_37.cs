using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CMcs_37 : Cmagicbase_17
{

}
public class DMcs_37 : DataBase
{
    public BasicAtt usedisAdd;
}
public class SMcs_37 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CMcs_37 card = _card as CMcs_37;
        MsgOnItem msg = _msg as MsgOnItem;
        DMcs_37 config = basicConfig as DMcs_37;

        card.mdata.useDis = config.usedisAdd.WithPow(card.pow).UseAttTo(card.mdata.useDis, msg.op);
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMcs_37 card = _card as CMcs_37;
        DMcs_37 config = basicConfig as DMcs_37;

        if (!TryGetCobj(msg.live, out var cobj)) return;

        Vector3 init = cobj.obj.transform.position;
        SFcyxx_5.GiveCy(cobj.obj.gameObject, 0.2f);

        cobj.obj.transform.position = msg.pos;

        SFcyxx_5.GiveCy(cobj.obj.gameObject, 0.2f);
        SShoulderCamera_37.RefreshView(card);
    }
}