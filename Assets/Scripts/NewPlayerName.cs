using CrazyChipmunk;
using UnityEngine;
using UnityEngine.UI;

class NewPlayerName : MonoBehaviour
{
    [SerializeField] GameEvent listFullEvent;
    [SerializeField] TextButton[] playerButtons;
    [SerializeField] Button playMultiplicationButton;
    [SerializeField] Button playDivisionButton;
    [SerializeField] Image playButtonImage;
    [SerializeField] InputField inputField;
    [SerializeField] float buttonFadeAlpha = 0.5F;
    [SerializeField] float buttonFadeDuration = 0.1F;
    [SerializeField] InitialGameSceneLoader sceneLoader;
    [SerializeField] VariableString playerName;
    [SerializeField] PlayerNameController playerNameController;
    [SerializeField] Prefs prefs;

    string newName;
    bool buttonsAlreadyPressed;

    public NewPlayerName()
    {
        playerButtons = null;
    }

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

    public void OnPlay(string questionType)
    {
        if (!buttonsAlreadyPressed)
        {
            playerNameController.AppendName(newName);
            playerName.Value = newName;
            QuestionGenerator.SetQuestionType(prefs, questionType);
            Play();
        }
    }

    void Start()
    {
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
        playMultiplicationButton.interactable = b;
        playDivisionButton.interactable = b;
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
        playMultiplicationButton.enabled = false;
        playDivisionButton.enabled = false;
        foreach (TextButton button in playerButtons)
        {
            button.enabled = false;
        }
    }
}