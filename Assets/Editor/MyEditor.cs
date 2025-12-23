using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MyEditorBase : EditorWindow
{
    public static string pathbg = "Scripts";
    public static string pathbgres = "Resources";
    public static string pathtemp = "Scripts/templete";

    static string pathbgdata = "Resources/" + DataManager.dataPath;
    static string metaPath = Application.dataPath + "/" + pathbgdata + "/meta.json";

    protected string bgrowname = "null";
    protected int id = -1;
    string ansmsg = "";
    public CardField CardField = CardField.card;
    string preString = "null";
    string folder = "";
    //bool exSetting = false;
    bool addPreString = true;

    static List<MetaData> metaDatas = new List<MetaData>();

    [MenuItem("My创建/card", priority = 10000)]
    static void Init0()
    {
        MyEditorBase window = (MyEditorBase)EditorWindow.GetWindow(typeof(MyEditorBase));
        window.Show();
    }

    public static string GetFolder(CardField pre)
    {
        foreach(MetaData metaData in metaDatas)
        {
            if (metaData.cardField == pre)
                return metaData.folder;
        }
        return "";
    }
    public static string GetPreString(CardField pre)
    {
        foreach (MetaData metaData in metaDatas)
        {
            if (metaData.cardField == pre)
                return metaData.fName;
        }
        return "null";
    }
    public static string GetHeadString(CardField pre)
    {
        foreach (MetaData metaData in metaDatas)
        {
            if (metaData.cardField == pre)
                return metaData.headString;
        }
        return "";
    }

    protected virtual void ExOnGUI()
    {

    }

    void OnGUI()
    {
        GUILayout.Label("卡牌创建", EditorStyles.boldLabel);
        bgrowname = EditorGUILayout.TextField("卡牌内部名称", bgrowname);
        CardField = (CardField)EditorGUILayout.EnumPopup("卡牌预设", CardField);
        addPreString = EditorGUILayout.Toggle("是否添加前置字符", addPreString);
        id = -1;
        ExOnGUI();
        if (GUILayout.Button("确认创建"))
        {
            if (NewItem())
            {
                ansmsg = "成功创建\nname = " + bgrowname + "\nid   = " + id + "\n的卡牌";
                id = -1;
            }
            else ansmsg = "失败的操作！";
        }
        //if (GUILayout.Button("修正数据")) FixAllData();
        GUILayout.Label(ansmsg);
    }
    bool NewItem()
    {
        string meta = File.ReadAllText(metaPath);
        metaDatas = MyTool.JsonDeserializeObject<List<MetaData>>(meta);
        foreach (MetaData data in metaDatas)
        {
            data.cardField = Enum.Parse<CardField>(data.fName);
        }

        if (bgrowname.Equals("null")) { return false; }
        
        preString = GetPreString(CardField);
        folder = GetFolder(CardField);

        //默认增加一个大写字母在首以区分脚本
        bgrowname = (addPreString ? GetHeadString(CardField) : "") + bgrowname;

        if (!AppendId()) return false;//向BGid里的id追加新物品的id，如果id为-1就自动读取并追加id

        NewAsset();//创建资源文件

        NewCs();//创建编辑用脚本

        return true;
    }
    protected virtual void NewCs()
    {
        string bgname = bgrowname + "_" + id;
        //目录
        string bfpath = System.IO.Path.Combine(pathbg + "/" + folder + preString, DataManager.GetIdFolderName(id) + "/" + id + "_" + bgrowname);
        string csname = "C" + bgname;

        //创建文件，写入所有内容
        string temp = "CT" + preString;
        
        string tpPath = Application.dataPath + "/" + pathtemp + "/" + folder + temp + ".cs";
        string content = File.ReadAllText(tpPath);
        content = content.Replace("T" + preString + " ", bgname + " ");
        content = content.Replace("T" + preString + ";", bgname + ";");

        string bpath = bfpath + "/" + csname + ".cs";
        string tbpath = Application.dataPath + "/" + bpath;
        string tbfpath = Application.dataPath + "/" + bfpath;

        System.IO.Directory.CreateDirectory(tbfpath);
        File.WriteAllText(tbpath, content);

        AssetDatabase.Refresh();
        // 选中新创建的文件
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets/" + bpath);
        // 打开新创建的文件
        AssetDatabase.OpenAsset(Selection.activeObject);
    }
    bool AppendId()
    {
        string bgidpath = Application.dataPath + "/" + pathbgdata + "/" + folder + preString + ".json";
        string bgcontent = File.ReadAllText(bgidpath);

        List<CardData> list = MyTool.JsonDeserializeObject<List<CardData> >(bgcontent);
        id = list.Count;
        CardData cardData = new CardData
        {
            id = list.Count,
            rawname = bgrowname,
        };
        if (list.Count > 0)
            cardData.configs = list[0].configs;

        string cardDataText = MyTool.JsonSerializeObject(cardData);
        var sb = new StringBuilder();
        using (var reader = new StringReader(cardDataText))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                sb.AppendLine("  " + line); // 每行前加两个空格
            }
        }
        string indentedString = sb.ToString();
        //向列表的最后插入新的元素
        int lastBrace = bgcontent.LastIndexOf('}');
        string indentedInsert = ",\n" + indentedString;

        bgcontent = bgcontent.Insert(lastBrace + 1, indentedInsert);

        File.WriteAllText(bgidpath, bgcontent);

        return true;
    }
    protected virtual void NewAsset()
    {
        string bgfold = id + "_" + bgrowname;
        string tpathres = Application.dataPath + "/" + pathbgres + "/" + folder + preString;
        //创建prefab的文件夹
        System.IO.Directory.CreateDirectory(tpathres + "/" + DataManager.GetIdFolderName(id) + "/" + bgfold);
    }
    void FixAllData()
    {
        foreach(CardField field in Enum.GetValues(typeof(CardField)))
        {
            string bgidpath = Application.dataPath + "/" + pathbgdata + "/" + GetPreString(field) + ".json";
            string bgcontent = File.ReadAllText(bgidpath);

            List<CardData> list = MyTool.JsonDeserializeObject<List<CardData>>(bgcontent);

            foreach(CardData cardData in list)
            {
                //修正每一个数据内容
            }

            bgcontent = MyTool.JsonSerializeObject(list);

            File.WriteAllText(bgidpath, bgcontent);
        }
        ansmsg = "修正完成";
    }
}