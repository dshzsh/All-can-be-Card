using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;

public class World : MonoBehaviour
{
    public static World world;
    public static void MainInit()
    {
        CardObj.hash_moveSpeed = Animator.StringToHash("moveSpeed");
        CardObj.hash_move = Animator.StringToHash("move");
        CardObj.hash_atkSpeed = Animator.StringToHash("atkSpeed");
        CardObj.hash_direction = Animator.StringToHash("direction");
        CardObj.layer_ghost = LayerMask.NameToLayer("ghost");
        CardObj.layer_wfzd = LayerMask.NameToLayer("wfzd");
        CardObj.layer_normal = LayerMask.NameToLayer("Default");
    }
    private void Awake()
    {
        world = this;
        MainInit();
        DataManager.Init();
        SystemManager.Init();
        CardManager.Init();
        Time.fixedDeltaTime = 0.01f;

        CLplayer_1 player = CardManager.CreateCard<CLplayer_1>();
        AddComponent(player, CreateCard<CTest_1>());
        AddToWorld(player);
        player.obj.transform.SetParent(null);
        player.obj.transform.position = new Vector3(2, 2, 2);

        CCmap_45 map = CreateCard<CCmap_45>();
        AddComponent(player, map);
    }

    private void Start()
    {
        //CfloorBase_2 floor = CreateCard<CfloorBase_2>();        
        
        //Debug.Log(DataManager.GetDescirbe(new Chealth_4 { myAtt.cur = 666, myAtt.max = 777 }));
    }
    /*private void FixedUpdate()
    {
        SystemManager.SendMsg(CardManager.GetAllCards(), MsgType.FixedUpdate, new MsgUpdate(Time.fixedDeltaTime));
    }
    private float time = 0;
    private void Update()
    {
        if(MyTool.IntervalTime(1, ref time, Time.deltaTime))
        {
            SystemManager.SendMsg(CardManager.GetAllCards(), MsgType.UpdateSec, new MsgUpdate(1));
        }
        SystemManager.SendMsg(CardManager.GetAllCards(), MsgType.Update, new MsgUpdate(Time.deltaTime));
    }*/
    

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
