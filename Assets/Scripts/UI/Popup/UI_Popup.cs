using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    //팝업이므로, 고정 캔버스(Scene)과 다르게 닫는 과정이 필요 ->생성과 삭제를 함수 실행으로 처리, stack sorting order도 고려 필수.
    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
