using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardTreeViewer : EditorWindow
{
    public CardBase rootCard; // 树的根节点
    private Vector2 scrollPosition;

    [MenuItem("My测试/显示CardTree")]
    public static void ShowWindow()
    {
        GetWindow<CardTreeViewer>("Card Tree Viewer");
    }
    private void OnEnable()
    {
        // 注册 EditorApplication.update 回调
        EditorApplication.update += UpdateWindow;
    }

    private void OnDisable()
    {
        // 取消注册 EditorApplication.update 回调
        EditorApplication.update -= UpdateWindow;
    }

    private void UpdateWindow()
    {
        for(int i=0;i<unfold.Count;i++)
        {
            if (unfold[i].valid <= 0)
            {
                unfold.RemoveAt(i);
                i--;
            }
        }
        // 更新窗口重绘
        Repaint();
    }

    private void OnGUI()
    {
        if(!EditorApplication.isPlaying)
        {
            EditorGUILayout.LabelField("游戏未运行", EditorStyles.boldLabel);
            return;
        }
        rootCard = CardManager.GetAllCards();
        EditorGUILayout.LabelField("Card Tree", EditorStyles.boldLabel);

        if (rootCard != null)
        {
            if (rootCard.components != null)
            {
                foreach (var component in rootCard.components)
                {
                    DrawCardTree(component, 0, rootCard);
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("没有根节点，出问题了.", MessageType.Info);
        }
    }
    CardBase showDetail;
    List<CardBase> unfold = new List<CardBase>();

    private void DrawCardTree(CardBase card, int indentLevel, CardBase parent)
    {
        var richTextStyle = new GUIStyle(EditorStyles.label)
        {
            richText = true,
        };

        if (card == null)
        {
            EditorGUI.indentLevel = indentLevel;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("<color=#EE0000>空节点</color>", richTextStyle);

            EditorGUILayout.EndHorizontal();
            return;
        }

        bool unfolded = unfold.Contains(card);

        EditorGUI.indentLevel = indentLevel;

        EditorGUILayout.BeginHorizontal();

        DataManager.PidToVid(card.id, out int vid, out CardField field);
        string text = $"VID: {MyEditorBase.GetHeadString(field)}{vid} | {DataManager.GetName(card.id)}";
        if (card.parent.components == null || !card.parent.components.Contains(card)) text = "<color=#06B300>A</color> " + text;
        if (indentLevel == 0) text = "<color=#7FD6FC>" + text + "</color>";
        else if (card.parent != parent) text = "<color=#EE0000>脱离的节点：" + text + " parent:" + card.parent + "</color>";
        if (card.activeComponents != null && card.activeComponents.Count > 0) text = (unfolded ? "▼" : "▶") + text;
        EditorGUILayout.LabelField(text, richTextStyle);

        EditorGUILayout.EndHorizontal();
        // 获取文字的区域
        Rect lastRect = GUILayoutUtility.GetLastRect();

        // 检测鼠标点击事件
        if (Event.current.type == EventType.MouseDown && lastRect.Contains(Event.current.mousePosition))
        {
            if(Event.current.button==1)
            {
                if (showDetail != card)
                    showDetail = card;
                else showDetail = null;
            }
            else if(Event.current.button==0)
            {
                if (unfolded) unfold.Remove(card);
                else unfold.Add(card);
                unfolded = !unfolded;
            }
            Event.current.Use(); // 消耗事件，避免其他控件响应
        }

        // 显示详细信息
        if (showDetail == card)
        {
            EditorGUILayout.HelpBox(DataManager.GetDescirbe(card), MessageType.Info);
        }

        if (unfolded && card.activeComponents != null)
        {
            foreach (var component in card.activeComponents)
            {
                DrawCardTree(component, indentLevel + 1, card);
            }
        }
    }
}
