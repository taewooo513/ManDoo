using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class MonoScene
{ //씬 로드할 때 필요한 init, release 등이 겹쳐서 상속시켜주기 위한 부모 클래스
    public int stage;
    public abstract AsyncOperationHandle? LoadPrefabs();
    public abstract AsyncOperationHandle? LoadSounds();
    
    //public abstract void OnPadeOut();
    public abstract void Init(); //초깃값 설정용
    
    public abstract void Release(); //필요 없는거 삭제하는 용
}