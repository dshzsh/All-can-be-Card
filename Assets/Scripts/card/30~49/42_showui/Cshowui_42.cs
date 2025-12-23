using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cshowui_42 : CardBase
{

}
public class MsgOnShowUI : MsgBase
{
    public int op = 1;
}
public class Sshowui_42: SystemBase
{
    public static void AddShowUI(CObj_2 card, int op)
    {
        //考虑效率和代码结构，新增的维护放在了addcomponent里
        card.inShowUI += op;

        if (!InWorld(card)) return;
        
        if(card.inShowUI==1)
        {
            SendMsg(card, MsgType.OnShowUI, new MsgOnShowUI() { op = 1 });
        }
        else if(card.inShowUI==0)
        {
            SendMsg(card, MsgType.OnShowUI, new MsgOnShowUI() { op = -1 });
        }
    }
    public static bool InShowUI(CardBase card)
    {
        if(GetTop(card) is CObj_2 cObj)
        {
            return cObj.inShowUI >= 1;
        }
        return false;
    }
    public override void Init()
    {
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        Cshowui_42 card = _card as Cshowui_42;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetCobj(card, out var cObj)) return;

        if(msg.op==1)
        {
            AddShowUI(cObj, 1);
            SetMainPlayer(cObj);
        }
        else
        {
            AddShowUI(cObj, -1);
            SetMainPlayer(null);
        }
    }
}