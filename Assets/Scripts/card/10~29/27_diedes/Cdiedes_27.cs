using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cdiedes_27 : CardBase
{
    public bool liveDiePar = false;
}
public class Ddiedes_27: DataBase
{
    public string diePar;

    public GameObject dieParPrefab;
    public override void Init(int id)
    {
        dieParPrefab = DataManager.LoadResource<GameObject>(id, diePar);
        Sdiedes_27.dieParPrefab = dieParPrefab;
    }
}

public class Sdiedes_27: SystemBase
{
    public static GameObject dieParPrefab;
    public static void GiveDiePar(GameObject obj, float size = -1, Color color = default)
    {
        if (size == -1)
            size = obj.transform.localScale.y;

        Vector3 givePos = obj.transform.position + Vector3.up * size / 2;
        //给予生命死亡时的粒子爆炸特效
        ParticleSystem diePar = GameObject.Instantiate(dieParPrefab, givePos, obj.transform.rotation).GetComponent<ParticleSystem>();

        diePar.transform.localScale = new Vector3(size, size, size);
        var renderer = diePar.GetComponent<ParticleSystemRenderer>();
        if (renderer != null && renderer.material != null)
        {
            if (color == default)
                renderer.material.color = obj.gameObject.GetComponentInChildren<Renderer>().material.color;
            else renderer.material.color = color;
        }
    }
    public override void Init()
    {
        AddHandle(MsgType.TrueDie, Die, HandlerPriority.Lowest);
    }
    void Die(CardBase _card, MsgBase _msg)
    {
        Cdiedes_27 card = _card as Cdiedes_27;
        if(card.liveDiePar && TryGetCobj(card,out var cobj))
        {
            Ddiedes_27 config = basicConfig as Ddiedes_27;
            //给予生命死亡时的粒子爆炸特效
            ParticleSystem diePar = GameObject.Instantiate(config.dieParPrefab, cobj.obj.Center, cobj.obj.transform.rotation).GetComponent<ParticleSystem>();
            float y = cobj.obj.transform.localScale.y;
            diePar.transform.localScale = new Vector3(y, y, y);
            var renderer = diePar.GetComponent<ParticleSystemRenderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.color = cobj.obj.gameObject.GetComponentInChildren<Renderer>().material.color;
            }
        }
        DestroyCard(GetTop(_card));
    }
}