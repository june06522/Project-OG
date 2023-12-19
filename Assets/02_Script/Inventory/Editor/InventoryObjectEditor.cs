using FD.Core.Editors;
using FD.Dev.AI;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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

    }

}


public class InventoryEditorGraph : FAED_BaseGraphView
{



}

internal class InvenVisualWindow : VisualElement
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

    public void HandleCreateInspactor(Object obj)
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