using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
public class UI_InfoPopup : UI_Popup
{
	[SerializeField] private TMP_Text UseTitleText;
	[SerializeField] private TMP_Text TrashTitleText;
	[SerializeField] private Button YesButton;
	[SerializeField] private Button NoButton;

	[SerializeField] private GameObject UseTab; 
	[SerializeField] private GameObject TrashTab;

	[SerializeField] private Image TrashItemImage;

	IItemData selectedItemData;

	UI_Inven_Item _item;

	int _tabIdx;
	int _itemIdx; //배열 요소를 삭제할 idx



    void Start()
    {
        Init();
		Activate();
    }

    public override void Init()
    {
        base.Init();

	}

	private void Activate()
    {
		YesButton.gameObject.BindEvent(OnClickYesButton);
		NoButton.gameObject.BindEvent(OnClickNoButton);

		TrashItemImage.sprite = Managers.Resource.Load<Sprite>(selectedItemData.GetIconPath);

	}

    #region On Click Function

	//Vis,Acc는 사용 아이템이 아니므로 삭제/생성 X , 사용 아이템일 경우 아이콘 정보 리셋(Destroy->생성하지 않음)
    void OnClickYesButton(PointerEventData data)
    {
		ItemDataType _type = selectedItemData.GetItemType;
		_itemIdx = selectedItemData.GetID;

		switch(_type)
        {
			case ItemDataType.Use:
				Managers.Game.UseItemList.Remove(Managers.Data.UseItemDict[_itemIdx]);
				_item.SlotItem.color = new Color(1, 1, 1, 0);
				Managers.Item.Refresh();
				break;

			case ItemDataType.Com:
				Managers.Game.ComItemList.Remove(Managers.Data.ComItemDict[_itemIdx]);
				_item.SlotItem.color = new Color(1, 1, 1, 0);
				Managers.Item.Refresh();
				break;

			default:
				break;
        }

		Managers.UI.ClosePopupUI(this);
    }

	void OnClickNoButton(PointerEventData data)
    {
		Managers.UI.ClosePopupUI(this);
    }

    #endregion


	public void SetInfo(IItemData _Data, int IDX, UI_Inven_Item item)
    {		
		selectedItemData = _Data;
		_item = item;
		_tabIdx = IDX;

		if (IDX == 1)
			TrashItemImage.sprite = Managers.Resource.Load<Sprite>(selectedItemData.GetIconPath);

		ShowTab(_tabIdx);

	}

	public void ShowTab(int idx)
	{

		UseTab.gameObject.SetActive(false);
		TrashTab.gameObject.SetActive(false);

		switch (idx)
        {
			case 0:
				UseTab.gameObject.SetActive(true);
				break;

			case 1:
				TrashTab.gameObject.SetActive(true);
				break;
        }
    }
}
