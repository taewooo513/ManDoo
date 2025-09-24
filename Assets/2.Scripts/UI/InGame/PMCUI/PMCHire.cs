using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCHire : MonoBehaviour
{
    public static PMCHire Instance { get; private set; }

    public GameObject playerPrefab;
    public Vector3[] spawnPoints = new Vector3[4];
    
    private List<GameObject> spawnedPMCs = new List<GameObject>(); // 실제 오브젝트 순서대로

    [SerializeField] private PMCCardManager cardManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 고용(소환)
    public void SpawnPMC(int initID)
    {
        // 1. GameManager 플레이어 리스트에서 중복 체크 (id 기준)
        if (GameManager.Instance.HasPlayerById(initID))
        {
            return;
        }

        // 2. 비어있는 자리 찾기
        int emptyIndex = FindEmptySpawnIndex();
        if (emptyIndex == -1)
        {
            return;
        }
        // 3. 실제 위치에 이미 캐릭터가 있는지 체크
        for (int i = 0; i < spawnedPMCs.Count; i++)
        {
            if (spawnedPMCs[i] != null && spawnedPMCs[i].transform.position == spawnPoints[emptyIndex])
            {
                Debug.LogWarning("이미 해당 자리에 캐릭터가 있습니다!");
                return;
            }
        }

        // 3. 생성 및 등록
        GameObject pmc = Instantiate(playerPrefab, spawnPoints[emptyIndex], Quaternion.identity);

        // 자리 맞춰서 리스트에 삽입
        if (emptyIndex < spawnedPMCs.Count)
            spawnedPMCs[emptyIndex] = pmc;
        else
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
        RefreshCardsOnPanel();
    }
    // 빈 자리 찾기 (맨 앞부터)
    public int FindEmptySpawnIndex()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i >= spawnedPMCs.Count || spawnedPMCs[i] == null)
                return i;
        }
        return -1;
    }

    // 자리 비우기(리무브) + 당김
    public void RemovePlayerAt(int removeIndex)
    {
        if (removeIndex < 0 || removeIndex >= spawnedPMCs.Count || spawnedPMCs[removeIndex] == null)
        {
            return;
        }

        GameObject pmc = spawnedPMCs[removeIndex];
        var playable = pmc.GetComponent<PlayableCharacter>();
        if (playable != null)
        {
            GameManager.Instance.RemovePlayer(playable.id);
        }
        Destroy(pmc);

        // 리스트에서 삭제
        spawnedPMCs.RemoveAt(removeIndex);

        // 뒤에 있는 캐릭터들 앞으로 한 칸씩 당김 & 위치 재배치
        for (int i = removeIndex; i < spawnedPMCs.Count; i++)
        {
            if (spawnedPMCs[i] != null)
            {
                spawnedPMCs[i].transform.position = spawnPoints[i];
            }
        }
        RefreshCardsOnPanel();
    }
    private void RefreshCardsOnPanel()
    {
        if (PMCCardManager.Instance != null)
            PMCCardManager.Instance.RefreshAllCards();
    }
}

