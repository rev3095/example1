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
    public TMP_Text txt1;
    public TMP_Text txt2;

    public UI_EventHandler handler;
    enum Buttons
    {
        NormalButton,
        HardButton,
        PlayButton,
        ExitButton,
    }

    private void Start()
    {
        txt1.enabled = false;
        txt2.enabled = false;
        Init();
    }
    public override void Init()
    {
        base.Init();
        
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.NormalButton).gameObject.BindEvent(OnClickNormalButton);
        GetButton((int)Buttons.HardButton).gameObject.BindEvent(OnClickHardButton);
        GetButton((int)Buttons.PlayButton).gameObject.BindEvent(OnClickPlayButton);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        GetButton((int)Buttons.PlayButton).gameObject.GetComponent<UI_EventHandler>().enabled = false;

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
        GetButton((int)Buttons.PlayButton).gameObject.GetComponent<UI_EventHandler>().enabled = true;


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
        GetButton((int)Buttons.PlayButton).gameObject.GetComponent<UI_EventHandler>().enabled = true;


    }

    void OnClickPlayButton(PointerEventData data)
    {
        if (Managers.Game.SaveData.Difficulty == null)
        {
            return;
        }
        else if(Managers.Game.SaveData.Difficulty != null)
        {
            //GetComponent<Button>().interactable = true;
            Managers.Game.SaveGame();
            Managers.UI.CloseAllPopupUI();
            SceneManager.LoadScene("LobbyScene");
            //if (IsNormal == true) if (isHard ==true);로 return 코드 작성 (밸런스는 Enemy 스탯 1.5배 높여서 스탯 데이터 작성)
        }
    }

    void OnClickExitButton(PointerEventData data)
    {
        Managers.Game.SaveData.Difficulty = null;
        Managers.UI.ClosePopupUI(this);
    }


}
