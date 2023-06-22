using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DialoguePopup : UI_Popup
{

    [SerializeField] DialogueSystem Dialogue;
    [SerializeField] DialogueDB DB;

    public void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

    }



}