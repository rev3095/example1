using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//구조상, Speakers는 무조건 Inspector창에서 할당하고 시작해야 함. 

[Serializable]
public struct Speaker
{
    //spriteRenderer -> UI.Image 사용
    public Image imageCharacter; //캐릭터 UI 이미지 (청자/화자 알파 값 제어)
    public Image imageDialogue; //대화 창 UI 이미지
    public TextMeshProUGUI textName; //현재 대사 출력 중인 캐릭터 이름 출력 UI Text
    public TextMeshProUGUI textDialogue; // 현재 대사 출력 UI Text
    public GameObject objectArrow; //대사 출력이 완료되었을 때 나타나는 커서 오브젝트
    public Image textBG;
}


[Serializable]
public struct DialogueData //DialogueDB.xlsx -> Reimport된 DialogueDB ScriptableObject로 name, dialogue를 가져오게 됨.
{
    public int speakerIndex; //이름과 대사를 출력할 현재 DialogueSystem의 Speaker 배열 순번
    public string name; //캐릭터 이름

    //[TextArea(3,5)] -> 텍스트 영역 지정 가능, 최소 줄 / 최대 줄 설정
    public string dialogue; //대사
    public bool isChoose; // UI_ChoicePopup으로 넘어갈 불리언 값
}


public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private int branch;

    [SerializeField]
    private DialogueDB dialogueDB;

    [SerializeField]
    private Speaker[] speakers; //대화에 참여하는 캐릭터들의 UI[]

    [SerializeField]
    private DialogueData[] dialogues; // 현재 branch의 대사 목록 배열 ->Awake()에서 할당됨

    [SerializeField]
    private bool isAutoStart = true; //자동 시작 여부
    private bool isFirst = true; //최초 1회만 호출하기 위한 변수
    private int currentDialogueIndex = -1; //현재 대사 순번
    private int currentSpeakerIndex = 0;//현재 말을 하는 화자(Speaker)의 speakers 배열 순번
    private float typingSpeed = 0.1f; //텍스트 타이핑 효과의 재생 속도
    private bool isTypingEffect = false; // 텍스트 타이핑 효과를 재생중인지 확인하는 불리언 값

    public static bool isChooseEnd = false;

    private void Awake()
    {
        int index = 0;
        //List.Count -> 요소의 수 (Length와 동일)
        //branch, name, dialogue 를 가지는 DialogueDBEntity 클래스의 요소 하나 당 for 문 진행
        for (int i = 0; i < dialogueDB.Entities.Count; i++)
        {
            //현재 DialogueSystem에서 설정된 branch 번호와 DialogueDB에서의 branch 번호가 맞을 경우, 그 Entities 데이터를 불러옴.
            if (dialogueDB.Entities[i].branch == branch)
            {
                dialogues[index].name = dialogueDB.Entities[i].name;
                dialogues[index].dialogue = dialogueDB.Entities[i].dialogue;
                dialogues[index].isChoose = dialogueDB.Entities[i].isChoose;
                index++;
            }
        }
        Init();
    }

    private void Init()
    {
        //모든 대화 관련 게임 오브젝트 비활성화
        for (int i = 0; i < speakers.Length; i++)
        {
            SetActiveObjects(speakers[i], false);

            //캐릭터 이미지는 보이도록 설정
            //gameObject : 이 스크립트 컴포넌트를 가지고 있는 GameObject를 뜻함

            //처음부터 모든 캐릭터 이미지들이 SetActive된다면, false로 변경해주기.
            speakers[i].imageCharacter.gameObject.SetActive(true);
            speakers[i].textBG.gameObject.SetActive(true);

        }
    }

    // 1. DialogueSystem 클래스를 가지는 IntroDialogueSystem 1,2,3이 -> IntroDialogue에서 적었던 순서대로 잘 실행되는가?
    // 2. DialogueSystem 안에서, dialogues[index].isChoose 부분에서, 다음 대사 진행 전 isChoose 불리언 값 여부 조사




    //isChoose 불리언 값을 true/false 조사하는게 0번째 텍스트부터 이루어지는게 아니라, 1번째 텍스트부터 이루어진다.
    //그렇기 때문에, 0번째 텍스트의 isChoose 불리언 값을 조사할 수 없다 => 근데, 현재 게임에서는 0번째 값이 True라서, UpdateDialogue할때 

    public bool UpdateDialogue()
    {
        //처음 시작했을 때, 즉 Load 불리언 ,Init 용도고 첫 번째 대사를 바로 띄울지 아니면 그 다음 Input.마우스 클릭에서 띄울지 정함.
        if (isFirst == true)
        {
            // 초기화. 캐릭터 이미지는 활성화하고, 대사 관련 UI는 모두 비활성화
            Init();
            
            SetNextDialogue();

            // 자동 재생(isAutoStart=true)으로 설정되어 있으면 첫 번째 대사 재생
            if (isAutoStart == true)
            {
               
            }

            isFirst = false;
        }
        
        //강제 스킵 여부 확인 , 강제 스킵 하지 않을 경우 + isFirst에서 isAutoStart가 false였을 경우 마지막 if문 실행

        //Input.GetMouseButtonUp(0) -> 한번 클릭마다 진행 (Auto 진행을 껐으므로 )
        if (Input.GetMouseButtonUp(0))
        {
            // 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
            if (isTypingEffect == true)
            {
                // 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
                isTypingEffect = false;
                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text = dialogues[currentDialogueIndex].dialogue;
                // 대사가 완료되었을 때 출력되는 커서 활성화
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);

                return false;
            }



            // 대사가 남아있을 경우 다음 대사 진행
            if (dialogues.Length > currentDialogueIndex + 1)
            {
                SetNextDialogue();
            }
            // 대사가 더 이상 없을 경우 모든 오브젝트를 비활성화하고 true 반환
            else
            {
                // 현재 대화에 참여했던 모든 캐릭터, 대화 관련 UI를 보이지 않게 비활성화
                for (int i = 0; i < speakers.Length; i++)
                {
                    SetActiveObjects(speakers[i], false);

                    //Init() 과 다르게 캐릭터 이미지 또한 보이지 않도록 설정
                    //이걸 Init()으로 대체하기, 혹은 이때 DialoguePopup을 닫아도 될듯.
                    speakers[i].imageCharacter.gameObject.SetActive(false);
                    speakers[i].textBG.gameObject.SetActive(false);

                }

                return true;
            }
        }

        return false;
    }
    
    //실제 대사 처리는 0번째 대사부터라도 여기서 이루어진다. -> 앞의 Speaker 부분은 UpdateDialogue()에서 세팅된다.
    private void SetNextDialogue()
    {
        if (dialogues[currentDialogueIndex+1].isChoose == true)
        {
            // 플레이어 : wait... 대사를 2초간 코루틴으로 정지시킨 뒤, 관련된 Choice 실행
            // static branch를 choice branch와 동일하게 맞춰줘야 한다.

            //ex ) IntroDialogueSystem2.branch = UI_ChoicePopup.branch
                //이 로직을 실행하기 위해서, ChoicePopup 만의 branch인 nowBranch. ( 사실상 nowBranch가 choiceBranch )
                // 이때 , 그렇다면 굳이 IntroDialogueSystem을 static으로 가져 올 필요가 없었음.

            UI_ChoicePopup.nowBranch = branch;
            isChooseEnd = false;
            Managers.UI.ShowPopupUI<UI_ChoicePopup>();
            StartCoroutine(WaitUntilChoice());
            //이 코루틴을 ChoicePopup에 넣어야겠네 ㅈㅅㅈㅅ;

            isChooseEnd = false;
        }

        //이전 화자의 대화 관련 오브젝트 비활성화
        SetActiveObjects(speakers[currentSpeakerIndex], false);
            // 다음 대사를 진행하도록 
            currentDialogueIndex++;
            // 현재 화자 순번 설정
            currentSpeakerIndex = dialogues[currentDialogueIndex].speakerIndex;

            //현재 화자의 대화 관련 오브젝트 활성화
            SetActiveObjects(speakers[currentSpeakerIndex], true);
            // 현재 화자의 이름 텍스트 설정
            speakers[currentSpeakerIndex].textName.text = dialogues[currentDialogueIndex].name;
            //현재 화자의 대사 텍스트 설정
            speakers[currentSpeakerIndex].textDialogue.text = dialogues[currentDialogueIndex].dialogue;

            StartCoroutine("OnTypingText");
        
    }

    private void SetActiveObjects(Speaker speaker, bool visible)
    {
        speaker.imageDialogue.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);
        speaker.textBG.gameObject.SetActive(visible);


        //화살표는 대사가 종료되었을 때만 활성화하기 땜누에 항상 false
        speaker.objectArrow.SetActive(false);

        //캐릭터 imageCharacter 알파 값 변경
        Color color = speaker.imageCharacter.color;
        color.a = visible == true ? 1 : 0.2f;
        speaker.imageCharacter.color = color;

    }

    private IEnumerator OnTypingText()
    {
        int index = 0;

        isTypingEffect = true;

        // 텍스트를 한글자씩 타이핑치듯 재생
        while (index < dialogues[currentDialogueIndex].dialogue.Length)
        {
            speakers[currentSpeakerIndex].textDialogue.text = dialogues[currentDialogueIndex].dialogue.Substring(0, index);

            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        // 대사가 완료되었을 때 출력되는 커서 활성화
        speakers[currentSpeakerIndex].objectArrow.SetActive(true);

    }
    IEnumerator WaitUntilChoice()
    {
        yield return new WaitUntil(() => isChooseEnd==true);
    }


}

//클래스는 생성하자마자 초기화가 가능한데 구조체는 생성도 안되고 초기화도 안됨. 흠.. 데이터를 실수 안하도록 더 빡빡하게 가능
//잘못 만들면 바로 오류 뱉어냄 ㅇㅇ 

//코루틴 물건입니다 디자인도 되고 로직도 되고

//Start 시점과 Awake 시점이 차이가 좀 많이 난다. 이정도일줄은 몰랐다 ㅇㅇ ㅇㅋㅇㅋ

//부모까지 세팅하고 기본기능+a 이거 좀 개꿀인듯?
//더군다나 MakeSubItem 이름 바꿔서 사용가능해서 다른 프로젝트에서 사용할