using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQcasf_28 : CGqhsbase_11
{

}
public class DGQcasf_28 : DataBase
{

}
public class SGQcasf_28 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MagicPress, MagicPress);
    }
    void MagicPress(CardBase _card, MsgBase _msg)
    {
        CGQcasf_28 card = _card as CGQcasf_28;
        MsgMagicPress msg = _msg as MsgMagicPress;

        if (msg.isNowUse) return;
        if (!IsParentCard(card, msg.magic)) return;

        SendMsg(GetTop(card), MsgType.MagicCon, new MsgMagicCon(msg.key, msg.pos));
    }
}