using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    private int id;
    private MercenaryData data;

    private void SetData()
    {
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);
    }

    public void init(int idx)
    {
        id = idx;
        SetData();
    }
    public override void Attack(int index)
    {
        base.Attack(index);
    }
}
