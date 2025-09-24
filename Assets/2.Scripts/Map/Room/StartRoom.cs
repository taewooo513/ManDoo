using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : BaseRoom
{
    public override void EnterRoom()
    {
        GameObject spawnObject = new GameObject("Spawn");
        Spawn spawn = spawnObject.AddComponent<Spawn>(); //스폰 컴포넌트 챙겨오기
        spawn.PlayableCharacterSpawn(1004); //시작 플레이어(1004) 생성
        
        //인벤토리.gold += 1500;
        
        //인벤토리.포션 랜덤 3개
        //for(int i=0; i<3; i++){
        //int potionId = Random.Range(0, 포션.count);
        //인벤토리 += potionId; //포션 추가
        //}
    }
}
