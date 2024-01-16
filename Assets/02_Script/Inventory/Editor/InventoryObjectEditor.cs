using FD.Core.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryEditorWindow : FAED_GraphBaseWindow<InventoryEditorGraph>
{

    private static InventoryObjectData opend;

    private InvenVisualWindow inspactor;
    private VisualElement graphRoot;

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

        inspactor = new InvenVisualWindow("Inspector", Position.Relative, new Color(0.3f, 0.3f, 0.3f));

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

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {

        if (Selection.activeObject is InventoryObjectData)
        {

            opend = Selection.activeObject as InventoryObjectData;

            OpenEditor();
            return true;

        }

        return false;

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

        graphView.Init(inspactor, opend);

    }

    private void OnDisable()
    {

        graphView.Release();
        opend = null;

    }

}


public class InventoryEditorGraph : FAED_BaseGraphView
{

    private InvenVisualWindow inspactor;
    private InvenSearchWindow searchWindow;
    private InventoryObjectData scriptObjectRoot;

    public void Init(InvenVisualWindow inspactor, InventoryObjectData scriptObjectRoot)
    {

        this.inspactor = inspactor;
        this.scriptObjectRoot = scriptObjectRoot;

        searchWindow = ScriptableObject.CreateInstance<InvenSearchWindow>();
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        searchWindow.Init(this);

        foreach(var obj in scriptObjectRoot.includes)
        {

            if (obj.GetType().IsSubclassOf(typeof(InventoryEventReceiverBase)))
            {

                AddInvenNodeNotCreate(new EventReveiveNode(obj));

            }
            else if (obj.GetType().IsSubclassOf(typeof(InventoryConverterBase)))
            {

                AddInvenNodeNotCreate(new ConverterNode(obj));

            }
            else if (obj.GetType().IsSubclassOf(typeof(InventoryExecuterBase)))
            {

                AddInvenNodeNotCreate(new ExecuterNode(obj));

            }

        }

        foreach(var node in nodes)
        {

            var pos = (node as InventoryBaseNode).invenObj.editorPos;

            node.style.left = pos.x;
            node.style.top = pos.y;

            ConnectedNode(node as InventoryBaseNode);

        }


        graphViewChanged += HandleGraphViewChanged;

    }

    private void ConnectedNode(InventoryBaseNode node)
    {

        var nodeTable = nodes.Select(x => x as InventoryBaseNode).ToList();

        foreach(var cnode  in node.invenObj.connectedOutput)
        {

            var connectNode = nodeTable.Find(x => x.invenObj.guid == cnode.guid);

            var edge = node.outputContainer.Q<Port>().ConnectTo(connectNode.inputContainer.Q<Port>());
            AddElement(edge);

        }


        foreach (var cnode in node.invenObj.connectedInput)
        {

            var connectNode = nodeTable.Find(x => x.invenObj.guid == cnode.guid);

            var edge = node.inputContainer.Q<Port>().ConnectTo(connectNode.outputContainer.Q<Port>());
            AddElement(edge);

        }

    }

