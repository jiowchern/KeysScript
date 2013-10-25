using UnityEngine;
using System.Collections;



public class CoreFramework : MonoBehaviour, Regulus.Utility.Console.IViewer, Regulus.Utility.Console.IInput
{
    Regulus.Project.Crystal.Application _Application;
    
    private string _Input = "";

    public GameObject LogoStage;
    public GameObject VerifyStage;
    public GameObject ParkingStage;
    public GameObject AdventureStage;
    public GameObject BattleStage;

    GameObject _CurrentStage;
    System.Collections.Generic.List<Regulus.Project.Crystal.IUser> _Users ;
    Regulus.Project.Crystal.IUser _CurrentUser;
    public CoreFramework()
    {
        _Users = new System.Collections.Generic.List<Regulus.Project.Crystal.IUser>();
    }

    ~CoreFramework()
    {

    }
    

	void Start () 
    {
        var app = new Regulus.Project.Crystal.Application(this, this);        
        _Application = app;        
        
        _Application.UserSpawnEvent += _OnSpawnUser;
        _Application.UserUnspawnEvent += _OnUnspawnUser;
        (_Application as Regulus.Game.IFramework).Launch();        
	}
    
    private void _OnSpawnUser(Regulus.Project.Crystal.IUser user)
    {
        _Users.Add(user);

        _Application.Command.Register("Lookup"  + user.GetHashCode() ,  () =>
        {
            _LookupUser(user);
        });
    }

    void _OnUnspawnUser(Regulus.Project.Crystal.IUser user)
    {
        _Application.Command.Unregister("Lookup" + user.GetHashCode());
        _Users.Remove(user);
    }

    private void _LookupUser(Regulus.Project.Crystal.IUser user)
    {
        user.StatusProvider.Supply += _OnGameStatus;
        
    }

    void _OnGameStatus(Regulus.Project.Crystal.IUserStatus obj)
    {
        obj.StatusEvent += _OnChangeStatus;
    }

    

    void _OnChangeStatus(Regulus.Project.Crystal.UserStatus obj)
    {
        GameObject[] stages = new GameObject[]{LogoStage , VerifyStage , ParkingStage , AdventureStage , BattleStage} ;
        var stage = GameObject.Instantiate(stages[(int)obj]) as GameObject;        
        _ChangeStage(stage);
        Debug.Log("切換場景" + obj.ToString());
    }

    void _ChangeStage(GameObject new_stage )
    {        
        if (_CurrentStage != null)
        {
            _RestroyStage();
        }
        _CreateStage(new_stage);
    }

    private void _CreateStage(GameObject new_stage)
    {
        _CurrentStage = new_stage;
        var user = _CurrentStage.GetComponent<User>();
        user.Initital(_CurrentUser);
    }

    private void _RestroyStage()
    {
        var user = _CurrentStage.GetComponent<User>();
        user.Release(_CurrentUser);
        GameObject.Destroy(_CurrentStage);
    }    

    System.Collections.Generic.Queue<string> _List = new System.Collections.Generic.Queue<string>();
	// Update is called once per frame
    bool _ShowConsole = false;
	void Update () 
    {
        (_Application as Regulus.Game.IFramework).Update();

        if (Input.GetKeyUp(KeyCode.BackQuote))
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

