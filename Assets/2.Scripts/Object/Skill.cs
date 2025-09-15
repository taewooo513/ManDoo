using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private int _id;
    public void Init(int id)
    {
        _id = id;
    }

    public void UseSkill(ArrayList attacker)
    {
        DataManager.Instance.Skill.GetSkillData(_id);
    }
}
