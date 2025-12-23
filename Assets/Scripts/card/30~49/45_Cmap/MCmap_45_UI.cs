using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static SCmap_45;

public class MCmap_45_UI : MonoBehaviour,IEscClose
{
    public static MCmap_45_UI instance = null;// 再次使用按键关闭地图而不是再开一个
    public TextMeshProUGUI title;
    public RectTransform content;
    public GoodUI nowEnv;

    const float xSize = 200;
    const float ySize = 200;
    const float viewHeight = 200;
    private Vector3 GetPos(int height, int index, int indexMax)
    {
        return new Vector3(xSize * (0.5f + index - indexMax / 2f), ySize * (height + 0.5f) + viewHeight / 2);
    }
    private void GiveLink(Vector3 from, Vector3 to, Color color)
    {
        // Debug.Log($"from:{from} to:{to}");
        GameObject hl = UIBasic.GiveUI(DCmap_45.linkLineUIPrefab);
        hl.transform.SetParent(content.transform, false);
        RectTransform rectTransform = hl.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = from;

        Vector2 direction = to - from;
        float distance = direction.magnitude;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, distance);

        // 计算旋转角度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        rectTransform.localEulerAngles = new Vector3(0, 0, angle);

        hl.GetComponent<UnityEngine.UI.Image>().color = color;
    }
    public void Set(CCmap_45 cmap)
    {
        nowEnv.SetCard(cmap.nowEnv);

        title.text = "当前层数 " + cmap.mapCnt;
        
        float width = content.sizeDelta.x;
        float height = ySize * (cmap.mapNodes.Count) + viewHeight;
        content.sizeDelta = new Vector2(width, height);

        // 先画道路再画点来实现覆盖
        for (int i = 0; i < cmap.mapNodes.Count - 1; i++)
        {
            List<MapNode> list = cmap.mapNodes[i];
            List<MapNode> nextList = cmap.mapNodes[i+1];
            for (int j = 0; j < list.Count; j++)
            {
                MapNode mapNode = list[j];
                Vector3 pos = GetPos(i, j, list.Count);
                Color color = Color.gray;
                if (i == cmap.nowHeight && j == cmap.nowIndex)
                {
                    // 是当前的节点，额外高亮显示
                    color = new Color(0.4f, 1, 0.4f);
                }

                // 根据道路连线新建线路的显示
                for(int k=0;k<mapNode.links.Count;k++)
                {
                    GiveLink(pos, GetPos(i + 1, mapNode.links[k], nextList.Count), color);
                }
            }
        }
        
        for (int i=0;i<cmap.mapNodes.Count;i++)
        {
            List<MapNode> list = cmap.mapNodes[i];
            for(int j=0;j<list.Count;j++)
            {
                MapNode mapNode = list[j];
                Vector3 pos = GetPos(i, j, list.Count);
                if (i == cmap.nowHeight && j == cmap.nowIndex)
                {
                    // 是当前的节点，额外高亮显示
                    GameObject hl = UIBasic.GiveUI(DCmap_45.highLightUIPrefab);
                    hl.transform.SetParent(content.transform, false);
                    hl.transform.localScale *= mapNode.size;
                    hl.GetComponent<RectTransform>().anchoredPosition = pos;

                    // 将地图移动对齐当前节点  地图的横线为0时对齐中心height/2处
                    content.anchoredPosition = new Vector2(0, -pos.y + height / 2);
                }
                // 根据当前的类型放上对应的ID的物品作为展示
                GameObject obj = UIBasic.GiveUI(DCmap_45.mapGoodUIPrefab);
                obj.transform.SetParent(content.transform, false);
                if (mapNode.isEnv || mapNode.showDetail)
                    obj.GetComponent<GoodUI>().SetCard(CardManager.CreateCard(mapNode.id));
                else
                    obj.GetComponent<GoodUI>().SetCard(CardManager.CreateCard(SfloorBase_2.GetFloorShow(mapNode.id).showID));
                    
                obj.GetComponent<RectTransform>().anchoredPosition = pos;
                obj.transform.localScale *= mapNode.size;
            }
        }
    }

    private void OnEnable()
    {
        SControl_3.AddNoControl(1);
        SControl_3.AddEscClose(this);
        instance = this;
    }
    private void OnDisable()
    {
        SControl_3.AddNoControl(-1);
        SControl_3.RemoveEscClose(this);
    }
    public void Close()
    {
        Destroy(gameObject);
        instance = null;
    }
}
