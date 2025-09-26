using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : BaseRoom
{
    public override void EnterRoom(int id)
    {
        base.EnterRoom(id); //플레이어 소환(위치 선정)
        Init(id);
    }

    public void Rewarded() //todo : 상자쪽에서 호출해야 됨
    {
        List<float> dropProbWeightList = new();
        if (battleRewardGroupId == rewardGroupId) //그룹 아이디가 같을 때
        {
            for (int i = 0; i < rewardIdList.Count; i++) //id 개수만큼 돌리면서 
            {
                dropProbWeightList.Add(rewardIdList[i].dropProb); //인덱스 순으로 드랍 확률(가중치) 추가하기
            }
            int rewardRoomId = RandomizeUtility.TryGetRandomPlayerIndexByWeight(dropProbWeightList); //가중치 돌려서 보상주는 방 id 뽑기
        }
        Clear(); //보상 id값들 넣어뒀던 리스트 비우기
    }

    public override void ExitRoom(int id)
    {
    }
}
