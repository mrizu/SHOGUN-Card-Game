using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuGameObject;
    [SerializeField] private GameObject _creditsGameObject;
    [SerializeField] private GameObject _settingsGameObject;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _settingsBackButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _creditsBackButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _introButton;
    [SerializeField] private AudioMixer _audioMixer;

    public void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            LevelLoaderManager.LevelLoaderInstance.LoadNextScene();
        });

        _creditsButton.onClick.AddListener(() =>
        {
            _creditsGameObject.SetActive(true);
            _settingsGameObject.SetActive(false);
            _mainMenuGameObject.SetActive(false);
            //MenuManager.SetActiveView(MenuManager.CreditsMenu);
        });

        _creditsBackButton.onClick.AddListener(() =>
        {
            _creditsGameObject.SetActive(false);
            _settingsGameObject.SetActive(false);
            _mainMenuGameObject.SetActive(true);
            //MenuManager.SetActiveView(MenuManager.CreditsMenu);
        });

        _settingsButton.onClick.AddListener(() =>
        {
            _creditsGameObject.SetActive(false);
            _settingsGameObject.SetActive(true);
            _mainMenuGameObject.SetActive(false);
            //MenuManager.SetActiveView(MenuManager.SettingsMenu);
        });

        _settingsBackButton.onClick.AddListener(() =>
        {
            _creditsGameObject.SetActive(false);
            _settingsGameObject.SetActive(false);
            _mainMenuGameObject.SetActive(true);
            //MenuManager.SetActiveView(MenuManager.SettingsMenu);
        });

        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        _introButton.onClick.AddListener(() =>
        {
            LevelLoaderManager.LevelLoaderInstance.LoadIntro();
        });
    }
    private void Start() {
        LoadAudioSettings();
    }
    private void LoadAudioSettings(){
        float music=PlayerPrefs.GetFloat("musicVolume");
        float sound=PlayerPrefs.GetFloat("soundsVolume");
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(music)*20);
        _audioMixer.SetFloat("SoundVolume", Mathf.Log10(sound)*20);
    }
}