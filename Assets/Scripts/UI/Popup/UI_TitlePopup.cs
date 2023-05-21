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

	enum Images
	{
		TitleImage,
    }

    //targetSprite를 public Sprite targetSprite = gameObject.GetComponent<Image>().sprite ; 이렇게 쓰고 싶었는데 캐싱 오류가 나서 패스.
    //그래서 그냥 Image 오브젝트를 드래그 해놓고 Drag Drop으로 GetComponent 시키기.

    //동적으로 현재 Resources 폴더에 TitleSprite가 몇 개 있는지 확인할 필요가 없음. 따라서 그냥 배열 크기 정하고, for문 돌려서 Image를 배열에 넣어놓자.

    bool isPlay = true;

	public int CurIndex;

    public Image targetImage;

	public Sprite[] images = new Sprite[8];


	public void Start()
	{
		Init();
		CurIndex = 0;
		isPlay = true;
	}
	public override void Init()
	{
		base.Init();

		BindText(typeof(Texts));
		BindButton(typeof(Buttons));
		BindImage(typeof(Images));
		;
		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
		GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OnClickOptionButton);
		GetButton((int)Buttons.CreditsButton).gameObject.BindEvent(OnClickCreditsButton);
		GetButton((int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);

		targetImage = GetImage((int)Images.TitleImage).GetComponent<Image>();
		//8번 반복됨.
		for (int i = 1; i < images.Length + 1; i++)
		{
			images[i - 1] = Resources.Load<Sprite>($"Sprites/UI/Title/{i}");
		}

		StartCoroutine(FadeAlternativeImage());

		//Managers.Sound.Clear();
		//Managers.Sound.Play(Sound.Effect, "Sound_MainTitle");


	}

	//코루틴에 주석처리했던 부분의 문제를 해결하기 위한 함수로써, 다시 코루틴을 호출시켜줌.
	//근데 코루틴은 매번 시작되는게 아니라, yield를 이해하면 다시 실행시킬 수 있기 때문에 StartCoroutine을 삭제함.
	void ContinueCo()
    {
		//void 함수 나가게 해주는 방지턱 (break 와 동일한 기능)
		if (isPlay)
			return;

		isPlay = !isPlay;
		//StartCoroutine(FadeAlternativeImage());


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

		isPlay = !isPlay;

		Managers.Game.Init();
		Managers.UI.ShowPopupUI<UI_SelectPopup>().SetInfo(ContinueCo);
	}


	//OptionData 따로 만들기
	void OnClickOptionButton(PointerEventData data)
	{
		//Managers.Sound.Play(Sound.Effect, ("Sound_FolderItemClick"));
		//Managers.Game.Init();
		//Managers.Game.LoadGame();
		isPlay = !isPlay;

		Managers.UI.ShowPopupUI<UI_OptionPopup>().SetInfo(ContinueCo);
		
	}
	void OnClickCreditsButton(PointerEventData data)
	{
		//Managers.Sound.Play(Sound.Effect, ("Sound_FolderItemClick"));
		isPlay = !isPlay;

		Managers.UI.ShowPopupUI<UI_CreditsPopup>().SetInfo(ContinueCo);
	}
	void OnClickQuitButton(PointerEventData data)
	{
		Application.Quit();
	}


	//다시 얘를 실행시켜줄 애가 없어서 else break로 StopCoroutine이 아니라, WaitUntil이나, 실제로 코루틴은 돌아가는데 !IsPlay인 상태 지속되서 코루틴만 돌아가게 만든다던가,
	//계속 이 코루틴을 실행시키되, 불리언 값으로 Update처럼 처리를 받는거임. 그러면 결국은 Update와 똑같은 기능을 하게 됨.
	IEnumerator FadeAlternativeImage()
	{
		while (isPlay)
		{


			for (int i = CurIndex; i < images.Length; i++)
			{

				if (isPlay)
				{
					targetImage.sprite = images[i];
					targetImage.gameObject.SetActive(true);
					for (float f = 0f; f < 1.2; f += 0.01f)
					{
						Color c = targetImage.color;
						c.a = f;
						targetImage.color = c;
						yield return null;
					}
					yield return new WaitForSeconds(2f);

					if (isPlay)
					{
						for (float f = 0f; f < 1.2; f += 0.01f)
						{
							//2번 판넬을 페이드 아웃 하면 1번이 페이드 인 되는 듯 보인다!
							Color c = targetImage.color;
							c.a = 1 - f;
							targetImage.color = c;
							yield return null;
						}
						targetImage.gameObject.SetActive(false);
						yield return new WaitForSeconds(0.2f);

					}


				}
				else
				{
					yield return new WaitUntil(() => isPlay == true);
				}

			}


		}
		

	}

}