using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGAmanaMax_3 : CGAattbase_1
{

}
public class DGAmanaMax_3 : DataBase
{
    public float recoverRate;
}
public class SGAmanaMax_3 : SGAattbase_1
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGAmanaMax_3 card = _card as CGAmanaMax_3;
        MsgUpdate msg = _msg as MsgUpdate;
        DGAmanaMax_3 config = basicConfig as DGAmanaMax_3;

        SendMsg(GetTop(_card), MsgType.RestoreMana, 
            new MsgRestoreMana(card.bvalue.GetValue() * config.recoverRate * msg.time));
    }
}