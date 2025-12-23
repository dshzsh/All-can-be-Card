using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCcolor_40 : Citem_33
{
    public Color color;
}
public class DGQCcolor_40 : DataBase
{

}
public class SGQCcolor_40 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BulletStart, BulletStart);
    }
    private Color MergeColor(Color color1, Color color2)
    {
        return color1 * 0.2f + color2 * 0.8f;
    }
    private static int _TintColorID = Shader.PropertyToID("_TintColor");
    private static int _ColorID = Shader.PropertyToID("_Color");
    public void AddColor(GameObject obj, Color color)
    {
        if (obj == null) return;

        foreach(var par in obj.GetComponentsInChildren<ParticleSystem>())
        {
            var main = par.main;
            main.startColor = MergeColor(main.startColor.color, color);
        }
        foreach (var par in obj.GetComponentsInChildren<MeshRenderer>())
        {
            if (par.material.HasColor(_ColorID))
                par.material.color = MergeColor(par.material.color, color);
            else if (par.material.HasColor(_TintColorID))
                par.material.SetColor(_TintColorID, MergeColor(par.material.GetColor(_TintColorID), color));

        }
        foreach (var par in obj.GetComponentsInChildren<TrailRenderer>())
        {
            par.startColor = MergeColor(par.startColor, color);
        }
    }
    void BulletStart(CardBase _card, MsgBase _msg)
    {
        CGQCcolor_40 card = _card as CGQCcolor_40;

        if (!TryGetCobj(card, out var cobj)) return;

        AddColor(cobj.obj.gameObject, card.color);
        if (cobj is Cbullet_10 cbullet) {
            AddColor(cbullet.hit, card.color);
        }
    }
}