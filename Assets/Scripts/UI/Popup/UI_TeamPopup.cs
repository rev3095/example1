using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static Define;
using TMPro;


#region LOG
/* ===================
 * 최대한 주석을 제외하기 위해, 주석 관련 세부 내용은 LOG에 정리.
    #6.18
    [구현 필요 List]
    1. Player Spine 적용 ( 멘토링 받은 Spine Structure 적용 )
    UI 프리펩 요소에서 Spine UI? Animation? (Animation밖에 안 된다면, UI는 왜 있는거임? )

    2. if) Upgrade Kit으로 강화되었다면, 강회된 스킬아이콘으로 변경 -> Excel PlayerSkillData 마지막에 upgradeIconPath 추가하기.
        //여담이지만, UI_InvenPopup의 Visual Items도 결국 위치가 정해져있으므로, TeamPopup과 동일한 구조로 제작하기.
        //다만 구조는, AlicePang처럼 동적 생성을 하되, 위치가 정해짐 (어차피 엑셀 Dict 순서대로 생성되므로) && Lock/UnLock도 관리하기.
    
 * ===================*/
#endregion

public class UI_TeamPopup : UI_Popup
{
    enum Texts
    {
        KSkillTitleText,
        KSkillBeforeText,
        KSkillAfterText,

        ChloeSkillTitleText,
        ChloeSkillBeforeText,
        ChloeSkillAfterText,

        SvanaSkillTitleText,
        SvanaSkillBeforeText,
        SvanaSkillAfterText,

        KPossesionValueText,
        ChloePossesionValueText,
        SvanaPossesionValueText,

    }

    enum Images
    {
        TeamButtonSelect1 = 0, TeamButtonSelect2, TeamButtonSelect3,

        KSkill1 = 3, KSkill2, KSkill3, KSkill4, KSkill5, KSkill6, KSkill7, KSkill8,
        CSkill1, CSkill2, CSkill3, CSkill4, CSkill5, CSkill6, CSkill7, CSkill8,
        SSkill1, SSkill2, SSkill3, SSkill4, SSkill5, SSkill6, SSkill7, SSkill8 =26,
    }

    enum Buttons
    {
        CloseButton =0 ,

        TeamButton1,
        TeamButton2,
        TeamButton3,

        K1 =4, K2, K3, K4, K5, K6, K7, K8 =11,
        C1 =12 , C2, C3, C4, C5, C6, C7, C8,
        S1 =20, S2, S3, S4, S5, S6, S7, S8 = 27,

        KUseButton,
        ChloeUseButton,
        SvanaUseButton,
    }

    enum GameObjects
    {
        KTab,
        ChloeTab,
        SvanaTab,

    }
    public enum CharacterList
    {
        None = 0,
        K,
        Chloe,
        Svana,
    }

    private CharacterList _list = CharacterList.None;

    public static int number = 0;


    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent((PointerEventData data) => Managers.UI.ClosePopupUI(this));
        GetButton((int)Buttons.TeamButton1).gameObject.BindEvent((PointerEventData data) => Showtab(CharacterList.K));
        GetButton((int)Buttons.TeamButton2).gameObject.BindEvent((PointerEventData data) => Showtab(CharacterList.Chloe));
        GetButton((int)Buttons.TeamButton3).gameObject.BindEvent((PointerEventData data) => Showtab(CharacterList.Svana));

        #region GetButton(4 - 27)
        GetButton(4).gameObject.BindEvent((PointerEventData => OnClickKSkill1()));
        GetButton(5).gameObject.BindEvent((PointerEventData => OnClickKSkill2()));
        GetButton(6).gameObject.BindEvent((PointerEventData => OnClickKSkill3()));
        GetButton(7).gameObject.BindEvent((PointerEventData => OnClickKSkill4()));
        GetButton(8).gameObject.BindEvent((PointerEventData => OnClickKSkill5()));
        GetButton(9).gameObject.BindEvent((PointerEventData => OnClickKSkill6()));
        GetButton(10).gameObject.BindEvent((PointerEventData => OnClickKSkill7()));
        GetButton(11).gameObject.BindEvent((PointerEventData => OnClickKSkill8()));

        GetButton(12).gameObject.BindEvent((PointerEventData => OnClickChloeSkill1()));
        GetButton(13).gameObject.BindEvent((PointerEventData => OnClickChloeSkill2()));
        GetButton(14).gameObject.BindEvent((PointerEventData => OnClickChloeSkill3()));
        GetButton(15).gameObject.BindEvent((PointerEventData => OnClickChloeSkill4()));
        GetButton(16).gameObject.BindEvent((PointerEventData => OnClickChloeSkill5()));
        GetButton(17).gameObject.BindEvent((PointerEventData => OnClickChloeSkill6()));
        GetButton(18).gameObject.BindEvent((PointerEventData => OnClickChloeSkill7()));
        GetButton(19).gameObject.BindEvent((PointerEventData => OnClickChloeSkill8()));

