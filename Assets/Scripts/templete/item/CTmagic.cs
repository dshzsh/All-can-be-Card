using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTmagic : Cmagicbase_17
{

}
public class DTmagic : DataBase
{
    
}
public class STmagic : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CTmagic card = _card as CTmagic;
        MsgUpdate msg = _msg as MsgUpdate;
        DTmagic config = basicConfig as DTmagic;

        Debug.Log(msg.time);
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CTmagic card = _card as CTmagic;
        DTmagic config = basicConfig as DTmagic;

        
    }
}