using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
	//IPointerClickHandler, IDragHandler 같은 인터페이스를 상속 받아 오버라이딩 하면, 해당 이벤트가 실행될 때 자동으로 이벤트 함수가 실행된다.
	//Action은 미리 C# 내에서 선언이 되어 있는 델리케이트로, 리턴 타입이 없는 함수만을 등록할 수 있는 제네릭 델리게이트이다.
	public Action<PointerEventData> OnClickHandler = null;
	public Action<PointerEventData> OnDragHandler = null;

	public void OnPointerClick(PointerEventData eventData) // 클릭 이벤트 오버라이딩 , 마우스 클릭 이벤트 발생 시 자동 실행
	{
		if (OnClickHandler != null)
			OnClickHandler.Invoke(eventData); // 클릭과 관련된 액션 실행
	}

	public void OnDrag(PointerEventData eventData) // 드래그 이벤트 오버라이딩 , 마우스 클릭 이벤트 발생 시 자동 실행
    {
		if (OnDragHandler != null)
			OnDragHandler.Invoke(eventData); // 드래그와 관련된 이벤트 실행
    }
}