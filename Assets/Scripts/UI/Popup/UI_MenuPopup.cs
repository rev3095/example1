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
        SaveButton,
        OptionButton,
        TitleButton,
    }

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        /*if (base.Init() == false)
			return false;
		*/
        base.Init();

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SaveButton).gameObject.BindEvent(OnClickSaveButton);
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OnClickOptionButton);
        GetButton((int)Buttons.TitleButton).gameObject.BindEvent(OnClickTitleButton);

    }

    //Mask 기능을 이용해서 현재 팝업 영역 이외를 클릭했을 때 꺼지도록 마우스 클릭을 받아 사용하려고 했는데,
        //현재 이미지를 못짜를 것 같아서 (배경 투명한 psd 파일로 달라고 요청) , 일단은 esc 입력으로 해당 팝업을 종료 처리
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.ClosePopupUI(this);
        }

    }

    void OnClickSaveButton(PointerEventData data)
    {
        Managers.Game.SaveGame();
        Debug.Log("OnClicked");
        //세이브 완료! 확인창 띄우기  -> ConfirmPopup에서 .text 나 TextAsset 혹은 TMPUGUI 가져와서 ㅇㅇ
    }

    void OnClickOptionButton(PointerEventData data)
    {
        Managers.UI.ShowPopupUI<UI_OptionPopup>();
        Debug.Log("OnClicked");

    }

    void OnClickTitleButton(PointerEventData data)
    {
        Managers.UI.CloseAllPopupUI();
        Managers.UI.ShowPopupUI<UI_TitlePopup>();
    }




}
