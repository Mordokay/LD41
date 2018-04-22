using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    GameObject player;
    GameObject gm;
    bool muted;
    public Sprite mutedSprite;
    public Sprite unmutedSprite;
    float lastVolume;

    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject playerWinPanel;
    public Text playerWinPanelMessage;

    public Slider volumeSlider;
    public GameObject muteButtonOnMenu;
    public GameObject muteButtonOnOptions;
    public GameObject continueButton;

    bool gameStarted;

    public Color winColor;
    public Color loseColor;

    public LayerMask groundLayer;
    public InputField nickname;

    void Start () {
        gameStarted = false;

        player = GameObject.FindGameObjectWithTag("Player");
        gm = GameObject.FindGameObjectWithTag("GameManager");
        if (!PlayerPrefs.HasKey("loading") || PlayerPrefs.GetInt("loading") == 0)
        {
            PlayerPrefs.SetInt("loading", 0);
            Time.timeScale = 0.0f;
        }
        else if(PlayerPrefs.GetInt("loading") == 1)
        {
            PlayerPrefs.SetInt("loading", 0);
            Time.timeScale = 1.0f;
            gameStarted = true;
            mainMenuPanel.SetActive(false);
        }

        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            muted = false;
        }
        else if (PlayerPrefs.GetInt("muted") == 1)
        {
            AudioListener.volume = 0;
        }

        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1.0f);
        }
        else
        {
            lastVolume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = lastVolume;
        }
        if(PlayerPrefs.HasKey("nickname") && PlayerPrefs.GetString("nickname") != "")
        {
            nickname.text = PlayerPrefs.GetString("nickname");
        }
    }
	
    public void ShowEnemyWin(string name)
    {
        playerWinPanel.SetActive(true);
        playerWinPanel.GetComponent<Image>().color = loseColor;
        playerWinPanelMessage.text = name + System.Environment.NewLine + "Wins the Game!!!";
    }

    public void UpdateNickname()
    {
        PlayerPrefs.SetString("nickname", nickname.text);
    }

    public void ShowPlayerWin()
    {
        playerWinPanel.SetActive(true);
        playerWinPanel.GetComponent<Image>().color = winColor;
        playerWinPanelMessage.text = PlayerPrefs.GetString("nickname") + System.Environment.NewLine + "Wins the Game!!!";
    }

    public void NewGame()
    {
        if (PlayerPrefs.GetString("nickname") != "" && PlayerPrefs.GetString("nickname") != "")
        {
            PlayerPrefs.SetInt("loading", 1);
            SceneManager.LoadScene(0);
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1.0f;
        mainMenuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToOptionsPanel()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        playerWinPanel.SetActive(false);
    }

    public void ReturnToMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        playerWinPanel.SetActive(false);
    }

    public void ReturnToMenuFromWin()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        playerWinPanel.SetActive(false);
        gameStarted = false;
    }

    private void Update()
    {
        if (!gameStarted)
        {
            continueButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            continueButton.GetComponent<Button>().interactable = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenuPanel.activeSelf)
            {
                mainMenuPanel.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                mainMenuPanel.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
    }

    public void ToggleMute()
    {
        if (muted)
        {
            PlayerPrefs.SetInt("muted", 0);
            AudioListener.volume = lastVolume;
            volumeSlider.value = lastVolume;
            muted = false;

            muteButtonOnMenu.GetComponent<Image>().sprite = unmutedSprite;
            muteButtonOnOptions.GetComponent<Image>().sprite = unmutedSprite;
        }
        else
        {
            lastVolume = volumeSlider.value;
            volumeSlider.value = 0.0f;
            PlayerPrefs.SetInt("muted", 1);
            AudioListener.volume = 0.0f;
            muted = true;

            muteButtonOnMenu.GetComponent<Image>().sprite = mutedSprite;
            muteButtonOnOptions.GetComponent<Image>().sprite = mutedSprite;
        }
    }

    public void UpdateVolume()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        if(volumeSlider.value == 0)
        {
            muteButtonOnOptions.GetComponent<Image>().sprite = mutedSprite;
        }
        else
        {
            muteButtonOnOptions.GetComponent<Image>().sprite = unmutedSprite;
        }
        AudioListener.volume = volumeSlider.value;
    }

    public void EndTurn()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.GetComponent<PlayerMovementController>().playerRemainingMoves = 0;
        if (player.GetComponent<PlayerMovementController>().jumping)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f, groundLayer))
            {
                player.GetComponent<PlayerMovementController>().droppingDown = true;
                player.GetComponent<PlayerMovementController>().MoveDown(1);
                gm.GetComponent<GameData>().playerActions.Add(new PlayerMovementController.Action("d", 1));
                gm.GetComponent<GameData>().PlayStage();
            }
        }
        else
        {
            gm.GetComponent<GameData>().NextParticipant();
        }
    }
}
