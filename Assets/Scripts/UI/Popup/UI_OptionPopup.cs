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
    /*
    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    */
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
        Init();
    }
    public override void Init()
    {
        base.Init();
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
    }

    public void OnClickConfirmButton(PointerEventData data)
    {
        //Managers.Sound.Play(Sound.Effect, ("Sound_Checkbutton"));
        //Debug.Log("OnClickConfirmButton");
        //Debug.Log($"Input ID {_inputField.text}");
        Managers.UI.ClosePopupUI(this);

    }

}
