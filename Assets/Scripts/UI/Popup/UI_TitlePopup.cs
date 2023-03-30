using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Define;

public class UI_TitlePopup : UI_Popup
{
	enum Texts
	{
		TitleText,
		StartButtonText,
		OptionButtonText,
		CreditsButtonText,
		QuitButtonText,
	}

	enum Buttons
	{
		StartButton,
		OptionButton,
		CreditsButton,
		QuitButton,
	}

	//필요시, enum GameObjects , Images 구현 / 근데 어차피 @Scene >GameScene으로 이거 가져올거라 굳이?, 그리고 이미지는 따로 필요한거 없음. 다행히..
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

		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
		GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OnClickOptionButton);
		GetButton((int)Buttons.CreditsButton).gameObject.BindEvent(OnClickCreditsButton);
		GetButton((int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);


		/*
		 * Define에서 int값 지정해주고 Define에서 사용하도록 함. 
		 * 또한, Managers 코드에서의 GetText는 Data가 구현되었을 때, 사용하므로 나중에 작업하기.
		//국가별로 kor,eng별로 다르게 바꿔주기 위해서, text도 textdata.xml에서의 id를 기반으로 바꿔치기( 어떤 텍스트를 사용할지 고를 수 있음)
		GetText((int)Texts.NewGameButtonText).text = Managers.GetText(Define.NewGameButtonText);
		GetText((int)Texts.LoadButtonText).text = Managers.GetText(Define.LoadButtonText);
		GetText((int)Texts.OptionButtonText).text = Managers.GetText(Define.OptionButtonText);
		GetText((int)Texts.CreditsButtonText).text = Managers.GetText(Define.CreditsButtonText);
		GetText((int)Texts.QuitButtonText).text = Managers.GetText(Define.QuitButtonText);
		*/

		//Managers.Sound.Clear();
		//Managers.Sound.Play(Sound.Effect, "Sound_MainTitle");
	}

	void OnClickStartButton(PointerEventData data)
	{
		//Managers.Sound.Play(Sound.Effect, "Sound_FolderItemClick");


		/*
		// 데이터 있는지 확인, 로드할 데이터가 있을 경우.
		if (Managers.Game.LoadGame())
		{
			Managers.UI.ShowPopupUI<UI_ConfirmPopup>().SetInfo(() =>
			{
				Managers.Game.Init();
				Managers.Game.SaveGame();

				Managers.UI.ClosePopupUI(this); // UI_TitlePopup
				Managers.UI.ShowPopupUI<UI_NamePopup>();
			}, Managers.GetText(Define.DataResetConfirm));
		}
		else
		{
			Managers.Game.Init();
			Managers.Game.SaveGame();

			Managers.UI.ClosePopupUI(this); // UI_TitlePopup
			Managers.UI.ShowPopupUI<UI_NamePopup>();
		}
		*/
		Managers.Game.Init();
		Managers.UI.ShowPopupUI<UI_StartPopup>();
	}


	void OnClickOptionButton(PointerEventData data)
	{
		//Managers.Sound.Play(Sound.Effect, ("Sound_FolderItemClick"));
		//Managers.Game.Init();
		//Managers.Game.LoadGame();

		Managers.UI.ShowPopupUI<UI_OptionPopup>();
	}
	void OnClickCreditsButton(PointerEventData data)
	{
		//Managers.Sound.Play(Sound.Effect, ("Sound_FolderItemClick"));
		//Managers.Game.Init();
		//Managers.Game.LoadGame();

		Managers.UI.ShowPopupUI<UI_CreditsPopup>();
	}
	void OnClickQuitButton(PointerEventData data)
    {
		Application.Quit();
    }
}
