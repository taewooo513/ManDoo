using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //스킬이 선택될때 추가해야함
        if (targetImage != null)
            targetImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
            targetImage.SetActive(false);
    }
}
