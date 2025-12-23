using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFwfsf_12 : Cbuffbase_36
{
    
}
public class DFwfsf_12 : DataBase
{
    
}
public class SFwfsf_12 : Sbuffbase_36
{
    public static int mTCanUseMagic = MsgType.ParseMsgType(CardField.buff, 12, 0);
    public class MsgCanUseMagic: MsgBase
    {
        public bool canUseMagic = true;

    }
    public static bool InWfsf(CardBase live)
    {
        SFwfsf_12.MsgCanUseMagic cmsg = new SFwfsf_12.MsgCanUseMagic();
        SendMsg(live, SFwfsf_12.mTCanUseMagic, cmsg);
        return !cmsg.canUseMagic;
    }

    public static int bid;
    public override void Init()
    {
        base.Init();
        // AddHandle(MsgType.MagicBegin, MagicBeginBefore, HandlerPriority.Before);
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(mTCanUseMagic, CanUseMagic);
        bid = id;
    }
    void CanUseMagic(CardBase _card, MsgBase _msg)
    {
        MsgCanUseMagic msg = _msg as MsgCanUseMagic;

        msg.canUseMagic = false;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFwfsf_12 card = _card as CFwfsf_12;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetClive(card, out var clive)) return;
        if (clive.myMagic == null) return;
        clive.myMagic.magicCanUseMeta -= msg.op;
    }
}