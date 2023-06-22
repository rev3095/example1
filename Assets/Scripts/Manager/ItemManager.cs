using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Define;
public class ItemManager
{
    public static UI_InvenPopup ui_InvenPopup;
    public static UI_Inven_Item selectedItem;

    //Inven_Item으로 출력되는 InvenPopup의 Title,DetailText가 Tab이 바뀌었을 경우 체크를 위한 bool
    //InvenPopup의 ShowTab에서 bool 조작
    public static bool OnChangedInvenTab = false;
    public void OnClickItemButton(IItemData _iData)
    {
        ui_InvenPopup.SetItemText(_iData);

    }

    public void Refresh()
    {
        ui_InvenPopup.SetCurValueText();
    }


}