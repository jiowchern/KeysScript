using UnityEngine;
using System.Collections;

public class CoreFramework : MonoBehaviour {

    Regulus.Project.Crystal.IUser _User;
	// Use this for initialization
	void Start () 
    {
        _User = new Regulus.Project.Crystal.Standalone.User();
        _User.VerifyProvider.Supply += _GetVerify;

	}

    void _GetVerify(Regulus.Project.Crystal.IVerify obj)
    {
        
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
