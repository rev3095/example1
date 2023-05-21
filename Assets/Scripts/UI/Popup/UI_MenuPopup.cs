using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Define;
using TMPro;

public class UI_MenuPopup : UI_Popup
{
    enum Texts
    {
        MenuText,
        SaveText,
        OptionText,
        TitleText,
    }

    enum Buttons
    {
        TutorialButton,
        OptionButton,
        TitleButton,
    }

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.TutorialButton).gameObject.BindEvent(OnClickTutorialButton);
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OnClickOptionButton);
        GetButton((int)Buttons.TitleButton).gameObject.BindEvent(OnClickTitleButton);

    }

    //Mask 기능을 이용해서 현재 팝업 영역 이외를 클릭했을 때 꺼지도록 마우스 클릭을 받아 사용하려고 했는데,
        //현재 이미지를 못짜를 것 같아서 (배경 투명한 psd 파일로 달라고 요청) , 일단은 esc 입력으로 해당 팝업을 종료 처리
        //esc 입력하는게 좀 불편해서 ||로 여러 Input 처리를 받는 편의성 작업이 필요할듯 함.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.ClosePopupUI(this);
        }

    }

    void OnClickTutorialButton(PointerEventData data)
    {
        //Managers.UI.ShowPopupUI<UI_TutorialPopup>();
    }

    void OnClickOptionButton(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_OptionPopup>();
        Debug.Log("OnClicked");

    }

    void OnClickTitleButton(PointerEventData data)
    {
        Managers.UI.CloseAllPopupUI();
        SceneManager.LoadScene("TitleScene");
    }




}
