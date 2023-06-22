using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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

            //Dialogue가 끝난 뒤에, 아이템 제공.
            //데이터 상으로는 이게 맞고, 이후에 인벤토리 띄울때 이 정보로 MakeSubItem으로 프리펩만 만들어주기. 결국 이 정보가 핵심임.

            //인덱스는 data.ID로 시작하므로, 0이 아니라 1부터 시작해주어야 한다.
            Managers.Game.AddItem("Use",1);
            Managers.Game.AddItem("Com",1);

            Managers.Game.SaveData.IsFirstStart = true;

            Managers.Game.SaveGame();
        }
    }

    public override void Clear()
    {
        Debug.Log("TitleScene Clear!");
        Managers.UI.CloseAllPopupUI();
    }
}
