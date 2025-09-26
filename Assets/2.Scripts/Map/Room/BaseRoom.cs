using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class BaseRoom
{
    public Dictionary<RoomDirection, BaseRoom> connectedRooms = new();
    public Dictionary<RoomDirection, Corridor> corridors = new();
    public Dictionary<int, GameObject> playableCharacterDic; //int에 키값, 게임오브젝트에 대응하는 프리팹
    public string RoomLocation;//시작 지점으로부터의 방향을 뜻함
    public Spawn spawn;
    public BattleData battleData; //배틀데이터 데이터테이블
    public RewardData rewardData;
    protected int dropGoldCount;
    protected int dropItem; //드랍하는 아이템id (골드x)
    protected float battleDropProb; //아이템 드랍 확률 (ex : 0.25)
    protected float goldRandomRatio; //0.9~1.1 사이 랜덤 난수 반환, 골드 떨어지는 랜덤 개수
    protected int randomGoldDropCount; //실제로 떨어지는 금화 개수
    protected float randomPercentage; //0~100 사이 중 랜덤 퍼센트 (랜덤 숫자 뽑기)
    protected List<int> equipItemIds = new(); //플레이어 죽었을 때, 가지고 있던 장비 아이템 저장하는 리스트. 복사본을 가져야되니 new로 생성
    protected int battleRewardGroupId; //배틀데이터에 있는 그룹 아이디
    protected int rewardGroupId; //보상 테이블 연결해주는 id
    protected List<RewardData> rewardIdList; //그룹에 속한 id 리스트
    protected int rewardRoomId; //실제로 보상 주는 방 id
    
    public virtual void EnterRoom(int id)
    {
        if (spawn == null) //스폰이 null이면
        {
            GameObject spawnObject = new GameObject("Spawn"); //스폰 생성
            spawn = spawnObject.AddComponent<Spawn>(); //스폰 컴포넌트 챙겨오기
        }
        spawn.PlayableCharacterSpawn(); //플레이어 소환(위치 선정)
    }

    public void SetRoomLocation(BaseRoom parentRoom, RoomDirection direction)
    {
        //???이런 것도 된다고?
        RoomLocation = parentRoom.RoomLocation + direction switch
        {
            RoomDirection.Up => "U",
            RoomDirection.Down => "D",
            RoomDirection.Left => "L",
            RoomDirection.Right => "R"
        };
    }
    public virtual void ExitRoom(int id)
    {
        Init(id);
    }

    public void Init(int id) //전투 보상 등등 기본값 세팅
    {
        battleData = DataManager.Instance.Battle.GetBattleData(id); //배틀데이터 데이터테이블에 접근
        rewardData = DataManager.Instance.Reward.GetRewardData(id); //보상 테이블 연결
        
        dropGoldCount = battleData.dropGold; //골드 드랍 개수
        dropItem = battleData.dropId; //드랍하는 아이템id (골드x)
        battleDropProb = battleData.dropProb; //아이템 드랍 확률 (ex : 0.25)
        battleRewardGroupId = battleData.rewardId;
        rewardGroupId = rewardData.groupId; //랜덤가챠 돌릴 범위
        rewardIdList = DataManager.Instance.Reward.GetRewardGroupList(rewardGroupId); //보상 그룹 가져오기
        
        goldRandomRatio = Random.Range(0.9f, 1.1f); //0.9~1.1 사이 랜덤 난수 반환, 골드 떨어지는 랜덤 개수
        randomGoldDropCount = (int)(dropGoldCount * goldRandomRatio); //실제로 떨어지는 금화 개수
        randomPercentage = Random.Range(0f, 100f);
    }

    public void Clear()
    {
        rewardIdList.Clear();
    }

    public void PlayerDeadItem(List<int> id) //플레이어가 죽을 때 가지고 있던 아이템 리스트
    {
        equipItemIds = id;
    }

    public Corridor MakeConnection(BaseRoom room, RoomDirection direction)
    {
        connectedRooms.Add(direction, room);
        var corridor = new Corridor();
        corridors.Add(direction, corridor);
        corridor.MakeCells();

        return corridor;
    }

    public void ApplyConnection(BaseRoom room, RoomDirection direction, Corridor corridor)
    {
        connectedRooms.Add(direction, room);
        corridors.Add(direction, corridor);
    }

    public bool IsConnected(BaseRoom room, RoomDirection direction)
    {
        if (connectedRooms.TryGetValue(direction, out BaseRoom connectedRoom))
        {
            return connectedRoom == room;
        }

        return false;
    }
}
