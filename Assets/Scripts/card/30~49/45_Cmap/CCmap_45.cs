using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using static SfloorBase_2;
using static SCmap_45;

public class CCmap_45 : CardBase
{
    // 当前环境
    public CardBase nowEnv;
    public int nowDifficulty = 0;
    public int mapCnt = 0;

    // 地图信息
    public int nowHeight, nowIndex;
    public List<List<MapNode> > mapNodes=new();// height => index => node
    public MapNode NowNode
    {
        get { return mapNodes[nowHeight][nowIndex]; }
    }
}
public class DCmap_45: DataBase
{
    public Dictionary<string, float> floorTypeWeightData = new();
    public static Dictionary<FloorType, float> floorTypeWeight = new();
    public string mapUI;
    public string mapGoodUI;
    public string highLightUI;
    public string linkLineUI;

    public static GameObject mapUIPrefab;
    public static GameObject mapGoodUIPrefab;
    public static GameObject highLightUIPrefab;
    public static GameObject linkLineUIPrefab;
    public override void Init(int id)
    {
        mapUIPrefab = DataManager.LoadResource<GameObject>(id, mapUI);
        mapGoodUIPrefab = DataManager.LoadResource<GameObject>(id, mapGoodUI);
        highLightUIPrefab = DataManager.LoadResource<GameObject>(id, highLightUI);
        linkLineUIPrefab = DataManager.LoadResource<GameObject>(id, linkLineUI);

        foreach (var pair in floorTypeWeightData)
        {
            floorTypeWeight.Add(System.Enum.Parse<FloorType>(pair.Key), pair.Value);
        }
    }
}
public class SCmap_45: SystemBase
{
    public static CCmap_45 mainMap;
    public static CfloorBase_2 nowFloor;

    public static int mTCreateNewMap = MsgType.ParseMsgType(GetTypeId(typeof(CCmap_45)), 0);
    public static int mTCreateNewMapAfter = MsgType.ParseMsgType(GetTypeId(typeof(CCmap_45)), 1);

    public class MapStruct
    {
        public Dictionary<FloorType, float> floorTypeWeight = new();

        public float linkRate = 0.6f;
        public int linkRange = 1;

        public int height = 2;
        public int widthMin = 2, widthMax = 4;

        public int repeat = 3;

        public int nextEnvChoose = 3;
    }
    public class MsgCreateNewMap: MsgBase
    {
        public MapStruct mapStruct = new();
        public List<List<MapNode>> newMap;
    }
    public class MapNode
    {
        public int id; // 这个房间的实际内容
        public bool isEnv = false;
        public bool showDetail = false;
        public float offsetX, offsetY; // 显示在地图上的偏移
        public List<int> links = new(); // 向下一层连接的点

        public float size = 1f;
    }
    
