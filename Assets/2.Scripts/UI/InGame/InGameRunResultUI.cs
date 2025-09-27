using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameRunResultUI : UIBase
{
    [SerializeField] private GameObject playerIconSlot; //프리팹
    [SerializeField] private RectTransform content; //프리팹 생성할 위치
    public Button returnBtn; //처음으로 돌아가는 버튼

    public void Start()
    {
        returnBtn.onClick.AddListener(OnClickBtn);
    }

    public void OnClickBtn()
    {
        UIManager.Instance.CloseUI<InGameRunResultUI>();
        //처음으로 돌아가기
    }
}
