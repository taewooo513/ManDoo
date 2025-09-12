using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDatas : DataTable.TestData
{
    public void Test()
    {
        if (TestDataList.Count == 0)
        {
            Load();
        }
        foreach (var Test in TestDataList)
        {
            Debug.Log(Test.id + " " + Test.intValue + " " + Test.strValue);
        }
    }
}
