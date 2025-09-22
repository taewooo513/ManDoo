using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    //public List<ItemData> itemData = new List<ItemData>();

    private void Awake()
    {
        
    }

    public Sprite GetIcon(Item item)
    {
        if (item == null) return null;
        return item.icon;
    }
    
    public bool CanUseItem(Item item)
    {
        if (item == null) return false;
        
        // TODO: 나중에 타입/효과 데이터 테이블에서 받아솨서 switch 문으로 작성
        
        return false;
    }

    public bool CanSwapItem(Item fromItem, int from, int to)
    {
        // TODO: 장착
        return true;
    }

    public void SwapItem(Item[] items, int from, int to)
    {
        // 아이템 배열이 null인 경우 스왑 취소
        if (items == null) return;
        // from 인덱스가 배열 범위를 벗어나면 스왑 취소  
        if ((uint)from >= items.Length) return;
        // to 인덱스가 배열 범위를 벗어나면 스왑 취소
        if ((uint)to >= items.Length) return;
        // 튜플을 사용한 두 아이템의 위치 교환
        (items[to], items[from]) = (items[from], items[to]);
    }
}
