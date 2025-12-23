using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cdes_13 : CardBase
{

}

public class Sdes_13 : SystemBase
{
    public override void Create(CardBase _card)
    {
        _card.valid = 0;
    }
}