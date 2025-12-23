using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static Sitem_33;
using static SystemManager;

public class CObj_2 : CardBase
{
    public bool needGiveObj = true;
    public CardObj obj;

    public int team = 0;
    public BasicAtt size = new BasicAtt(1);// 统一化管理属性，要使用的时候用attAndRevise施加

    public Vector3 centerOffset = Vector3.zero;
    public float height = 1;    

    public int inShowUI = 0;

    [JsonIgnore]
    public Chealth_4 myHealth;
    [JsonIgnore]
    public Cbuff_35 myBuff;
    [JsonIgnore]
    public Cbag_40 myBag;
    [JsonIgnore]
    public Cmagic_14 myMagic;

    [JsonIgnore]
    public Dictionary<int, List<CardAndPriority> > msgTypeToHandler = new();
    [JsonIgnore]
    public LiveUpUI liveUpUI;}
public class DObj_2 : DataBase
{
    public string obj;

    [JsonIgnore]
    public GameObject gameObject;
    public override void Init(int id)
    {
        gameObject = DataManager.LoadResource<GameObject>(id, obj);
    }
}
public class SObj_2 : SystemBase
{
    public static AnimationClip defaultAnim;
    public override void Create(CardBase _card)
    {
        CObj_2 card = (CObj_2)_card;
        //CreateCardObj(id, cardAbandon);
        AddComponent(card, CreateCard<Cbuff_35>());
        //AddToWorld(cardAbandon);
    }
    public override void Init()
    {        
        AddHandle(MsgType.OnItem, OnObj, HandlerPriority.Highest);
    }
    //better:在后摇结束前无限循环当前的动画片段
    public static void PlayAtkAnim(CObj_2 cobj, AnimationClip anim, float windDownTime = -1, bool onlyUpperBody = false, float atkSpeed = 1f)
    {
        if (anim == null) anim = defaultAnim;
        if (cobj == null) return;

        float trueWindDownTime = (windDownTime == -1 ? anim.length : windDownTime) / atkSpeed;

        if (onlyUpperBody)
        {
            cobj.obj.SetLockMotion(0);
            cobj.obj.SetUpper(trueWindDownTime);
        }
        else
        {
            cobj.obj.SetMoveSpeed(0);
            //cobj.moveWantDir = Vector3.zero;
            cobj.obj.SetLockMotion(trueWindDownTime);
            cobj.obj.SetUpper(0);
        }

        if (cobj.obj.animator == null) return;

        string useAtk = cobj.obj.useAtk2 ? "atk2" : "atk";
        cobj.obj.useAtk2 = !cobj.obj.useAtk2;

        cobj.obj.overrideController[useAtk] = onlyUpperBody ? null : anim;
        cobj.obj.overrideController["u" + useAtk] = anim;

        cobj.obj.animator.SetFloat(CardObj.hash_atkSpeed, atkSpeed);
        

        cobj.obj.animator.SetTrigger(onlyUpperBody ? "goidle" : useAtk);
        cobj.obj.animator.SetTrigger("u" + useAtk);
    }
    void OnObj(CardBase _card, MsgBase _msg)
    {
        CObj_2 card = _card as CObj_2;
        MsgOnItem msg = _msg as MsgOnItem;

        if (msg.op == 1)
        {
            if (card.needGiveObj)
                CreateCardObj(id, card);
        }
        else
        {
            if (card.obj.gameObject != null)
                GameObject.Destroy(card.obj.gameObject);
        }
    }
    
    public override Color GetColor(CardBase _card)
    {
        return GoodUIColor.Obj;
    }
}
