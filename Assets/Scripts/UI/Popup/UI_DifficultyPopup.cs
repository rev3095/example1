using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_DifficultyPopup :UI_Popup
{
    //public bool IsNormal = false; -> Managers.Game.IsNormal = true;로 설정하기
    //public bool isHard = false;


    //public GameObject txt1 = transform.Find("NormalButtonText_2").gameObject;

    //public TMP_Text NormalButtonText_2;

    public TMP_Text txt1;
    public TMP_Text txt2;


    /*
    enum Texts
    {
        NewGamePopupText,
        NormalButtonText_1,
        NormalButtonText_2,
        HardButtonText_1,
        HardButtonText_2,
        PlayButtonText,
        ExitButtonText,

    }

    */
    enum Buttons
    {
        NormalButton,
        HardButton,
        PlayButton,
        ExitButton,
    }

    private void Start()
    {
        //GameObject txt1 = GameObject.Find("NormalButtonText_2");
        //txt1.SetActive(false);

        //NormalButtonText_2 = FindObjectOfType<TMP_Text>();
        txt1.enabled = false;
        txt2.enabled = false;
        Init();
    }
    public override void Init()
    {
        base.Init();
        //GameObject txt2 = GameObject.Find("HardButtonText_2");
        
        //BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.NormalButton).gameObject.BindEvent(OnClickNormalButton);
        GetButton((int)Buttons.HardButton).gameObject.BindEvent(OnClickHardButton);
        GetButton((int)Buttons.PlayButton).gameObject.BindEvent(OnClickPlayButton);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        //GetText((int)Texts.NormalButtonText_2).text = Managers.GetText(Define.NormalButtonText_2);
        //GetText((int)Texts.HardButtonText_2).text = Managers.GetText(Define.HardButtonText_2);

        

    }

    void OnClickNormalButton(PointerEventData data)
    {

        if (txt2.enabled == true)
        {
            txt2.enabled = false;
            txt1.enabled = true;
        }
        else
        {
            txt1.enabled = true;
        }
        Managers.Game.SaveData.Difficulty = "Normal";
        Managers.Game.IsNormal = true;
        
    }

    void OnClickHardButton(PointerEventData data)
    {
        if (txt1.enabled == true)
        {
            txt1.enabled = false;
            txt2.enabled = true;
        }
        else
        {
            txt2.enabled = true;
        }
        Managers.Game.IsNormal = false;
        Managers.Game.SaveData.Difficulty = "Hard";

    }

    void OnClickPlayButton(PointerEventData data)
    {
        if (Managers.Game.SaveData.Difficulty == null)
        {
            //GetComponent<Button>().interactable = false;
            //이때는 플레이 버튼이 작동하지 않음.
        }
        else
        {
            //GetComponent<Button>().interactable = true;
            Managers.Game.SaveGame();
            Managers.UI.CloseAllPopupUI();
            Managers.UI.ShowPopupUI<UI_Lobby_10F>();
        }
        //if (IsNormal == true) if (isHard ==true);로 return 코드 작성 (밸런스는 Enemy 스탯 1.5배 높여서 스탯 데이터 작성)ㅊ

    }

    void OnClickExitButton(PointerEventData data)
    {
        Managers.UI.ClosePopupUI(this);

    }


}
