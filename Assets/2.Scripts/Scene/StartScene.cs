using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StartScene : BaseScene
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
        //Addressables.Release(함수이름);
    }
}
