﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    DateTime time;

    [SerializeField] RectTransform _globalPanel;
    UIPosition _uiPosition;
    GameManager _gameMangager;

    [SerializeField] CanvasGroup _globalCanvas;
    [SerializeField] CanvasGroup _tutoCanvas;
    // Sounds
    [Header("Sounds")]
    public GameObject menuSoundManager;
    public Slider sliderMusic;
    public Slider sliderSFX;

    [SerializeField] CanvasGroup main;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float speed;

    [SerializeField] bool _isMain = true;
    float progress = 1;

    [SerializeField] bool _isGlobal = true;
    float globalProgress = 1;

    public UIPosition UiPosition { get => _uiPosition; set => _uiPosition = value; }
    public RectTransform GlobalPanel { get => _globalPanel; set => _globalPanel = value; }
    private void Awake()
    {
        _uiPosition = GetComponent<UIPosition>();
        _gameMangager = FindObjectOfType<GameManager>();
        progress = (_isMain) ? 1 : 0;
        time = DateTime.Now;

    }

    private void Update()
    {
       
        if(_isMain && progress < 1)
        {
            progress += (float)(DateTime.Now - time).TotalSeconds * speed;
            if (progress >= 1) progress = 1;
            main.alpha = curve.Evaluate(progress);
        }
        if (!_isMain && progress > 0)
        {
            progress -= (float)(DateTime.Now - time).TotalSeconds * speed;
            if (progress <= 0) progress = 0;
            main.alpha = curve.Evaluate(progress);
        }
        if (_isGlobal && globalProgress < 1)
        {
            globalProgress += (float)(DateTime.Now - time).TotalSeconds * speed;
            if (globalProgress >= 1) globalProgress = 1;
            _globalCanvas.alpha = curve.Evaluate(globalProgress);
            _tutoCanvas.alpha = 1 - _globalCanvas.alpha;
        }
        if (!_isGlobal && globalProgress > 0)
        {
            globalProgress -= (float)(DateTime.Now - time).TotalSeconds * speed;
            if (globalProgress <= 0) globalProgress = 0;
            _globalCanvas.alpha = curve.Evaluate(globalProgress);
            _tutoCanvas.alpha = 1 - _globalCanvas.alpha;
        }

        time = DateTime.Now;
    }
    

    public void Tutoriel(bool active = false)
    {
        _isGlobal = active;
        _globalCanvas.interactable = _isGlobal;
        _globalCanvas.blocksRaycasts = _isGlobal;
        _tutoCanvas.interactable = !_isGlobal;
        _tutoCanvas.blocksRaycasts = !_isGlobal;

        AkSoundEngine.PostEvent("Menu_Select", menuSoundManager);
    }
    public void StartButton(bool isMain)
    {
        _isMain = isMain;
        main.interactable = _isMain;
        main.blocksRaycasts = _isMain;
        ActivateInputManager();

        AkSoundEngine.PostEvent("Menu_Play", menuSoundManager);

    }
    public void ResumeButton(bool isMain)
    {
        if (!isMain) _gameMangager.Pause();
        Resume(isMain);
    }
    public void Resume(bool isMain)
    {
        _isMain = isMain;
        main.interactable = _isMain;
        main.blocksRaycasts = _isMain;
        _uiPosition.Next(0);
    }
    public void OptionButton()
    {
        _uiPosition.Next(1);
        AkSoundEngine.PostEvent("Menu_Select", menuSoundManager);
    }
    public void MainButton()
    {
        _uiPosition.Next(0);
        AkSoundEngine.PostEvent("Menu_Back", menuSoundManager);
    }
    public void CreditButton()
    {
        _uiPosition.Next(-1);
        AkSoundEngine.PostEvent("Menu_Select", menuSoundManager);
    }
    public void ExitButton()
    {
        Debug.Log("Exit");
        AkSoundEngine.PostEvent("Menu_Quit", menuSoundManager);
        Application.Quit();
    }
    
    public void ActivateInputManager()
    {
        _gameMangager.GetComponent<PlayerInputManager>().enabled = true;
    }

    public void OnValueChangeMusic()
    {
        AkSoundEngine.SetRTPCValue("RTPC_MusicVolume", sliderMusic.value*100f);
    }

    public void OnValueChangeSFX()
    {
        AkSoundEngine.SetRTPCValue("RTPC_SFXVolume", sliderSFX.value * 100f);
    }

}
