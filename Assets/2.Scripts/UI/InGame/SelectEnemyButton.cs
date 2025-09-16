using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyButton : MonoBehaviour
{
    private Button button;
    private InGamePlayerUI inGamePlayerUI;
    private Enemy enemy;
    private void Awake()
    {
        inGamePlayerUI = GetComponentInParent<InGamePlayerUI>(); // 테스트용
        button = GetComponent<Button>();
        enemy = transform.GetComponentInParent<Enemy>();
    }

    public void Setting()
    {
        button.onClick.AddListener(OnClickEnemyButton);
    }

    public void OnClickEnemyButton()
    {

    }

    public void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
