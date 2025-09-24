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
        PMCCardManager.Instance.RefreshCardsOnPanel();
    }

    public void RemovePlayerAt(int index)
    {
        var playable = GameManager.Instance.PlayableCharacter[index];
        if (playable != null)
        {
            GameManager.Instance.RemovePlayer(playable.id);
            Destroy(playable.gameObject);
            // 리스트에서 직접 삭제하는 RemovePlayer 로직에 포함!
        }

        UpdatePlayerPositions();
        PMCCardManager.Instance.RefreshCardsOnPanel();
    }
    private void UpdatePlayerPositions()
    {
        for (int i = 0; i < GameManager.Instance.PlayableCharacter.Count; i++)
        {
            GameManager.Instance.PlayableCharacter[i].transform.position = spawnPoints[i];
        }
    }  
}

