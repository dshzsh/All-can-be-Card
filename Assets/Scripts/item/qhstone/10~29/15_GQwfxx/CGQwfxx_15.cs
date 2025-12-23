using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQwfxx_15 : CGqhsbase_11
{

}
public class DGQwfxx_15 : DataBase
{

}
public class SGQwfxx_15 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyContainerJudge, MyContainerJudge);
        AddHandle(MsgType.ParseDescribe, ParseDescribe);
    }
    void MyContainerJudge(CardBase _card, MsgBase _msg)
    {
        CGQwfxx_15 card = _card as CGQwfxx_15;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (msg.gmsg.item != card.parent) return;

        msg.ok = false;
        msg.AddMsg("无法卸下！");
    }
    void ParseDescribe(CardBase _card, MsgBase _msg)
    {
        CGQwfxx_15 card = _card as CGQwfxx_15;
        MsgParseDescribe msg = _msg as MsgParseDescribe;

        if (msg.card != card.parent) return;

        msg.InsertFront($"<color=#{Sitem_33.SpecialTextColor.Important}>无法卸下</color>");
    }
}