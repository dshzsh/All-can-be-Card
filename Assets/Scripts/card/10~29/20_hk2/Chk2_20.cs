using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Chk2_20 : Cmagicbase_17
{

}
public class Dhk2_20 : DataBase
{
    
}
public class Shk2_20 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        Chk2_20 card = _card as Chk2_20;
        Dhk2_20 config = DataManager.GetConfig<Dhk2_20>(id);

        
    }
}