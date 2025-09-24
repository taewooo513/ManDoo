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

        // 3. 생성 및 등록
        GameObject pmc = Instantiate(playerPrefab, spawnPoints[GameManager.Instance.PlayableCharacter.Count], Quaternion.identity);

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

        UpdatePlayerPositions();

        RefreshCardsOnPanel();
    }
    private void UpdatePlayerPositions()
    {
        for (int i = 0; i < GameManager.Instance.PlayableCharacter.Count; i++)
        {
            GameManager.Instance.PlayableCharacter[i].transform.position = spawnPoints[i];
        }
    }
    private void RefreshCardsOnPanel()
    {
        if (PMCCardManager.Instance != null)
            PMCCardManager.Instance.RefreshAllCards();

    }
}

