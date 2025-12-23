using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQewcc_64 : CGQCewcc_63
{

}
public class DGQewcc_64 : DataBase
{
    public float exTimeMax = 1;
}
public class SGQewcc_64 : SGQCewcc_63
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGQewcc_64 card = _card as CGQewcc_64;
        DGQewcc_64 config = basicConfig as DGQewcc_64;
        card.exTimeMax = config.exTimeMax;
    }
}