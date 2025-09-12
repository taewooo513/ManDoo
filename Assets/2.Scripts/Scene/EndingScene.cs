using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EndingScene : MonoScene
{
    public override AsyncOperationHandle? LoadPrefabs()
    {
        return null;
    }

    public override AsyncOperationHandle? LoadSounds()
    {
        return null;
    }

    public override void Init()
    {
    }

    public override void Release()
    {
    }
}