    public static DLEenv GetEnvMat()
    {
        DLEenv denv = DataManager.GetConfig<DLEenv>(SCmap_45.mainMap.nowEnv.id);
        if (denv == null) // 默认材质用草原材质
            denv = DataManager.GetConfig<DLEenv>(DataManager.VidToPid(1, CardField.env));
        return denv;
    }
    public static int GetMapCnt()
    {
        return mainMap.mapCnt;
    }
    private static List<int> GetNextFloorIndexs()
    {
        List<int> list = new();
        foreach(int id in mainMap.mapNodes[mainMap.nowHeight][mainMap.nowIndex].links)
        {
            list.Add(id);
        }
        return list;
    }
    public static void CreateInitMap()
    {
        mainMap.nowEnv = new CNull_0();
        {
            List<MapNode> list = new();
            MapNode node = new MapNode();
            node.id = GetTypeId(typeof(CLFmain_4));
            node.links = new List<int>() { 0, 1, 2 };
            list.Add(node);
            mainMap.mapNodes.Add(list);
        }
        {
            // 下一环境的预览
            List<MapNode> list = new();
            List<int> nextEnvs = Sitem_33.GetNoRepeatRandomItems(3, mainMap.nowDifficulty + 1, MyTag.CardTag.env);
            for (int i = 0; i < 3; i++)
            {
                MapNode node = new();
                node.id = nextEnvs[i];
                node.isEnv = true;
                list.Add(node);
            }
            mainMap.mapNodes.Add(list);
        }
    }
    public static void CreateNewMap(int env)
    {
        InactiveComponent(mainMap, mainMap.nowEnv);
        mainMap.nowEnv = CreateCard(env);
        ActiveComponent(mainMap, mainMap.nowEnv);
        MsgCreateNewMap msg = new MsgCreateNewMap();

        foreach (var pair in DCmap_45.floorTypeWeight)
        {
            msg.mapStruct.floorTypeWeight.Add(pair.Key, pair.Value);
        }

        //SendMsg(mainMap, mTCreateNewMap, msg);
        SendMsgToPlayer(mTCreateNewMap, msg);
        List<List<MapNode>> newMap = new();

        {
            List<MapNode> list = new();
            MapNode node = new MapNode();

            if (mainMap.mapCnt==0)
                node.id = GetTypeId(typeof(CLFsfzd_8));
            else
                node.id = DFloor.GetRandFloor(mainMap.nowEnv.id, FloorType.start);

            list.Add(node);
            newMap.Add(list);
        }
        for(int tt=0;tt<msg.mapStruct.repeat;tt++)
        {
            for (int i = 0; i < msg.mapStruct.height; i++)
            {
                List<MapNode> list = new();

                int sizeW = Random.Range(msg.mapStruct.widthMin, msg.mapStruct.widthMax + 1);
                for (int j = 0; j < sizeW; j++)
                {
                    MapNode node = new MapNode();
                    //if (i == msg.mapStruct.treaGoodHeight)
                    //    node.id = DFloor.GetRandFloor(mainMap.nowEnv.id, FloorType.treasureGood);
                    //else
                        node.id = DFloor.GetRandFloor(mainMap.nowEnv.id,
                            MyRandom.GetRandomKeyByWeight(msg.mapStruct.floorTypeWeight));
                    list.Add(node);
                }
                newMap.Add(list);
            }
            {
                List<MapNode> list = new();
                MapNode node = new MapNode();
                // 环境的最后一个节点为boss战斗
                if (tt == msg.mapStruct.repeat - 1)
                {
                    node.id = DFloor.GetRandFloor(mainMap.nowEnv.id, FloorType.bossFight);
                    node.size = 1.5f;
                    node.showDetail = true;
                }
                else
                {
                    node.id = DFloor.GetRandFloor(mainMap.nowEnv.id, FloorType.fight);
                    node.size = 1.2f;
                }
                
                list.Add(node);
                newMap.Add(list);
            }
        }
        
        {
            // 下一环境的预览
            List<MapNode> list = new();
            List<int> nextEnvs = Sitem_33.GetNoRepeatRandomItems(msg.mapStruct.nextEnvChoose, mainMap.nowDifficulty + 1, MyTag.CardTag.env);
            for(int i=0;i< msg.mapStruct.nextEnvChoose; i++)
            {
                MapNode node = new();
                node.id = nextEnvs[i];
                node.isEnv = true;
                list.Add(node);
            }
            newMap.Add(list);
        }

        //添加道路连线：至少会向上连接一根线
        for (int i = 0; i < newMap.Count-1; i++)
        {
            List<MapNode> list = newMap[i];
            List<MapNode> nextList = newMap[i + 1];
            if (i == 0|| i == newMap.Count - 2)
            {
                MapNode node = list[0];
                for (int k = 0; k < nextList.Count; k++)
                    node.links.Add(k);
                continue;
            }
            for (int j = 0; j < list.Count; j++)
            {
                MapNode node = list[j];

                float linkRate = msg.mapStruct.linkRate;
                // 因为地图显示的是居中的，所以需要计算偏移
                int offset = (nextList.Count - list.Count) / 2;

                int startk = j - msg.mapStruct.linkRange + offset; 
                if (startk < 0) startk = 0;
                if(startk > nextList.Count - 1)
                {
                    node.links.Add(nextList.Count - 1);
                    continue;
                }

                int endk = j + msg.mapStruct.linkRange + offset; 
                if (endk > nextList.Count - 1) endk = nextList.Count - 1;
                if (endk < 0)
                {
                    node.links.Add(0);
                    continue;
                }
                
                for (int tt = 0; tt <= 5; tt++)
                {
                    for (int k = startk; k <= endk; k++)
                    {
                        if (MyRandom.RandPer(linkRate))
                            node.links.Add(k);
                    }
                    if (node.links.Count != 0) break;
                    linkRate += 0.1f;
                }
            }
        }

        msg.newMap = newMap;

        //SendMsg(mainMap, mTCreateNewMapAfter, msg);
        SendMsgToPlayer(mTCreateNewMapAfter, msg);

        mainMap.mapNodes = newMap;
        mainMap.nowHeight = mainMap.nowIndex = 0;
    }
    const float csmLength = 3f;
    public static void GiveNextFloorCsm(Vector3 pos, Quaternion rot)
    {
        List<int> list = GetNextFloorIndexs();
        
        float total = csmLength * list.Count;
        for(int i=0; i<list.Count; i++)
        {
            MapNode nextNode = mainMap.mapNodes[mainMap.nowHeight + 1][list[i]];
            GameObject csmObj;
            if(nextNode.isEnv)
            {
                CMOcjcsm_2 csm = CreateCard<CMOcjcsm_2>();
                csm.targetEnvID = nextNode.id;
                csm.color = GetCardColor(csm.targetEnvID);
                AddToWorld(csm);
                csmObj = csm.obj.gameObject;
            }
            else
            {
                CMOcsm_1 csm = CreateCard<CMOcsm_1>();
                csm.targetID = nextNode.id;
                csm.index = list[i];
                csm.color = GetFloorShow(csm.targetID).color;
                AddToWorld(csm);
                csmObj = csm.obj.gameObject;
            }
            // 整体思想，算出全部的位置后再减去一半的总长
            float offset = (2 * i + 1) / 2f * csmLength - total / 2;
            csmObj.transform.SetParent(nowFloor.obj.transform, false);
            csmObj.transform.SetPositionAndRotation(pos + rot * Vector3.left * offset, rot);
        }
    }
    public static void ToNewFloor(CfloorBase_2 floor, int index)
    {
        // 处理地图的逻辑
        SCmap_45.mainMap.mapCnt++;
        // 根据index转移当前位置
        mainMap.nowHeight++;
        mainMap.nowIndex = index;

        // 将玩家放到位置
        MsgFloorStart msg = new MsgFloorStart(GetMainPlayer() as CObj_2);
        SendMsg(floor, SfloorBase_2.mTFloorStart, msg);
        SendMsg(GetMainPlayer(), mTFloorStart, msg);
    }

