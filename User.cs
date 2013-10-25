using UnityEngine;


class User : MonoBehaviour
{
    public delegate void OnSpawnUser(Regulus.Project.Crystal.IUser user);
    public event OnSpawnUser SpawnUserEvent;

    public delegate void OnUnspawnUser(Regulus.Project.Crystal.IUser user);
    public event OnUnspawnUser UnspawnUserEvent;

    internal void Release(Regulus.Project.Crystal.IUser user)
    {
        if (SpawnUserEvent != null)
            SpawnUserEvent(user);
    }

    internal void Initital(Regulus.Project.Crystal.IUser user)
    {
        if (UnspawnUserEvent != null)
            UnspawnUserEvent(user);
    }
}

