using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCHire : MonoBehaviour
{
    public static PMCHire Instance { get; private set; }
    public Spawn spawnManager; // 인스펙터에서 연결

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (spawnManager == null)
            spawnManager = FindObjectOfType<Spawn>();
    }

    // 용병 고용(캐릭터 생성)
    public void HirePMC(int id)
    {
        // 중복 체크: GameManager의 플레이어 리스트에서 id 확인
        if (GameManager.Instance.HasPlayerById(id))
        {
            Debug.LogWarning("이미 고용된 용병입니다!");
            return;
        }

        // Spawn의 캐릭터 생성 함수 호출
        spawnManager.PlayableCharacterCreate(id);

        // Spawn에서 위치 지정이 따로 필요하면, spawnManager.PlayableCharacterSpawn() 호출
        // ex) spawnManager.PlayableCharacterSpawn();
    }

    // 기타 자리 관리, 제거 등 필요시 추가

}

