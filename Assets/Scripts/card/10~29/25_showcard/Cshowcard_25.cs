using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cshowcard_25 : CardBase
{

}

public class Sshowcard_25: SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Cshowcard_25 card = _card as Cshowcard_25;
        MsgUpdate msg = _msg as MsgUpdate;

        
    }
}