using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;

public interface IItemData
{
    ItemDataType GetItemType { get; }

    int GetID { get ; }

    string GetIconPath { get; }
}

public enum ItemDataType
{
    None = 0,
    Use,Com,Vis,Acc,
}

public class UI_Inven_Item : UI_Base
{
    #region enum Buttons/Images
    enum Buttons
    {
        UI_Inven_Item,
    }

    #endregion

    [SerializeField] public Image Slot;
    [SerializeField] public Image SlotItem;


    IItemData selectedItemData;
    
    private bool _selected;

    public override void Init()
    {
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.UI_Inven_Item).gameObject.BindEvent(OnClickInvenButton);

    }

    //Question : bind가 Init으로 안되었는가? 그러면, 먼저 ShowPopup해주고, 나중에 정보 셋인포로 순서 맞춰주기.
    //사실 이런걸 방지하기 위해서 이벤트 함수, 즉 Start사용 안하고 Init으로 바로.. 라고하는데 그럼 처음 생성할때 어떻게 처음 실행시킴?
    void OnClickInvenButton(PointerEventData data)
    {
        if (selectedItemData == null)
            return;


        if (ItemManager.selectedItem != this)
            _selected = false;

        if (ItemManager.OnChangedInvenTab == true)
        {
            _selected = false;
            ItemManager.OnChangedInvenTab = false;
        }

        ItemManager.selectedItem = this;

        if(_selected == true)
        {
            if (data.button == PointerEventData.InputButton.Left)
                Managers.UI.ShowPopupUI<UI_InfoPopup>().SetInfo(selectedItemData, 0 , this);
            else if(data.button == PointerEventData.InputButton.Right)
                Managers.UI.ShowPopupUI<UI_InfoPopup>().SetInfo(selectedItemData, 1, this);

            _selected = false;

        }
        else
        {
            _selected = true;
            Managers.Item.OnClickItemButton(this.selectedItemData);
        }
        
    }


    #region Set Item Info (Use/Com/Acc/Vis)
    Sprite _invenSprite;

    //처음 MakeSubItem이 진행될때 실행됨.
    public void SetItemInfo(IItemData _iData)
    {
        this.selectedItemData = _iData;
        Refresh(_iData);
    }
    #endregion

    public void Refresh(IItemData _iData)
    {
        int _id = _iData.GetID;
        string _path = _iData.GetIconPath;
        if (_path != null)
        {
            _invenSprite = Managers.Resource.Load<Sprite>(_path);
            SlotItem.sprite = _invenSprite;
        }
    }

}