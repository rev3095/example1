using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//BaseController로 카메라 관련 Utils기능을 가지고 있어도 좋음.
public class LobbyCameraController : BaseController
{
    public Camera lobbyCamera;

    public float SetTransitionDuration = 0.6f;
    public float transitionDuration = 1.0f;

    public void SetCamera()
    {
        Vector3 startPosition = new Vector3(UI_Main.possiblePosition_10F[Managers.Game.nowMP].position.x, UI_Main.possiblePosition_10F[Managers.Game.nowMP].position.y, -10);
        lobbyCamera.transform.DOMove(startPosition, SetTransitionDuration);
    }
    public void MoveCamera()
    {
        Vector3 playerPosition =  new Vector3(UI_Main.possiblePosition_10F[Managers.Game.nowMP].position.x, UI_Main.possiblePosition_10F[Managers.Game.nowMP].position.y, -10);
        //lobbyCamera.transform.position = Vector3.Lerp(lobbyCamera.transform.position, objectTransform, Time.deltaTime);
        lobbyCamera.transform.DOMove(playerPosition, transitionDuration);
        
    }


}
