using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TutorialPopup : UI_Popup
{
    #region const
    [SerializeField] const int MAX_LOBBY_COUNT = 7;
    [SerializeField] const int MAX_ROOM_COUNT = 7;
    [SerializeField] const int MAX_SELECTION_COUNT = 7;
    [SerializeField] const int MAX_COMBAT_COUNT = 12;
    #endregion


    [SerializeField] private GameObject UI_10F;

    [SerializeField] private Sprite[] _Lobby = new Sprite[MAX_LOBBY_COUNT];
    [SerializeField] private Sprite[] _Room = new Sprite[MAX_ROOM_COUNT];
    [SerializeField] private Sprite[] _Selection = new Sprite[MAX_SELECTION_COUNT];
    [SerializeField] private Sprite[] _Combat = new Sprite[MAX_COMBAT_COUNT];

    TutorialType _type;
    bool _end = false;
    int _idx = 0;

    #region Enum TutorialType/Buttons/Images/GameObjects
    enum TutorialType
    {
        None = 0,
        Lobby,
        Room,
        Selection,
        Combat
    }
    enum Buttons
    {
        NextButton,
        LobbyButton,
        RoomButton,
        TeamSelectionButton,
        CombatButton,
        CloseButton1,
        CloseButton2,
    }

    enum Images
    {
        TutorialImage,

    }

    enum GameObjects
    {
        TutorialSelection,
    }

    #endregion

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.ClosePopupUI(this);
        }

    }

    public override void Init()
    {
        base.Init();

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.LobbyButton).gameObject.BindEvent(PointerEventData => OnClickLobbyTutorial());
        GetButton((int)Buttons.RoomButton).gameObject.BindEvent(PointerEventData => OnClickRoomTutorial());
        GetButton((int)Buttons.TeamSelectionButton).gameObject.BindEvent(PointerEventData => OnClickTeamSelectionTutorial());
        GetButton((int)Buttons.CombatButton).gameObject.BindEvent(PointerEventData => OnClickCombatTutorial());
        GetButton((int)Buttons.NextButton).gameObject.BindEvent(PointerEventData => OnClickNextButton());
        GetButton((int)Buttons.CloseButton1).gameObject.BindEvent(PointerEvetnData => OnClickCloseButton());
        GetButton((int)Buttons.CloseButton2).gameObject.BindEvent(PointerEvetnData => OnClickCloseButton());

        GetButton((int)Buttons.NextButton).gameObject.SetActive(false);

        _idx = 1;

    }

    #region OnClick Button
    void OnClickLobbyTutorial()
    {
        ShowTutorial(TutorialType.Lobby);
    }
    void OnClickRoomTutorial()
    {
        ShowTutorial(TutorialType.Room);
    }

    void OnClickTeamSelectionTutorial()
    {
        ShowTutorial(TutorialType.Selection);
    }
    void OnClickCombatTutorial()
    {

        ShowTutorial(TutorialType.Combat);
    }

    void OnClickNextButton()
    {
        if(_end != true)
            NextImage();
    }
    void OnClickCloseButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    #endregion

    private void ShowTutorial(TutorialType type)
    {
        //Init
        UI_10F.SetActive(false);
        GetObject((int)GameObjects.TutorialSelection).gameObject.SetActive(false);
        GetButton((int)Buttons.CloseButton1).gameObject.SetActive(false);
        GetButton((int)Buttons.CloseButton2).gameObject.SetActive(false);

        GetButton((int)Buttons.NextButton).gameObject.SetActive(true);

        switch (type)
        {
            case TutorialType.Lobby:
                GetImage((int)Images.TutorialImage).sprite = _Lobby[0];
                break;

            case TutorialType.Room:
                GetImage((int)Images.TutorialImage).sprite = _Room[0];
                break;

            case TutorialType.Selection:
                GetImage((int)Images.TutorialImage).sprite = _Selection[0];
                break;

            case TutorialType.Combat:
                GetImage((int)Images.TutorialImage).sprite = _Combat[0];
                break;

            default:
                break;
        }

        _idx = 1;
        _type = type;

    }

    private void NextImage()
    {
        if (_end == true)
        {
            GetButton((int)Buttons.NextButton).gameObject.SetActive(false);
            GetObject((int)GameObjects.TutorialSelection).gameObject.SetActive(true);
            UI_10F.SetActive(true);
            GetButton((int)Buttons.CloseButton1).gameObject.SetActive(true);
            GetButton((int)Buttons.CloseButton2).gameObject.SetActive(true);

            _end = false; //flag
            _idx = 1;

        }

        switch (_type)
        {
            case TutorialType.Lobby:

                if(MAX_LOBBY_COUNT == _idx)
                {
                    _end = true;
                    _idx = 1;
                    break;
                }
                GetImage((int)Images.TutorialImage).sprite = _Lobby[_idx];
                _idx++;
                break;

            case TutorialType.Room:

                if (MAX_ROOM_COUNT == _idx)
                {
                    _end = true;
                    _idx = 1;
                    break;
                }
                GetImage((int)Images.TutorialImage).sprite = _Room[_idx];
                _idx++;
                break;

            case TutorialType.Selection:

                if (MAX_SELECTION_COUNT == _idx)
                {
                    _end = true;
                    _idx = 1;
                    break;
                }
                GetImage((int)Images.TutorialImage).sprite = _Selection[_idx];
                _idx++;
                break;
                
            case TutorialType.Combat:

                if (MAX_COMBAT_COUNT == _idx)
                {
                    _end = true;
                    _idx = 1;
                    break;
                }
                GetImage((int)Images.TutorialImage).sprite = _Combat[_idx];
                _idx++;
                break;

            default:
                break;

        }

        if (_end == true)
        {
            GetButton((int)Buttons.NextButton).gameObject.SetActive(false);
            GetObject((int)GameObjects.TutorialSelection).gameObject.SetActive(true);
            UI_10F.SetActive(true);
            GetButton((int)Buttons.CloseButton1).gameObject.SetActive(true);
            GetButton((int)Buttons.CloseButton2).gameObject.SetActive(true);

            _end = false; //flag

            _idx = 1;
        }
    }
}
