using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyEditorGood : MyEditorBase
{
    [MenuItem("My创建/道具", priority = 0)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.good;
        window.Show();
    }
}
public class MyEditorMagic : MyEditorBase
{
    [MenuItem("My创建/技能", priority = 1)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.magic;
        window.Show();
    }
}
public class MyEditorQhstone : MyEditorBase
{
    [MenuItem("My创建/强化石", priority = 2)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.qhstone;
        window.Show();
    }
}
public class MyEditorBuff : MyEditorBase
{
    [MenuItem("My创建/Buff", priority = 3)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.buff;
        window.Show();
    }
}
public class MyEditorRule : MyEditorBase
{
    [MenuItem("My创建/规则", priority = 4)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.rule;
        window.Show();
    }
}
public class MyEditorBuild : MyEditorBase
{
    [MenuItem("My创建/流派", priority = 5)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.build;
        window.Show();
    }
}
public class MyEditorBullet : MyEditorBase
{
    [MenuItem("My创建/子弹", priority = 100)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.bullet;
        window.Show();
    }
}
public class MyEditorLive : MyEditorBase
{
    [MenuItem("My创建/生命", priority = 101)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.live;
        window.Show();
    }
}
public class MyEditorEnemy : MyEditorBase
{
    [MenuItem("My创建/敌人", priority = 102)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.enemy;
        window.Show();
    }
}
public class MyEditorMapObj : MyEditorBase
{
    [MenuItem("My创建/地图物体", priority = 103)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.mapObj;
        window.Show();
    }
}
public class MyEditorEnv : MyEditorBase
{
    [MenuItem("My创建/环境", priority = 200)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.env;
        window.Show();
    }
}
public class MyEditorFloor : MyEditorBase
{
    [MenuItem("My创建/地图节点", priority = 201)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.floor;
        window.Show();
    }
}
public class MyEditorFight : MyEditorBase
{
    [MenuItem("My创建/战斗节点", priority = 202)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.fight;
        window.Show();
    }
}
public class MyEditorThing : MyEditorBase
{
    [MenuItem("My创建/事件", priority = 203)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.thing;
        window.Show();
    }
}

public class MyEditorNpc : MyEditorBase
{
    [MenuItem("My创建/NPC", priority = 204)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.npc;
        window.Show();
    }
}
public class MyEditorAtt : MyEditorBase
{
    [MenuItem("My创建/属性", priority = 900)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.att;
        window.Show();
    }
}

public class MyEditorTag : MyEditorBase
{
    [MenuItem("My创建/tag", priority = 1000)]
    static void Init()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.CardField = CardField.tag;
        window.Show();
    }
}