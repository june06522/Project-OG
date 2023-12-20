using FD.Core.Editors;
using FD.Dev.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UIElements;
using static FD.Core.Editors.FAED_BehaviorTreeEditorWindow;

/// <summary>
/// 
/// </summary>
public class InventoryEditorWindow : FAED_GraphBaseWindow<InventoryEditorGraph>
{

    private InvenVisualWindow inspactor;
    private VisualElement graphRoot;

    [MenuItem("Inventory/Editor")]
    private static void OpenEditor()
    {

        var window = CreateWindow<InventoryEditorWindow>();
        window.titleContent.text = "InvenEditor";

        window.Show();
        window.maximized = true;

    }

    private void SetUpGraph()
    {

        graphView.SetDrag();
        graphView.SetGrid();
        graphView.SetZoom();
        graphView.style.position = Position.Relative;
        graphView.SetMiniMap(new Rect(10, 10, 300, 300));

    }

    private void SetUpInspacter()
    {

        inspactor = new InvenVisualWindow("Inspactor", Position.Relative, new Color(0.3f, 0.3f, 0.3f));

    }

    private void SetUpSplit()
    {

        var split = new TwoPaneSplitView(1, 300, TwoPaneSplitViewOrientation.Horizontal);
        split.contentContainer.Add(graphView);
        split.contentContainer.Add(inspactor);
        graphRoot.Add(split);

    }

    private void SetUpToolBar()
    {
    }

    protected override void OnEnable()
    {

        base.OnEnable();
        AddToolBar();

        graphRoot = new VisualElement();
        graphRoot.style.flexGrow = 1;
        rootVisualElement.Add(graphRoot);

        SetUpGraph();
        SetUpInspacter();
        SetUpSplit();
        SetUpToolBar();

        graphView.Init(inspactor);

    }

}


public class InventoryEditorGraph : FAED_BaseGraphView
{

    private InvenVisualWindow inspactor;
    private InvenSearchWindow searchWindow;

    public void Init(InvenVisualWindow inspactor)
    {
        this.inspactor = inspactor;
        searchWindow = ScriptableObject.CreateInstance<InvenSearchWindow>();
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);

    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {

        return ports.ToList().Where(
            x => x.direction != startPort.direction && 
            x.node != startPort.node && 
            x.portType == startPort.portType).ToList();

    }


    private class InvenSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private InventoryEditorGraph graph;

        public void Init(InventoryEditorGraph graph)
        {

            this.graph = graph;

        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {

            var tree = new List<SearchTreeEntry>()
            {

                new SearchTreeGroupEntry(new GUIContent("원준이는 원준이가 아니라 원준이입니다"), 0),
                new SearchTreeGroupEntry(new GUIContent("Create Event Node"), 1),

            };

            var types = TypeCache.GetTypesDerivedFrom<InventoryEventReceiver>();

            foreach (var t in types)
            {

                if (t.IsAbstract) continue;

                tree.Add(new SearchTreeEntry(new GUIContent(t.Name))
                {

                    userData = t,
                    level = 2

                });

            }



            return tree;

        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {

            if (SearchTreeEntry.userData as Type != null)
            {

                var type = SearchTreeEntry.userData as Type;

                if (type.IsSubclassOf(typeof(InventoryEventReceiver)))
                {

                    var node = new EventReveiveNode(type);

                    graph.AddInvenNode(node);
                    
                }

                return true;

            }

            return false;

        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    public void AddInvenNode(InventoryBaseNode node)
    {

        node.OnSelectedEvent += inspactor.HandleCreateInspactor;
        AddElement(node);

    }


}

public class InvenVisualWindow : VisualElement
{

    private Editor editor;

    public VisualElement titleContainer { get; protected set; }
    public Label titleLabel { get; protected set; }
    public VisualElement guiContainer { get; protected set; }

    public InvenVisualWindow(string text, Position position, Color backGroundColor)
    {

        style.backgroundColor = backGroundColor;
        style.position = position;
        style.flexGrow = 1;

        CreateTitleContainer();

        titleContainer = new VisualElement();
        titleContainer.style.position = Position.Relative;
        titleContainer.style.backgroundColor = Color.black;
        titleContainer.style.flexShrink = 0;

        titleLabel = new Label(text);
        titleLabel.style.position = Position.Relative;
        titleLabel.style.fontSize = 24;

        titleContainer.Add(titleLabel);

        guiContainer = new VisualElement();
        guiContainer.style.position = Position.Relative;
        guiContainer.style.flexGrow = 1;

        Add(titleContainer);
        Add(guiContainer);


    }

    private void CreateTitleContainer()
    {

        titleContainer = new VisualElement();
        titleContainer.style.position = Position.Relative;
        titleContainer.style.backgroundColor = Color.black;

        Add(titleContainer);

    }

    public void HandleCreateInspactor(UnityEngine.Object obj)
    {

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(obj);
        guiContainer.Clear();

        var imgui = new IMGUIContainer(() =>
        {

            if (editor.target)
            {

                editor.OnInspectorGUI();

            }

        });

        imgui.style.flexGrow = 1;

        guiContainer.Add(imgui);


    }

}