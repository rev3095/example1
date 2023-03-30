using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;
using TMPro;


[Serializable]
public struct ChooseData //chooseDB.xlsx -> Reimport된 chooseDB ScriptableObject로 name, chooseData를 가져오게 됨.
{
    public string dialogue; //대사

}

//클래스는 상속 하나만 됨. 다중상속 안됨. 다중 상속은 인터페이스만 가능함.
public class UI_ChoicePopup : UI_Popup
{

    //프리펩을 로드하려면 버튼을 로드해야되는데 그러려면 여기서 해야될듯 ㅇㅅㅇ;
    // List에 넣고, Clear()

    // 동일하게 List<프로젝트아이템> _projectItem
    //Init 바인드 , Populateroject()를 Init에서 실행함. ( Init 시점에서 전부 데이터 할당후 가져오기)
    //PopulateProject()는 여기서 구현하기
    enum Buttons
    {
        UI_ChoiceButton,
    }

    enum GameObjects
    {
        ChoiceContext,
    }
    [SerializeField]
    public int branch;

    public static int nowBranch;

    //S.O
    [SerializeField]
    public ChooseDB chooseDB;

    //이게 결론적으로 선택지 각 데이터 (얘만 가지고 오면 됨 for문으로 )
    [SerializeField]
    public ChooseData[] chooseData; // 현재 branch의 대사 목록 배열 ->Awake()에서 할당됨

    public void Awake()
    {
        int index = 0;
        chooseData = new ChooseData[chooseDB.Entities.Count];

        for (int i = 0; i < chooseDB.Entities.Count; i++)
        {
            if (chooseDB.Entities[i].branch == branch) //branch또한 동적 할당 필요.
            {
                chooseData[index].dialogue = chooseDB.Entities[i].dialogue;
                index++;
            }
        }
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        PopulateButtons();
    }


    // GameManager <-  캐릭터 배경 
    // 1. GameManager () Update (캐릭터, 배경, ~~ )
    // 캐릭터 , 배경 Awake Start Update .. Unity XX

    // 2. 
    // 

    public void Update()
    {
        // asdf.update();
        // asdds.update();
        // asdsd.update();

        // 1번만


        // 선택지 버튼 Action -> Update X

        // 선택지 클릭되면 로직 실행

        // 동적 --- 

        if (DialogueSystem.isChooseEnd == true)
        {
            Managers.UI.ClosePopupUI(this);
            DialogueSystem.isChooseEnd = true;
        }

    }
    List<UI_ChoiceButton> _choiceButtons = new List<UI_ChoiceButton>();
    public static string ChoiceText;
    public void GetChoiceText(string choiceText)
    {
        ChoiceText = choiceText;
    }

    public void PopulateButtons()
    {
        _choiceButtons.Clear(); //아까 프로젝트 아이템을 선언해준 리스트를 클리어함. ( 더미데이터 삭제 , 혹은 리프레시 할때 다시 사용할 수 있도록, 추가로 동적할당 시켜줄 수 있도록)
        var parent = GetObject((int)GameObjects.ChoiceContext);

        foreach (Transform child in parent.transform)
            Managers.Resource.Destroy(child.gameObject); //더미를 Destroy 시킴. 현재 게임오브젝트가 오브젝트 할당때문에 가지고 있던 더미 데이터 ( 일단 가지고 있어야되긴함, setactive로 가지고 있을수도 있다고 생각들지만, 그걸 대비해서 루키스가 일단 가지고 있도록 만들라고 한 것 같음.)

        //List<UI_ChoiceButton> projects = Managers.Data.Projects.Values.ToList();  // 메인 메인ㄴㄴㄴㄴㄴㄴㄴㄴㄴㄴㄴ로직 -> Data 구현시키기.

        
        for (int i = 0; i < chooseDB.Entities.Count; i++)
        {
            UI_ChoiceButton item = Managers.UI.MakeButton<UI_ChoiceButton>(parent.transform,"UI_ChoiceButton");
            //SetInfo 코드 있었음 -> 이거 나중에 SetInfo만 구현해보자, 마지막에 안되면
            GetChoiceText(chooseData[i].dialogue);
            item.Init();
            item.SetChoiceText(ChoiceText);

            _choiceButtons.Add(item);
        }

    }

    //코루틴으로 선택 전까지 Dialogue재생시키지 않도록 설정  --
    //여기서 ChoiceSytem 코드를 이용해서, 버튼을 동적 생성 -> 이게 BindObject를 ChoiceContext를 가져오려면 어쩔수없이 ChoiceSystem 대신, 여기다가 로직을 작성해야 함. 굳이 ChoiceSystem을 만들어서 GetComponent를 시켜도 되지만 일단은 사본이 있으니 이렇게 진행.

}