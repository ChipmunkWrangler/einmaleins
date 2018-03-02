using CrazyChipmunk;
using UnityEngine;
using UnityEngine.UI;

class NewPlayerName : MonoBehaviour
{
    [SerializeField] GameEvent listFullEvent = null;
    [SerializeField] TextButton[] playerButtons = null;
    [SerializeField] Button playButton = null;
    [SerializeField] Image playButtonImage = null;
    [SerializeField] InputField inputField = null;
    [SerializeField] float buttonFadeAlpha = 0.5F;
    [SerializeField] float buttonFadeDuration = 0.1F;
    [SerializeField] InitialGameSceneLoader sceneLoader = null;
    [SerializeField] StringVariable playerName = null;

    string newName;
    bool buttonsAlreadyPressed;
    PlayerNameController playerNameController;

    public void OnPlayerNameChanged(string name)
    {
        if (!buttonsAlreadyPressed)
        {
            ActivatePlayButton(playerNameController.IsNameValid(name));
            newName = name;
        }
    }

    public void OnPlayerNameButton(int i)
    {
        if (!buttonsAlreadyPressed)
        {
            playerName.Value = playerNameController.Names[i];
            Play();
        }
    }

    public void OnPlay()
    {
        if (!buttonsAlreadyPressed)
        {
            playerNameController.AppendName(newName);
            playerName.Value = newName;
            Play();
        }
    }

    void Start()
    {
        playerNameController = new PlayerNameController();
        playerNameController.Load();
        if (playerName != "")
        {
            RocketParts.Reset();
            TargetPlanet.Reset();
        } // else this is initial start
        ActivatePlayButton(false);
        int numPlayers = playerNameController.Names.Length;
        if (numPlayers >= playerButtons.Length)
        {
            listFullEvent.Raise();
        }
        if (numPlayers == 0)
        {
            inputField.Select();
        }
        for (int i = 0; i < numPlayers; ++i)
        {
            playerButtons[i].SetText(playerNameController.Names[i]);
            playerButtons[i].SetActive(true);
        }
        for (int i = numPlayers; i < playerButtons.Length; ++i)
        {
            playerButtons[i].SetActive(false);
        }
    }

    void ActivatePlayButton(bool b)
    {
        playButton.interactable = b;
        playButtonImage.CrossFadeAlpha(b ? 1.0F : buttonFadeAlpha, buttonFadeDuration, false);
    }

    void Play()
    {
        // todo transition
        DisableButtons();
        playerNameController.Save();
        PlayerPrefs.Save();
        sceneLoader.LoadInitialGameScene();
    }


    void DisableButtons()
    {
        buttonsAlreadyPressed = true;
        playButton.enabled = false;
        foreach (TextButton button in playerButtons)
        {
            button.enabled = false;
        }
    }
}