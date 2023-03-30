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


public class UI_NamePopup : UI_Popup
{
	enum GameObjects
	{
		InputField,
	}

	public TMP_Text ConfirmButtonText;
	public TMP_Text ValueText;

	enum Buttons
	{
		ConfirmButton,
	}

	TMP_InputField _inputField;

	void Start()
    {
		Init();
    }
	public override void Init()
	{
		base.Init();

		BindObject(typeof(GameObjects));
		//BindText(typeof(Texts));
		BindButton(typeof(Buttons));

		GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

		_inputField = GetObject((int)GameObjects.InputField).gameObject.GetComponent<TMP_InputField>();
		_inputField.text = "";

		//RefreshUI();
	}
	/*
	void RefreshUI()
	{
		GetText((int)Texts.NameText).text = Managers.GetText(Define.Sinibe);
		GetText((int)Texts.HintText).text = Managers.GetText(Define.PleaseWriteNickName);
	}
	*/
	public void OnClickConfirmButton(PointerEventData data)
	{
		//Managers.Sound.Play(Sound.Effect, ("Sound_Checkbutton"));
		//Debug.Log("OnClickConfirmButton");
		//Debug.Log($"Input ID {_inputField.text}");

		if (_inputField.text.Length <= 0)
        {
		
        }
        else
		{

			//Managers.Game.InputName = _inputField.text;
			//Managers.Game.SaveData.Name = Managers.Game.InputName;
			Managers.Game.SaveData.Name = _inputField.text;
			//Managers.Game.SaveGame();

			Debug.Log("현재 이름:" + Managers.Game.SaveData.Name);
			// UI_NamePopup 닫기
			Managers.UI.ClosePopupUI(this);
			Managers.UI.ShowPopupUI<UI_DifficultyPopup>();
		}

	}
}
