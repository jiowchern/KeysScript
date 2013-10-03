using UnityEngine;
using System.Collections;


public class CoreFramework : MonoBehaviour, Regulus.Utility.Console.IViewer, Regulus.Utility.Console.IInput
{
    Regulus.Game.IFramework _Application;
    private string _Input = "";

    public GameObject LogoStage;
    public GameObject VerifyStage;
    public GameObject ParkingStage;
    public GameObject AdventureStage;
    public GameObject BattleStage;

    public GameObject _CurrentStage;
	void Start () 
    {
        var app = new Regulus.Project.Crystal.Application(this, this);        
        _Application = app;
        _Application.Launch();

        _ShowLogoStage();
	}

    void _ChangeStage(GameObject new_stage )
    {
        if (_CurrentStage != null)
        {
            GameObject.Destroy(_CurrentStage);
        }
        _CurrentStage = new_stage;
    }
    private void _ShowLogoStage()
    {
        /*var stage = GameObject.Instantiate(LogoStage) as GameObject;
        //stage.GetComponent<CreateSystem>();
        _ChangeStage(stage);*/
    }

    System.Collections.Generic.Queue<string> _List = new System.Collections.Generic.Queue<string>();
	// Update is called once per frame
    bool _ShowConsole = false;
	void Update () 
    {
        _Application.Update();

        if (Input.GetKeyUp(KeyCode.A))
        {
            _ShowConsole = !_ShowConsole;
        }
	}

    void OnGUI()
    {
        if (_ShowConsole)
        {
            GUILayout.BeginVertical();

            foreach (var message in _List)
            {
                GUILayout.Label(message);
            }
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            _Input = GUILayout.TextField(_Input);
            if (GUILayout.Button("輸入"))
            {
                _OutputEvent(_Input.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries));
                _Input = "";
            }
            GUILayout.EndHorizontal();
        }
        
    }

    void Regulus.Utility.Console.IViewer.Write(string message)
    {
        
    }

    void Regulus.Utility.Console.IViewer.WriteLine(string message)
    {
        _List.Enqueue(message);
        if (_List.Count > 25)
        {
            _List.Dequeue();
        }
    }

    event Regulus.Utility.Console.OnOutput _OutputEvent;
    event Regulus.Utility.Console.OnOutput Regulus.Utility.Console.IInput.OutputEvent
    {
        add { _OutputEvent += value; }
        remove { _OutputEvent -= value; }
    }
}