    public override void Init()
    {
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);

        AddFloorShow(FloorType.fight, new FloorTypeShow(GetTypeId(typeof(CLFsimple_3)), new Color(1, 0.2f, 0.2f)));
        AddFloorShow(FloorType.bossFight, new FloorTypeShow(GetTypeId(typeof(CLFsimple_3)), new Color(1, 0.2f, 0.2f)));
        AddFloorShow(FloorType.treasureGood, new FloorTypeShow(GetTypeId(typeof(CLFdjf_5)), Color.yellow));
        AddFloorShow(FloorType.start, new FloorTypeShow(GetTypeId(typeof(CLFqs_9)), Color.green));
        AddFloorShow(FloorType.thing, new FloorTypeShow(GetTypeId(typeof(CLFthing_10)), new Color(0, 0.8f, 1)));
        AddFloorShow(FloorType.tresureMagic, new FloorTypeShow(GetTypeId(typeof(CLFtreaMagic_11)), new Color(0, 0.4f, 1)));
        AddFloorShow(FloorType.wzd, new FloorTypeShow(GetTypeId(typeof(CLFwzd_12)), new Color(1, 0.4f, 0)));
        AddFloorShow(FloorType.shop, new FloorTypeShow(GetTypeId(typeof(CLFshop_13)), new Color(1, 0.66f, 0)));
        AddFloorShow(FloorType.forge, new FloorTypeShow(GetTypeId(typeof(CLFdzt_14)), new Color(0.5245282f, 0.3949164f, 0.2721608f)));
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CCmap_45 card = _card as CCmap_45;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            mainMap = card;
            CreateInitMap();
        }
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CCmap_45 card = _card as CCmap_45;
        MsgUpdate msg = _msg as MsgUpdate;
        DCmap_45 config = basicConfig as DCmap_45;

        
    }
}