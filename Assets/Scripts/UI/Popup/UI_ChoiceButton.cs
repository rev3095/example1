using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;
using TMPro;

public class UI_ChoiceButton : UI_Base
{
    enum Texts
    {
        ChoiceButtonText,
    }

    enum Buttons
    {
        UI_ChoiceButton,
    }

    /*
    [SerializeField] 
    public TextMeshProUGUI dial;
    public static string strVal;

    Button btn;
    

    void Awake()
    {
        dial.text = strVal;
    }
    */

    //ProjectData
    //List<ChooseDB>의 dialogu리스트 - > 대사 텍스트 -> 

    //개미키우기에는 프로젝트 버튼 데이터, 프로젝트 버튼의 세부 텍스트 데이터를 두개로 가져옴. 

    //무튼 이렇게 가져온 데이터를 GetText 시켜주기, 데이터 할당은 S.O가 아니라 Json으로 진행하기.
    //Onclick이벤트는 버튼에서 할당시켜주기 ,Init에서 이벤트 할당 -> ㅇㅋ
    
   
    public override void Init()
    {
        //base.Init();
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.UI_ChoiceButton).gameObject.BindEvent(OnClickUI_ChoiceButton);
        //GetText((int)Texts.ChoiceButtonText).text = ... -> 이거 어차피 버튼 텍스트를 가져오는거여서, 현재 데이터 구조에서 의미 없음. 어차피 뒤 로직에서 동적 수정될 예정.

        //gameObject.BindEvent(OnClickProjectItem); 이렇게 하고, 버튼이 아니라 이 영역 오브젝트가 (Vertical Layout 만 가진 ) 이 게임 오브젝트가 클릭되었을때 실행됨. 코드 구조상 이게 맞음. 얘도 이해는 되는데 ㅇㅇ

        //Rookiss 프로젝트에서는, AbilityText, AbilityValueText(각 3개 -> Money, RewardValue, RewardColor) 가져오는데, 이건 선택지 팝업이므로 선택지 데이터만 가져오면 됨.

    }


    void OnClickUI_ChoiceButton(PointerEventData data)
    {
        DialogueSystem.isChooseEnd = true;  //static 값
    }

    //public UI_ChoicePopup nowChoicePopup = Utils.GetOrAddComponent<UI_ChoicePopup>(this); 하고 싶었는데 이게 힘드니까 그냥 static 변수로 가져와야될듯 ㅇㅇ
    public void SetChoiceText(string str)
    {
        //이 텍스트 불러올려고 바인딩 했자너~
        GetText((int)Texts.ChoiceButtonText).text = str;

    }




    //ProjectData _data; 
    //List<UI_ProjectItemSubItem> _items = new List<UI_ProjectItemSubItem>(); //텍스트 가져온 것.
    /*
     * public void SetInfo(ProjectData data, float delay)
    {
        _data = data;
    }

    */
    //나는 choiceDB만 가져오면 됨.     ChoiceSystem.chooseData

}


//여기서 ChoiceData 가져오기
//그리고 GetText해서 가져오기.