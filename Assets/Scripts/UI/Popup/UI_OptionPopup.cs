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
using UnityEngine.Audio;


public class UI_OptionPopup : UI_Popup
{
    /* ===== Audio Setting ====
    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    */

    private Action OptionAction;

    // ===== Screen Space Camera nowSceneCamera 할당 -> 별로 안이뻐서 삭제함
        //정확히는 World Space로 변경시, 카메라 크기가 작아지는 구간에서 크기를 맞추기 어렵다고 판단하여 Screen Space Overlay를 사용하게 됨. (Overlay -> WS -> Camera -> 다시 Overlay )
    //[SerializeField]
    //private Canvas canvas;

    //private Camera nowCamera;
    
    enum Texts
    {
        OptionText,
        SoundText,
        FXText,
    }
    enum Buttons
    {
        ConfirmButton,
    }
    void Start()
    {
        //nowCamera = GameObject.FindObjectOfType<Camera>();
        Init();
    }
    public override void Init()
    {
        base.Init();
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        //canvas.worldCamera = nowCamera;

    }

    public void SetInfo(Action action)
    {
        OptionAction = action;
    }


    public void OnClickConfirmButton(PointerEventData data)
    {
        //Managers.Sound.Play(Sound.Effect, ("Sound_Checkbutton"));
        //Debug.Log("OnClickConfirmButton");
        //Debug.Log($"Input ID {_inputField.text}");
        Managers.UI.ClosePopupUI(this);
        OptionAction?.Invoke();
    }

}
