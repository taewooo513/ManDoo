using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : BaseRoom
{
    private int _rewardId; //실제로 보상 주는 방 id
    public bool isOpen = false;
    //이거 이렇게하면 보상 상자가 있는 룸이 여러개일 때, A룸에서 상자 열고 템먹고, B룸에서 상자 안 열고 지나갔는데, A룸에서 상자 아이템 남아있는 경우가 될 수도 있나?
    //TODO : => 각자 별개의 인스턴스로 생성하면 해결됨. (ex : TreasureRoom roomA = new TreasureRoom();) 주의하기.
    
    public override void EnterRoom(int id) //todo : 통로/룸 관리하는 쪽에서 2001로 넣어줘야 됨
    { //방 호출할 때마다(다시 찾아올때도) enterRoom 부르는건지, 아니면 한 번만 부르고 이후는 계속 다른곳에 저장해놓고 있는건지 물어보기 
        base.EnterRoom(id); //플레이어 소환(위치 선정)
        Init(id);
        Rewarded(); //방에 들어왔을 때 보상 리스트 1개로 결정됨
    }

    public void Rewarded() //보상id 랜덤으로 뽑고, 보상 ui에 넣어주는 함수.
    {
        List<float> dropProbWeightList = new();
        List<int> itemIdList = new();
        List<int> itemCountList = new();
        
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

    public bool IsOpen(bool isOpen)
    {
        return isOpen;
    }

    public override void ExitRoom(int id)
    {
        if (isOpen) //열었던 상자라면
        {
            Clear(); //보상 id값들 넣어뒀던 리스트 비우기 (그룹id 안에 속한 id들 리스트)
        }
        //보물상자 안 열었던 경우에는 그대로 남아있음. (x를 누르면 아예 사라짐)
    }
}
