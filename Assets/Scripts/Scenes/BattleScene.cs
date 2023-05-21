using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleScene : BaseScene
{
    public bool preBattle;
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Battle;
        preBattle = true;
    }   

    public override void Clear()
    {
        Debug.Log("TitleScene Clear!");
        Managers.UI.CloseAllPopupUI();
    }
}
