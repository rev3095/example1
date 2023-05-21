using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : BaseScene
{

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;
        if (Managers.Game.SaveData.IsFirstStart == false)
        {
            Managers.UI.CloseAllPopupUI();

            //이때 그 DialogueTest에 해당하는 , IntroDialogue가 실행되어야 함.
            
            //Managers.UI.ShowPopupUI<UI_DialoguePopup>();

            //SceneManager.LoadScene("BattleScene");
            Managers.Game.SaveData.IsFirstStart = true;
        }
    }

    public override void Clear()
    {
        Debug.Log("TitleScene Clear!");
        Managers.UI.CloseAllPopupUI();
    }
}
