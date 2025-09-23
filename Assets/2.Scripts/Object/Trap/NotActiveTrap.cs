using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotActiveTrap : MonoBehaviour
{
    public GameObject outline;
    public GameObject notActiveTrapUI;
    public LayerMask playableLayer; //레이어 선택
    private float _findRange = 4f; //범위

    public void Start()
    {
        outline.SetActive(false);
        notActiveTrapUI.SetActive(true);
    }

    public void Update()
    {
        Collider2D playableSensor = Physics2D.OverlapCircle(transform.position, _findRange, playableLayer);
        if (playableSensor != null) //플레이어가 다가왔을 때
        {
            outline.SetActive(true);
            notActiveTrapUI.SetActive(true);
        }
        if(playableSensor == null)
        {
            outline.SetActive(false);
            notActiveTrapUI.SetActive(false);
        }
    }

    void OnDrawGizmos() //범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _findRange);
    }
}
