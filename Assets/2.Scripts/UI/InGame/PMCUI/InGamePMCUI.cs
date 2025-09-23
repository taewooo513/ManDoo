using System.Collections.Generic;
using UnityEngine;
using System;

public class InGamePMCUI : UIBase
{
    public static InGamePMCUI Instance { get; private set; }

    public GameObject playerPrefab; 
    public Transform[] spawnPoints;

    private List<GameObject> spawnedPMCs = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        spawnPoints = new Transform[4];
        spawnPoints[0] = GameObject.Find("First").transform;
        spawnPoints[1] = GameObject.Find("Second").transform;
        spawnPoints[2] = GameObject.Find("Third").transform;
        spawnPoints[3] = GameObject.Find("Fourth").transform;
    }

    public void SpawnPMC(int spawnIndex, int initID)
    {
        // 1. GameManager 플레이어 리스트에서 중복 체크 (id 기준)
        if (GameManager.Instance.HasPlayerById(initID))
        {
            Debug.Log("이미 소환된 PMC입니다!");
            return;
        }

        // 2. 스폰 위치 인덱스 체크
        if (spawnIndex < 0 || spawnIndex >= spawnPoints.Length)
        {
            Debug.LogError($"spawnIndex({spawnIndex})가 spawnPoints 배열 범위를 벗어났습니다!");
            return;
        }

        // 3. 해당 위치에 이미 PMC가 있는지 확인
        bool alreadySpawned = spawnedPMCs.Exists(pmc =>
            pmc != null && Vector3.Distance(pmc.transform.position, spawnPoints[spawnIndex].position) < 0.1f
        );
        if (alreadySpawned)
        {
            Debug.Log($"스폰 위치 {spawnIndex}에 이미 PMC가 존재합니다. 중복 소환 불가.");
            return;
        }

        // 4. 생성 및 등록
        GameObject pmc = Instantiate(playerPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        spawnedPMCs.Add(pmc);

        var playable = pmc.GetComponent<PlayableCharacter>();
        if (playable != null)
        {
            playable.SetData(initID);
            GameManager.Instance.AddPlayer(playable);
        }
        else
        {
            Debug.LogError("PlayableCharacter 컴포넌트가 프리팹에 없습니다!");
        }
    }

    // 빈 위치 인덱스 반환 (-1은 빈 자리 없음)
    public int FindEmptySpawnIndex()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            bool occupied = spawnedPMCs.Exists(pmc =>
                pmc != null && Vector3.Distance(pmc.transform.position, spawnPoints[i].position) < 0.1f
            );
            if (!occupied)
                return i;
        }
        return -1;
    }
    public void RemovePlayerAt(int spawnIndex)
    {
        for (int i = spawnedPMCs.Count - 1; i >= 0; i--)
        {
            GameObject pmc = spawnedPMCs[i];
            if (pmc != null && Vector3.Distance(pmc.transform.position, spawnPoints[spawnIndex].position) < 0.1f)
            {
                spawnedPMCs.RemoveAt(i);
                var playable = pmc.GetComponent<PlayableCharacter>();
                if (playable != null)
                {
                    GameManager.Instance.RemovePlayer(playable.id); // 또는 playable.ID
                }
                Destroy(pmc);
                Debug.Log($"스폰 위치 {spawnIndex}의 플레이어 삭제 완료");
                return;
            }
        }
        Debug.Log($"스폰 위치 {spawnIndex}에 플레이어가 없습니다.");
    }

}