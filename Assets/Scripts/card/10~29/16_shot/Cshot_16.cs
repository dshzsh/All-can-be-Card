using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Dshot_16 : DataBase
{

}
public class Cshot_16 : Cmagicbase_17
{
    
}
public class Sshot_16 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        SObj_2.defaultAnim = DataManager.GetConfig<Dmagic>(id).animClip;
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        Cshot_16 card = _card as Cshot_16;

    }
}