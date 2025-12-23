using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLEenvBase_2 : Citem_33
{

}
public class DLEenvBase_2 : DataBase
{

}
public class DLEenv : DataBase
{
    public string ground, wall;

    public Material groundMat, wallMat;
    public override void Init(int id)
    {
        base.Init(id);
        groundMat = DataManager.LoadResource<Material>(id, ground);
        wallMat = DataManager.LoadResource<Material>(id, wall);
    }
}

public class SLEenvBase_2 : Sitem_33
{
    public Color envColor = Color.white;
    public override void Init()
    {
        base.Init();
        envColor = DataManager.GetConfig<Dcolor>(id).color;
        //Debug.Log(envColor);
    }
    public override Color GetColor(CardBase _card)
    {
        return envColor;
    }
}