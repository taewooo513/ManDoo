using DataTable;
using UnityEngine;

public class PMCCardManager : MonoBehaviour
{
    public PMCInfo[] pmcCards;

    public void RefreshAllCards()
    {
        pmcCards = GetComponentsInChildren<PMCInfo>(true); // 항상 최신 카드 리스트
        foreach (var card in pmcCards)
        {
            bool hasPlayer = GameManager.Instance.HasPlayerById(card.InitID); // 프로퍼티로 접근!
            card.gameObject.SetActive(!hasPlayer);
        }
    }
}
