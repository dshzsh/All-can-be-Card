using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cquickdie_34 : CardBase
{
    public bool needTrueDie = false;
}

public class Squickdie_34: SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Die, Die, HandlerPriority.Lowest);
        AddHandle(MsgType.Update, Update);
    }

    void Update(CardBase _card, MsgBase _msg)
    {
        Cquickdie_34 card = _card as Cquickdie_34;

        // 到下一帧再死能规避很多问题（受伤后的一些内容，斩杀对于卡信息的调用等）
        if(card.needTrueDie)
            SendMsg(GetTop(card), MsgType.TrueDie, _msg);
    }
    void Die(CardBase _card, MsgBase _msg)
    {
        Cquickdie_34 card = _card as Cquickdie_34;

        card.needTrueDie = true;
    }
}