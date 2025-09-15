using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;

public class Skill : MonoBehaviour
{
    private SkillData sd;

    public void Init(int id)
    {
        sd = DataManager.Instance.Skill.GetSkillData(id);
    }

    public void UseSkill(ArrayList attacker)
    {
        
    }
}
