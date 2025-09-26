using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeCheck : MonoBehaviour //NotActiveTrap, TreasureChest 등 범위 체크해야되는 오브젝트에 사용
{
    public GameObject outline;
    public GameObject openUIButton; //오브젝트 눌렀을 때 열고싶은 UI(버튼). notActiveTrapUI, ChestUI 등 
    public LayerMask playableLayer; //레이어 선택
    private float _findRange = 4f; //범위

    public void Start() //UI 켜져있으면 전부 끄고 시작 
    {
        outline.SetActive(false);
        openUIButton.SetActive(true);
    }

    public void Update()
    {
        Collider2D playableSensor = Physics2D.OverlapCircle(transform.position, _findRange, playableLayer);
        if (playableSensor != null) //플레이어가 다가왔을 때
        {
            outline.SetActive(true);
            openUIButton.SetActive(true);
        }
        if(playableSensor == null)
        {
            outline.SetActive(false);
            openUIButton.SetActive(false);
        }
    }

    void OnDrawGizmos() //범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _findRange);
    }
}
