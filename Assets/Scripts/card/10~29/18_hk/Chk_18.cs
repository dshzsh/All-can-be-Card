using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Chk_18 : Cmagicbase_17
{

}
public class Dhk_18 : DataBase
{
    
}
public class Shk_18 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        Chk_18 card = _card as Chk_18;
        Dhk_18 config = DataManager.GetConfig<Dhk_18>(id);
        

    }
}