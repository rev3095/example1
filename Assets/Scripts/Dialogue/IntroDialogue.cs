using System.Collections;
using UnityEngine;
using TMPro;

public class IntroDialogue : MonoBehaviour
{
    //That point didn't work static -> make errors;
    [SerializeField]
    public DialogueSystem IntroDialogueSystem1;
    [SerializeField]
    public DialogueSystem IntroDialogueSystem2;

    [SerializeField]
    public DialogueSystem IntroDialogueSystem3;

    public void Awake()
    {
        Debug.Log("코루틴관련 코드 실행됨");
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => IntroDialogueSystem1.UpdateDialogue());
        yield return new WaitUntil(() => IntroDialogueSystem2.UpdateDialogue());
        yield return new WaitUntil(() => IntroDialogueSystem3.UpdateDialogue());

        yield return new WaitForSeconds(2);
        EndDialogue();

    }

    public void EndDialogue()
    {
        Managers.Game.SaveData.IsFirstStart = true;
        Managers.UI.CloseAllPopupUI();
        Debug.Log("정상적으로 코루틴 실행 및 완료되었음.");
        Debug.Log(Managers.Game.SaveData.IsFirstStart);
        //Managers.UI.ShowPopupUI<UI_Lobby_10F>();
    }
    /*
    //코루틴 Start() 하면 되는데 , 현재는 Update()로 처리
    private void Update()
    {
        IntroDialogueSystem1.UpdateDialogue();
    }
    */


}