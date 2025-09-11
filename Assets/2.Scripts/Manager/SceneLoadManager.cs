using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public static class SceneKey
{
    public const string startScene = "StartScene";
    public const string gameScene1 = "GameScene1";
    public const string endingScene = "EndingScene";
    public const string testScene = "TestScene";
}

public class SceneLoadManager : Singleton<SceneLoadManager>
{ //씬 흐름 관리 클래스
    Dictionary<string, MonoScene> scenes;
    MonoScene nowScene;
    Coroutine asyncLoadScene;
    
    //private GameObject fadeObject; //씬 화면 페이드용(검은색 이미지)
    
    protected override void Awake()
    {
        base.Awake();
        scenes = new Dictionary<string, MonoScene>();
        AddScene(SceneKey.startScene, new StartScene());
        AddScene(SceneKey.gameScene1, new GameScene1());
        AddScene(SceneKey.endingScene, new EndingScene());
        AddScene(SceneKey.testScene, new TestScene());
    }

    public void AddScene(string key, MonoScene monoScene)
    {
        if (scenes.TryGetValue(key, out MonoScene scene))
        {
            Debug.Log($"{key} is duplicate in scene");
            return;
        }
        scenes.Add(key, monoScene);
    }

    public void LoadScene(string key)
    {
        if (asyncLoadScene != null) StopCoroutine(asyncLoadScene);

        if (scenes.TryGetValue(key, out MonoScene scene))
        {
            asyncLoadScene = StartCoroutine(AsyncLoadScene(key));
            return;
        }
        Debug.Log($"Not Find {key} in Objects");
    }

    IEnumerator AsyncLoadScene(string key)
    {
        if (nowScene != null)
        {
            nowScene.Release();
        }
        nowScene = scenes[key];
        AsyncOperation operation = SceneManager.LoadSceneAsync(key);
        
        operation.allowSceneActivation = false;

        var loadHandlePrefab = nowScene.LoadPrefabs();
        var loadHandleSound = nowScene.LoadSounds();

        while (loadHandlePrefab != null && loadHandleSound != null && (!loadHandlePrefab.Value.IsDone || !loadHandleSound.Value.IsDone))
        {
            yield return null;
        }

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        nowScene.Init();
    }
}