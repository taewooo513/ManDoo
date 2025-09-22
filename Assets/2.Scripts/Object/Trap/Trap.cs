using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject inGameTrapUI;
    [SerializeField] private GameObject notActiveTrap; //발동x 함정 오브젝트
    [SerializeField] private RectTransform activeTrapRect; //발동한 함정 오브젝트 위치
    private BaseEntity _trappedPlayer;
    private Vector3 _startPos = new Vector3(0, -1025, 0);
    private Vector3 _targetPos = new Vector3(0, -365, 0);

    public void Start()
    {
        activeTrapRect.anchoredPosition = _startPos; //시작 위치
    }

    public void OnTrapRelease() //클릭해서 함정 해제하기
    {
        int randomNum = Random.Range(0, 2); //0, 1중 랜덤 숫자
        
        //TODO : if 아이템의 '함정 해제 도구'를 사용중이라면 100%로 해제
        TrapReleaseSuccess();
        //else //아무것도 없다면 50%로 함정 해제
        switch (randomNum)
        {
            case 0: //해제 성공
                TrapReleaseSuccess();
                break;
            case 1: //실패
                _trappedPlayer = BattleManager.Instance._playableCharacters[0]; //제일 앞에 있는 플레이어
                TrapReleaseFail(_trappedPlayer);
                _trappedPlayer = null; //혹시 몰라서 초기화
                break;
        }
    }

    public void TrapActive(BaseEntity trappedPlayer) //함정 발동    //TODO : 플레이어 지나갈 때 호출
    {   
        _trappedPlayer = trappedPlayer;
        activeTrapRect.DOAnchorPos(_targetPos, 0.1f).SetEase(Ease.Linear); //함정 튀어나오기
        trappedPlayer.entityInfo.canMove = false; //플레이어 못 움직임
        inGameTrapUI.SetActive(true);
        TrapReleaseFail(trappedPlayer); //데미지 닳음
    }

    public void ActiveTrapReturn(BaseEntity trappedPlayer) //발동한 함정 원위치
    {
        activeTrapRect.DOAnchorPos(_startPos, 0.1f).SetEase(Ease.Linear); //트랩 원위치
        trappedPlayer.entityInfo.canMove = true; //플레이어 움직이기 가능
        inGameTrapUI.SetActive(false); //UI 끄기
    }

    public void TrapReleaseSuccess() //미발동 함정 해제 성공 시
    {
        notActiveTrap.IsDestroyed(); //함정 삭제
    }

    public void TrapReleaseFail(BaseEntity trappedPlayer) //함정 해제 실패
    {
        double damage = trappedPlayer.entityInfo.maxHp * 0.1; //최대 체력의 10% 피해
        trappedPlayer.entityInfo.currentHp -= (int)damage; //체력 감소
        
        if (trappedPlayer.entityInfo.currentHp <= 0)
        {
            BattleManager.Instance.EntityDead(trappedPlayer);
        }
    }
}
