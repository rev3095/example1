using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Define;
using DG.Tweening;

// ===== 2023.5.18 Comment =====

//1. UI_Scene으로 하고, UI_Top과의 Sorting은 어떻게 처리할것인가? 그래서 그냥 UI_Base로만 Bind함수를 이용하고, sorting은 직접 Inspector창에서 테스트하면서 처리할 수 밖에 없다 -> 하지만 근본적인 개념 부족으로 발생하는 문제인듯.
//2. 결국 층이 바뀔때마다 Naming을 10F 이렇게 바꾸고 , UI_Main_10F , UI_Top_10F 변경해줘야되긴 함, 이건 kor Replace할때 같이 작업해주기.
//3. Doors와 MovePoint는 프리펩으로 다시 변경해주기. -> 바로 변경 안해도 한번에 shift 클릭으로 변경해주면 되기 때문에 후순위 작업.

//===============================

public class UI_Main : UI_Base
{
    #region enums
    public enum Buttons
    {
        MP_1 =0,
        MP_2,
        MP_3,
        MP_4,
        MP_5,
        MP_6,
        MP_7,
        MP_8,
        MP_9,
        MP_51,
        MP_52,
        Door_1013,
        Door_1012,
        Door_1011,
        Door_1010,
        Door_1009,
        Door_1008,
        Door_1007,
        Door_1006,
        Door_1005,
        Door_1004,
        Door_1003,
        Door_1002,
        Door_1001,
        Door_10_1,
        Door_10_2,
        Door_10_3,
        Door_10_4,
        Door_10_5,
    }

    enum GameObjects
    {
        _MovePoints,
        PlayerLocation,
        Rooms,
        Doors,
    }

    enum Images
    {
        Room_1013 =0,
        Room_1012,
        Room_1011,
        Room_1010,
        Room_1009,
        Room_1008,
        Room_1007,
        Room_1006,
        Room_1005,
        Room_1004,
        Room_1003,
        Room_1002,
        Room_1001,
        Room_10_1,
        Room_10_2,
        Room_10_3,
        Room_10_4,
        Room_10_5,
    }

    #endregion
    public float lerpDuration = 1.0f;

    public static Transform[] possiblePosition_10F = new Transform[MAX_MOVEPOINT_COUNT];
    public GameObject nowPlayerObj;
    public LobbyCameraController lobbyCameraController = new LobbyCameraController();

    private Button[] _MovePoints = new Button[11];
    private Button[] _Doors = new Button[18];

    private const int MAX_MOVEPOINT_COUNT = 11;
    private const int MIN_DOOR_COUNT = 11;
    private const int MAX_DOOR_COUNT = 28;


    public void Update()
    {
        Check_MovePointstate();
        CheckDoorState();
    }
    public override void Init()
    {
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        #region GetButton_MP
        GetButton((int)Buttons.MP_1).gameObject.BindEvent(OnClickMP_1);
        GetButton((int)Buttons.MP_2).gameObject.BindEvent(OnClickMP_2);
        GetButton((int)Buttons.MP_3).gameObject.BindEvent(OnClickMP_3);
        GetButton((int)Buttons.MP_4).gameObject.BindEvent(OnClickMP_4);
        GetButton((int)Buttons.MP_5).gameObject.BindEvent(OnClickMP_5);
        GetButton((int)Buttons.MP_6).gameObject.BindEvent(OnClickMP_6);
        GetButton((int)Buttons.MP_7).gameObject.BindEvent(OnClickMP_7);
        GetButton((int)Buttons.MP_8).gameObject.BindEvent(OnClickMP_8);
        GetButton((int)Buttons.MP_9).gameObject.BindEvent(OnClickMP_9);
        GetButton((int)Buttons.MP_51).gameObject.BindEvent(OnClickMP_51);
        GetButton((int)Buttons.MP_52).gameObject.BindEvent(OnClickMP_52);
        #endregion

        #region GetButton_Door
        GetButton((int)Buttons.Door_1001).gameObject.BindEvent(OnClickDoor_1001);
        GetButton((int)Buttons.Door_1002).gameObject.BindEvent(OnClickDoor_1002);
        GetButton((int)Buttons.Door_1003).gameObject.BindEvent(OnClickDoor_1003);
        GetButton((int)Buttons.Door_1004).gameObject.BindEvent(OnClickDoor_1004);
        GetButton((int)Buttons.Door_1005).gameObject.BindEvent(OnClickDoor_1005);
        GetButton((int)Buttons.Door_1006).gameObject.BindEvent(OnClickDoor_1006);
        GetButton((int)Buttons.Door_1007).gameObject.BindEvent(OnClickDoor_1007);
        GetButton((int)Buttons.Door_1008).gameObject.BindEvent(OnClickDoor_1008);
        GetButton((int)Buttons.Door_1009).gameObject.BindEvent(OnClickDoor_1009);
        GetButton((int)Buttons.Door_1010).gameObject.BindEvent(OnClickDoor_1010);
        GetButton((int)Buttons.Door_1011).gameObject.BindEvent(OnClickDoor_1011);
        GetButton((int)Buttons.Door_1012).gameObject.BindEvent(OnClickDoor_1012);
        GetButton((int)Buttons.Door_1013).gameObject.BindEvent(OnClickDoor_1013);
        GetButton((int)Buttons.Door_10_1).gameObject.BindEvent(OnClickDoor_10_1);
        GetButton((int)Buttons.Door_10_2).gameObject.BindEvent(OnClickDoor_10_2);
        GetButton((int)Buttons.Door_10_3).gameObject.BindEvent(OnClickDoor_10_3);
        GetButton((int)Buttons.Door_10_4).gameObject.BindEvent(OnClickDoor_10_4);
        GetButton((int)Buttons.Door_10_5).gameObject.BindEvent(OnClickDoor_10_5);
        #endregion

        CheckRoomState();

        //LobbyCameraController에서 순서 문제로 이동
        for (int i = 0; i < MAX_MOVEPOINT_COUNT; i++)
        {
            possiblePosition_10F[i] = GetButton((int)(UI_Main.Buttons)i).transform;
            _MovePoints[i] = GetButton((int)(UI_Main.Buttons)i);
        }
        for(int i = MIN_DOOR_COUNT; i< MAX_DOOR_COUNT+1; i++)
        {
            _Doors[i-MIN_DOOR_COUNT] = GetButton((int)(UI_Main.Buttons)i);
        }

        Check_MovePointstate();

        lobbyCameraController.SetCamera();
        nowPlayerObj.transform.position = new Vector3(possiblePosition_10F[Managers.Game.nowMP].position.x, possiblePosition_10F[Managers.Game.nowMP].position.y, 0);

        StartCoroutine(CoSaveGame(3.0f));

    }

