using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
{
    private PlayableCharacter player;
    private Button button;
    private InGamePlayerUI inGamePlayerUI;
    public int index;
    private void Awake()
    {
        inGamePlayerUI = GetComponentInParent<InGamePlayerUI>(); // 테스트용
        player = GetComponentInParent<PlayableCharacter>();
        
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickPlayerButton);
    }

    public void OnClickPlayerButton()
    {
    }

    public void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}