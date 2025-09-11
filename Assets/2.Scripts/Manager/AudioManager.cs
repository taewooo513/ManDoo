using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : Singleton<AudioManager>
{
    Dictionary<string, AudioClip> sounds;
    AsyncOperationHandle soundHandle;

    protected override void Awake()
    {
        base.Awake();
        sounds = new Dictionary<string, AudioClip>();
    }

    public void InsertSound(string key, AudioClip audioClip)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            Debug.Log($"{key} is duplicate in sounds");
            return;
        }
        sounds.Add(key, clip);
    }

    public AsyncOperationHandle LoadSound(string label)
    {
        var handle = Resource.Instance.LoadResource<AudioClip>(label, clip =>
        {
            sounds.Add(clip.name, clip);
        });
        handle.Completed += OnLoadCompleteObject;
        soundHandle = handle;
        return soundHandle;
    }
    private void OnLoadCompleteObject<T>(AsyncOperationHandle<IList<T>> handle) where T : UnityEngine.Object
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Load Sounds Succeeded");
        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("Load Sounds Failed");
        }
    }
    public override void Release()
    {
        sounds.Clear();
        Addressables.Release(soundHandle);
    }
}
