using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class TreasureRoom : BattleTreasureEvent
{
    private int _rewardId; //실제로 주는 보상id
    protected int battleRewardGroupId; //배틀데이터에 있는 그룹 아이디
    protected int rewardGroupId; //보상 테이블 연결해주는 id
    protected List<RewardData> rewardIdList; //그룹에 속한 id 리스트
    List<int> itemIdList = new(); //보상id 리스트
    List<int> itemCountList = new(); //보상 개수 리스트

    public override void Init(int id)
    {
        base.Init(id);
        battleRewardGroupId = battleData.rewardId;
        rewardGroupId = rewardData.groupId; //랜덤가챠 돌릴 범위
        rewardIdList = DataManager.Instance.Reward.GetRewardGroupList(rewardGroupId); //보상 그룹 가져오기
        Rewarded(); //방에 들어왔을 때 보상 리스트 1개로 결정됨
        OnEventEnded(); //상자를 열어도 되고, 안 열어도 되니 바로 버튼 활성화되도록 함
    }

    public override void EnterRoom()
    {
        base.EnterRoom(); //플레이어 소환(위치 선정)
    }

    public void Rewarded() //보상id 랜덤으로 뽑고, 보상 ui에 넣어주는 함수.
    {
        List<float> dropProbWeightList = new(); //가중치 리스트
        
        if (battleRewardGroupId == rewardGroupId) //그룹 아이디가 같을 때
        {
            for (int i = 0; i < rewardIdList.Count; i++) //id 개수만큼 돌리면서 
            {
                dropProbWeightList.Add(rewardIdList[i].dropProb); //인덱스 순으로 드랍 확률(가중치) 추가하기
            }
            _rewardId = RandomizeUtility.TryGetRandomPlayerIndexByWeight(dropProbWeightList); //가중치 돌려서 보상주는 방 id 뽑기
        }

        if (rewardIdList[_rewardId].itemIdList.Count == rewardIdList[_rewardId].itemCount.Count) //아이템 리스트와 아이템 개수가 같을 때
        {
            itemIdList.AddRange(rewardIdList[_rewardId].itemIdList); //보상 아이템 리스트에 한 번에 넣기
            itemCountList.AddRange(rewardIdList[_rewardId].itemCount);
        }
        
        ItemManager.Instance.AddReward(eItemType.Consumable, itemIdList, itemCountList); //보상 UI에 추가해주기
    }

    public void IsOpen(int id, bool isOpen)
    {
        if (roomId == id)
        {
            isInteract = isOpen; //상자랑 상호작용 시, 상자open 상태 = true
        }
    }

    public override void ExitRoom()
    {
        if (isInteract) //열었던 상자라면
        {
            Clear(); //보상 id값들 넣어뒀던 리스트 비우기 (그룹id 안에 속한 id들 리스트)
        }
        //보물상자 안 열었던 경우에는 그대로 남아있음. (x를 누르면 아예 사라짐)
    }
    
    public void Clear() //리스트 내용 비워주기
    {
        rewardIdList.Clear();
        itemIdList.Clear();
        itemCountList.Clear();
    }
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground";
    }
}
