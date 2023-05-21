using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using static Define;
using TMPro;

public class UI_CreditsPopup : UI_Popup
{
    enum Texts
    {
        CreditsText,
        CreditsText_1,
        CreditsText_2,
    }

    enum Buttons
    {
        ConfirmButton,
    }

    void Start()
    {
        Init();
    }

    Action CreditsAction;
    public override void Init()
    {
        base.Init();
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
    }
    public void SetInfo(Action action)
    {
        CreditsAction = action;
    }


    public void OnClickConfirmButton(PointerEventData data)
    {
        //Managers.Sound.Play(Sound.Effect, ("Sound_Checkbutton"));
        //Debug.Log("OnClickConfirmButton");
        //Debug.Log($"Input ID {_inputField.text}");
        Managers.UI.ClosePopupUI(this);
        CreditsAction?.Invoke();

    }


}