    #region OnClickMP

    void CheckMoveEvent()
    {
        MoveEventData data = Managers.Game.GetMoveEvent();
        if (data.ID != 0)
        {
            Managers.UI.ShowPopupUI<UI_MoveEventPopup>().SetInfo(data);
        }

    }

    //nowMP -> Button transform position -> MoveCamera -> DOTween -> Event Code (MoveEventData)

    void OnClickMP_1(PointerEventData data)
    {
        Managers.Game.nowMP = 0;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_1).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

        CheckMoveEvent();
    }
    void OnClickMP_2(PointerEventData data)
    {
        Managers.Game.nowMP = 1;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_2).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();
        CheckMoveEvent();

    }
    void OnClickMP_3(PointerEventData data)
    {
        Managers.Game.nowMP = 2;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_3).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();
        CheckMoveEvent();



    }
    void OnClickMP_4(PointerEventData data)
    {
        Managers.Game.nowMP = 3;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_4).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

        CheckMoveEvent();


    }
    void OnClickMP_5(PointerEventData data)
    {
        Managers.Game.nowMP = 4;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_5).transform.position, lerpDuration);

        lobbyCameraController.MoveCamera();
    }
    void OnClickMP_51(PointerEventData data)
    {
        Managers.Game.nowMP = 9;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_51).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

    }
    void OnClickMP_52(PointerEventData data)
    {
        Managers.Game.nowMP = 10;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_52).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

    }
    void OnClickMP_6(PointerEventData data)
    {
        Managers.Game.nowMP = 5;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_6).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

    }
    void OnClickMP_7(PointerEventData data)
    {
        Managers.Game.nowMP = 6;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_7).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

    }
    void OnClickMP_8(PointerEventData data)
    {
        Managers.Game.nowMP = 7;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_8).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

    }
    void OnClickMP_9(PointerEventData data)
    {
        Managers.Game.nowMP = 8;
        nowPlayerObj.transform.DOMove(GetButton((int)Buttons.MP_9).transform.position, lerpDuration);
        lobbyCameraController.MoveCamera();

    }
    #endregion

    #region OnClickDoor
    void OnClickDoor_1013(PointerEventData data)
    {
        Managers.Game.CurRoom = 0;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;
        CheckRoomState();
    }
    void OnClickDoor_1012(PointerEventData data)
    {
        Managers.Game.CurRoom = 1;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;
        CheckRoomState();
    }
    void OnClickDoor_1011(PointerEventData data)
    {
        Managers.Game.CurRoom = 2;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1010(PointerEventData data)
    {
        Managers.Game.CurRoom = 3;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1009(PointerEventData data)
    {
        Managers.Game.CurRoom = 4;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1008(PointerEventData data)
    {
        Managers.Game.CurRoom = 5;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }

    void OnClickDoor_1007(PointerEventData data)
    {
        Managers.Game.CurRoom = 6;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1006(PointerEventData data)
    {
        Managers.Game.CurRoom = 7;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1005(PointerEventData data)
    {
        Managers.Game.CurRoom = 8;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1004(PointerEventData data)
    {
        Managers.Game.CurRoom = 9;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1003(PointerEventData data)
    {
        Managers.Game.CurRoom = 10;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1002(PointerEventData data)
    {
        Managers.Game.CurRoom = 11;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_1001(PointerEventData data)
    {
        Managers.Game.CurRoom = 12;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_10_1(PointerEventData data)
    {
        Managers.Game.CurRoom = 13;
        Managers.Game.checkedRoom[Managers.Game.CurRoom] = Define.CheckedRoom.Checked;

        CheckRoomState();

    }
    void OnClickDoor_10_2(PointerEventData data)
    {
        Managers.Game.CurRoom = 14;
        CheckRoomState();

    }
    void OnClickDoor_10_3(PointerEventData data)
    {
        Managers.Game.CurRoom = 15;
        CheckRoomState();

    }
    void OnClickDoor_10_4(PointerEventData data)
    {
        Managers.Game.CurRoom = 16;
        CheckRoomState();

    }
    void OnClickDoor_10_5(PointerEventData data)
    {
        Managers.Game.CurRoom = 17;
        CheckRoomState();

    }

    #endregion

    public void Interaction()
    {

    }



    #region UI Refresh / Enable MovePoint,Door
    public void CheckRoomState()
    {
        //OnClick_Door 해당 코드 -> 어차피 이미 클릭된 상태라면 아래 for문에서 확인될 것.
        if (Managers.Game.checkedRoom[Managers.Game.CurRoom] != CheckedRoom.Default)
        {
            Image nowRoomImage = GetImage(((int)(Images)Managers.Game.CurRoom));
            nowRoomImage.color = new Color32(255, 255, 255, 255);

        }

        //시작 전 SaveData의 checkedRoom[] 데이터를 기반한 RoomImage 컬러값 세팅
        for (int i = 0; i < MAX_ROOM_COUNT_10F; i++)
        {
            if (Managers.Game.checkedRoom[i] != CheckedRoom.Default)
            {
                Image nowRoomImage = GetImage(((int)(Images)i));
                nowRoomImage.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                Image nowRoomImage = GetImage(((int)(Images)i));
                nowRoomImage.color = new Color32(100, 100, 100, 255);
            }

        }

    }

    public void Check_MovePointstate()
    {
        int idx = Managers.Game.nowMP;

        //raycastTarget off : 이미지는 있지만, 클릭은 안된다 (UI_EventHandler을 껐다는 의미와 같다.)
        void enabledAll()
        {
            for (int i = 0; i < 11; i++)
            {
                _MovePoints[i].gameObject.GetComponent<Image>().raycastTarget = false;
            }
        }

        switch (idx)
        {

            case 0:
                enabledAll();
                _MovePoints[1].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 1:
                enabledAll();
                _MovePoints[0].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[2].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 2:
                enabledAll();
                _MovePoints[1].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[3].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 3:
                enabledAll();
                _MovePoints[2].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[4].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 4:
                enabledAll();
                _MovePoints[3].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[5].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[9].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 5:
                enabledAll();
                _MovePoints[4].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[6].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 6:
                enabledAll();
                _MovePoints[5].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[7].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 7:
                enabledAll();
                _MovePoints[6].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[8].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 8:
                enabledAll();
                _MovePoints[7].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 9:
                enabledAll();
                _MovePoints[4].gameObject.GetComponent<Image>().raycastTarget = true;
                _MovePoints[10].gameObject.GetComponent<Image>().raycastTarget = true;
                break;

            case 10:
                enabledAll();
                _MovePoints[9].gameObject.GetComponent<Image>().raycastTarget = true;
                break;
        }
        
    }

    public void CheckDoorState()
    {
        int idx = Managers.Game.nowMP;
        void enabledAll()
        {
            for (int i = 0; i < 11; i++)
            {
                _MovePoints[i].gameObject.GetComponent<Image>().raycastTarget = false;
            }
        }
    }
    #endregion


    #region CoSaveGame
    //DD처럼 진행상황 저장에 대한 주도권을 플레이어에게 제공하지 않음.
    IEnumerator CoSaveGame(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Managers.Game.SaveGame();
        }
    }
    #endregion

}

//RoomData는 기본 고유 Room값 , ROom에서 등장하는 특정 몬스터나 아이템에 관한 정보가 있다는 것
//InteractionData는 Door와 상호작용했을때 나오는 이벤트 -> Door가 OnClick 되었을때
//MoveEventData는 한번 움직일때마다 실행되는 함수로써 동작하고 주어진 확률에 맞게 데이터의 결과값들이 실행된다.
    //-> 관련 함수는 MP가 OnClick 되었을때 (*구조상 GM에서 작성되는게 맞지만, 함수만 작성하고 함수 자체는 Main에서 실행시키자. 그럼 좀더 명시적인 코드가 될 것 같음.)

//구조적으롭 봤을때 추상 클래스를 만든다음에 , 룸이든 MP든 관련 세부 함수를 작성하는게 맞지만, 생략한다.