    private GraphViewChange HandleGraphViewChanged(GraphViewChange graphViewChange)
    {

        if (graphViewChange.elementsToRemove != null)
        {

            foreach(var removed in graphViewChange.elementsToRemove)
            {

                var node = removed as InventoryBaseNode;
                
                if (node != null)
                {

                    scriptObjectRoot.includes.Remove(node.invenObj);

                    EditorUtility.SetDirty(scriptObjectRoot);
                    AssetDatabase.SaveAssets();

                    EditorUtility.SetDirty(node.invenObj);
                    AssetDatabase.SaveAssets();

                    node.Delete();

                }

                var edge = removed as Edge;

                if(edge != null)
                {

                    var parent = edge.output.node as InventoryBaseNode; //신호 보내는놈
                    var child = edge.input.node as InventoryBaseNode; //신호 받는놈

                    parent.invenObj.Disconnect(child.invenObj, ConnectType.Output);
                    child.invenObj.Disconnect(parent.invenObj, ConnectType.Input);

                    EditorUtility.SetDirty(parent.invenObj);
                    AssetDatabase.SaveAssets();

                    EditorUtility.SetDirty(child.invenObj);
                    AssetDatabase.SaveAssets();

                }

            }

        }

        if (graphViewChange.edgesToCreate != null)
        {

            foreach(var edge in graphViewChange.edgesToCreate)
            {

                var parent = edge.output.node as InventoryBaseNode; //신호 보내는놈
                var child = edge.input.node as InventoryBaseNode; //신호 받는놈

                parent.invenObj.Connect(child.invenObj, ConnectType.Output);
                child.invenObj.Connect(parent.invenObj, ConnectType.Input);

                EditorUtility.SetDirty(parent.invenObj);
                AssetDatabase.SaveAssets();

                EditorUtility.SetDirty(child.invenObj);
                AssetDatabase.SaveAssets();

            }
            
        }

        AssetDatabase.SaveAssets();

        return graphViewChange;

    }

    public void Release()
    {

        EditorUtility.SetDirty(scriptObjectRoot);
        AssetDatabase.SaveAssets();

        graphViewChanged -= HandleGraphViewChanged;

    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {

        return ports.ToList().Where(
            x => x.direction != startPort.direction && 
            x.node != startPort.node && 
            (x.portType == startPort.portType || x.portType == typeof(object))).ToList();

    }

    public void AddInvenNode(InventoryBaseNode node)
    {

        node.OnSelectedEvent += inspactor.HandleCreateInspactor;

        scriptObjectRoot.includes.Add(node.invenObj);

        AssetDatabase.AddObjectToAsset(node.invenObj, scriptObjectRoot);
        EditorUtility.SetDirty(scriptObjectRoot);
        AssetDatabase.SaveAssets();

        AddElement(node);

    }

    public void AddInvenNodeNotCreate(InventoryBaseNode node)
    {

        node.OnSelectedEvent += inspactor.HandleCreateInspactor;
        AddElement(node);

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

                new SearchTreeGroupEntry(new GUIContent("Node Window"), 0),
                new SearchTreeGroupEntry(new GUIContent("Create Event Node"), 1),

            };

            var types = TypeCache.GetTypesDerivedFrom<InventoryEventReceiverBase>();

            foreach (var t in types)
            {

                if (t.IsAbstract) continue;

                tree.Add(new SearchTreeEntry(new GUIContent(t.Name))
                {

                    userData = t,
                    level = 2

                });

            }


            tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Converter Node"), 1));

            types = TypeCache.GetTypesDerivedFrom<InventoryConverterBase>();

            foreach (var t in types)
            {

                if (t.IsAbstract) continue;

                tree.Add(new SearchTreeEntry(new GUIContent(t.Name))
                {

                    userData = t,
                    level = 2

                });

            }

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Executer Node"), 1));

            types = TypeCache.GetTypesDerivedFrom<InventoryExecuterBase>();

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

                if (type.IsSubclassOf(typeof(InventoryEventReceiverBase)))
                {

                    var node = new EventReveiveNode(type);

                    graph.AddInvenNode(node);

                    node.transform.position = graph.ChangeCoordinatesTo(node, context.screenMousePosition);
                    
                }
                else if (type.IsSubclassOf(typeof(InventoryConverterBase)))
                {

                    var node = new ConverterNode(type);

                    graph.AddInvenNode(node);

                    node.transform.position = graph.ChangeCoordinatesTo(node, context.screenMousePosition);

                }
                else if (type.IsSubclassOf(typeof(InventoryExecuterBase)))
                {

                    var node = new ExecuterNode(type);

                    graph.AddInvenNode(node);

                    node.transform.position = graph.ChangeCoordinatesTo(node, context.screenMousePosition);

                }

                return true;

            }

            return false;

        }

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