        GetButton(20).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill1()));
        GetButton(21).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill2()));
        GetButton(22).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill3()));
        GetButton(23).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill4()));
        GetButton(24).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill5()));
        GetButton(25).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill6()));
        GetButton(26).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill7()));
        GetButton(27).gameObject.BindEvent((PointerEventData => OnClickSvanaSkill8()));

        #endregion

        for(int a =4; a<28; a++)
        {
            GetButton(a).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        #region Image Binding

        //강화된 스킬은 skill Icon이 바뀌므로 for문에서 if로 하나씩 처리해서 가져가기, 그렇기에 엑셀파일에 강화된 스킬아이콘도 필요함.

        string[] datas = new string[24];

        for (int i = 1; i<25; i++)
        {
            CharacterSkillData data = Managers.Data.CharacterSkillDict[i];

            datas[i-1] = data.iconPath;
        }

        
        GetImage(3).sprite = Managers.Resource.Load<Sprite>(datas[0]);
        GetImage(4).sprite = Managers.Resource.Load<Sprite>(datas[1]);
        GetImage(5).sprite = Managers.Resource.Load<Sprite>(datas[2]);
        GetImage(6).sprite = Managers.Resource.Load<Sprite>(datas[3]);
        GetImage(7).sprite = Managers.Resource.Load<Sprite>(datas[4]);
        GetImage(8).sprite = Managers.Resource.Load<Sprite>(datas[5]);
        GetImage(9).sprite = Managers.Resource.Load<Sprite>(datas[6]);
        GetImage(10).sprite = Managers.Resource.Load<Sprite>(datas[7]);
        GetImage(11).sprite = Managers.Resource.Load<Sprite>(datas[8]);
        GetImage(12).sprite = Managers.Resource.Load<Sprite>(datas[9]);
        GetImage(13).sprite = Managers.Resource.Load<Sprite>(datas[10]);
        GetImage(14).sprite = Managers.Resource.Load<Sprite>(datas[11]);
        GetImage(15).sprite = Managers.Resource.Load<Sprite>(datas[12]);
        GetImage(16).sprite = Managers.Resource.Load<Sprite>(datas[13]);
        GetImage(17).sprite = Managers.Resource.Load<Sprite>(datas[14]);
        GetImage(18).sprite = Managers.Resource.Load<Sprite>(datas[15]);
        GetImage(19).sprite = Managers.Resource.Load<Sprite>(datas[16]);
        GetImage(20).sprite = Managers.Resource.Load<Sprite>(datas[17]);
        GetImage(21).sprite = Managers.Resource.Load<Sprite>(datas[18]);
        GetImage(22).sprite = Managers.Resource.Load<Sprite>(datas[19]);
        GetImage(23).sprite = Managers.Resource.Load<Sprite>(datas[20]);
        GetImage(24).sprite = Managers.Resource.Load<Sprite>(datas[21]);
        GetImage(25).sprite = Managers.Resource.Load<Sprite>(datas[22]);
        GetImage(26).sprite = Managers.Resource.Load<Sprite>(datas[23]);

        #endregion


        GetText((int)Texts.KPossesionValueText).text = $"{Managers.Game.UpgradePossesion}";
        GetText((int)Texts.ChloePossesionValueText).text = $"{Managers.Game.UpgradePossesion}";
        GetText((int)Texts.SvanaPossesionValueText).text = $"{Managers.Game.UpgradePossesion}";


        GetObject((int)GameObjects.KTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.ChloeTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.SvanaTab).gameObject.SetActive(false);

        GetImage((int)Images.TeamButtonSelect1).gameObject.SetActive(false);
        GetImage((int)Images.TeamButtonSelect2).gameObject.SetActive(false);
        GetImage((int)Images.TeamButtonSelect3).gameObject.SetActive(false);

    }


    #region OnClickSkillSlot(4-27)

    //Image를 .enabled = false; 로 해버리면, Image의 Raycast Target까지 off되어 클릭이 먹히지 않는다 -> 그래서 그냥 컬러값만 변경해주기.
    public void SetSkillInfo(int idx ,string charIdx)
    {

        CharacterSkillData data = Managers.Data.CharacterSkillDict[idx];

        double DMG = data.applyDmg;
        double CRT = data.difCrt;
        double ACC = data.difAcc;

        switch (charIdx)
        {
            case "K":
                GetText((int)Texts.KSkillBeforeText).text = $"MELEE / DMG {DMG * 100}% / CRT {CRT*100}% / {ACC*100}% DMG VS MARK";
                GetText((int)Texts.KSkillAfterText).text = $"MELEE / DMG {DMG * 150}% / CRT {CRT * 150}% / {ACC * 150}% DMG VS MARK";
                break;

            case "C":
                GetText((int)Texts.ChloeSkillBeforeText).text = $"MELEE / DMG {DMG * 100}% / CRT {CRT * 100}% / {ACC * 100}% DMG VS MARK";
                GetText((int)Texts.ChloeSkillAfterText).text = $"MELEE / DMG {DMG * 150}% / CRT {CRT * 150}% / {ACC * 150}% DMG VS MARK";
                break;

            case "S":
                GetText((int)Texts.SvanaSkillBeforeText).text = $"MELEE / DMG {DMG * 100}% / CRT {CRT * 100}% / {ACC * 100}% DMG VS MARK";
                GetText((int)Texts.SvanaSkillAfterText).text = $"MELEE / DMG {DMG * 150}% / CRT {CRT * 150}% / {ACC * 150}% DMG VS MARK";
                break;
        }

    }

   
    public void OnClickKSkill1()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);


        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_1);
        SetSkillInfo(1, "K");

    }

    public void OnClickKSkill2()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K2).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_2);
        SetSkillInfo(2, "K");

    }
    public void OnClickKSkill3()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K3).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_3);
        SetSkillInfo(3, "K");

    }
    public void OnClickKSkill4()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K4).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_4);
        SetSkillInfo(4, "K");

    }

    public void OnClickKSkill5()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K5).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_5);
        SetSkillInfo(5, "K");

    }
    public void OnClickKSkill6()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K6).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_6);
        SetSkillInfo(6, "K");

    }
    public void OnClickKSkill7()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K7).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_7);
        SetSkillInfo(7, "K");

    }
    public void OnClickKSkill8()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.K8).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.KSkillTitleText).text = Managers.GetText(Define.KTitle_8);
        SetSkillInfo(8, "K");

    }
    public void OnClickChloeSkill1()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_1);
        SetSkillInfo(9, "C");

    }
    public void OnClickChloeSkill2()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C2).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_2);
        SetSkillInfo(10, "C");

    }
    public void OnClickChloeSkill3()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C3).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_3);
        SetSkillInfo(11, "C");

    }
    public void OnClickChloeSkill4()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C4).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_4);
        SetSkillInfo(12, "C");

    }
    public void OnClickChloeSkill5()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C5).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_5);
        SetSkillInfo(13, "C");

    }
    public void OnClickChloeSkill6()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C6).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_6);
        SetSkillInfo(14, "C");

    }
    public void OnClickChloeSkill7()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C7).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_7);
        SetSkillInfo(15, "C");

    }
    public void OnClickChloeSkill8()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.C8).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.ChloeSkillTitleText).text = Managers.GetText(Define.CTitle_8);
        SetSkillInfo(16, "C");

    }
    public void OnClickSvanaSkill1()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_1);
        SetSkillInfo(17, "S");

    }
    public void OnClickSvanaSkill2()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S2).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_2);
        SetSkillInfo(18, "S");

    }
    public void OnClickSvanaSkill3()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S3).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_3);
        SetSkillInfo(19, "S");

    }
    public void OnClickSvanaSkill4()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S4).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_4);
        SetSkillInfo(20, "S");

    }
    public void OnClickSvanaSkill5()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S5).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_5);
        SetSkillInfo(21, "S");

    }
    public void OnClickSvanaSkill6()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S6).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_6);
        SetSkillInfo(22, "S");

    }
    public void OnClickSvanaSkill7()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S7).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_7);
        SetSkillInfo(23, "S");

    }
    public void OnClickSvanaSkill8()
    {
        for (int num = 4; num < 28; num++)
        {
            GetButton(num).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        GetButton((int)Buttons.S8).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        GetText((int)Texts.SvanaSkillTitleText).text = Managers.GetText(Define.STitle_8);
        SetSkillInfo(24, "S");

    }
 


    #endregion

    public void Showtab(CharacterList list)
    {
        if (_list == list)
            return;
        _list = list;

        GetObject((int)GameObjects.KTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.ChloeTab).gameObject.SetActive(false);
        GetObject((int)GameObjects.SvanaTab).gameObject.SetActive(false);

        GetImage((int)Images.TeamButtonSelect1).gameObject.SetActive(false);
        GetImage((int)Images.TeamButtonSelect2).gameObject.SetActive(false);
        GetImage((int)Images.TeamButtonSelect3).gameObject.SetActive(false);

        switch (_list)
        {
            case CharacterList.K:
                GetImage((int)Images.TeamButtonSelect1).gameObject.SetActive(true);
                GetObject((int)GameObjects.KTab).gameObject.SetActive(true);
                break;

            case CharacterList.Chloe:
                GetImage((int)Images.TeamButtonSelect2).gameObject.SetActive(true);
                GetObject((int)GameObjects.ChloeTab).gameObject.SetActive(true);
                break;

            case CharacterList.Svana:
                GetImage((int)Images.TeamButtonSelect3).gameObject.SetActive(true);
                GetObject((int)GameObjects.SvanaTab).gameObject.SetActive(true);
                break;
        }

    }




}
