using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
	//다른 코드의 bool 형식의 Init이라면, 여기도 수정해줘야해서 오류 시, 나중에 수정 필요.
	//SetCanvas에서, sorting order를 가질 것인지의 여부 = false (SceneUI 이므로 )
	public override void Init()
	{
		//false -> true;
		Managers.UI.SetCanvas(gameObject, false);
	}
}
