using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectSkillButton : MonoBehaviour/*, IPointerEnterHandler, IPointerExitHandler*/
{

    private Button skillButton;
    private InGameUIManager inGameUIManager;

    //public GameObject[] targetImage;

    private void Awake()
    {
        skillButton = GetComponent<Button>();
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
        //if (targetImage != null)
        //{
        //    foreach (var img in targetImage)
        //        if (img != null) img.SetActive(false);
        //}
    }
    public void SetButton(Skill skill)
    {
        skillButton.onClick.AddListener(() => OnClickSkillButton(skill));
    }

    private void OnClickSkillButton(Skill skill)
    {
        inGameUIManager.OnClickSkillButton(skill);
    }
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (targetImage != null)
    //    {
    //        foreach (var img in targetImage)
    //            if (img != null) img.SetActive(true);
    //    }
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (targetImage != null)
    //    {
    //        foreach (var img in targetImage)
    //            if (img != null) img.SetActive(false);
    //    }
    //}
}
