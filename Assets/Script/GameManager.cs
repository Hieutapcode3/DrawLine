using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Sprite _musicImg;
    [SerializeField] private Sprite _muteImg;
    [SerializeField] private List<Text> _levelIndex;
    private bool isMuted;
    [SerializeField] private List<Button> _musicButtons;
    public static GameManager Instance;
    
    
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        Time.timeScale = 1;
        if(_levelIndex.Count !=0){
            foreach(Text txt in _levelIndex)
                txt.text = "Level " + (SceneManager.GetActiveScene().buildIndex - 1).ToString();
        }

        isMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;
        if(_musicButtons.Count != 0){
            foreach (Button btn in _musicButtons)
            {
                btn.image.sprite = isMuted ? _muteImg : _musicImg;
                btn.onClick.AddListener(OnMusicButtonClick);
            }
        }
    }
    public void OnMusicButtonClick()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        // AudioManager.Instance.audioSource.mute = isMuted;
        foreach (Button btn in _musicButtons)
        {
            btn.image.sprite = isMuted ? _muteImg : _musicImg;
        }
    }

    public void PlayGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    public void ReloadScene(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToStartGame()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
