using CrazyChipmunk;
using UnityEngine;
using UnityEngine.UI;

internal class NewPlayerName : MonoBehaviour
{
    [SerializeField] private float buttonFadeAlpha = 0.5F;
    [SerializeField] private float buttonFadeDuration = 0.1F;
    private bool buttonsAlreadyPressed;
    [SerializeField] private InputField inputField;
    [SerializeField] private GameEvent listFullEvent;

    private string newName;
    [SerializeField] private Button playDivisionButton;
    [SerializeField] private Text playDivisionSymbol;
    [SerializeField] private TextButton[] playerButtons;
    [SerializeField] private VariableString playerName;
    [SerializeField] private PlayerNameController playerNameController;
    [SerializeField] private Button playMultiplicationButton;
    [SerializeField] private Text playMultiplicationSymbol;
    [SerializeField] private Prefs prefs;
    [SerializeField] private InitialGameSceneLoader sceneLoader;

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

    private void Start()
    {
        playerNameController.Load();
        if (playerName != "")
        {
            RocketParts.Reset();
            TargetPlanet.Reset();
        } // else this is initial start

        ActivatePlayButton(false);
        var numPlayers = playerNameController.Names.Length;
        if (numPlayers >= playerButtons.Length) listFullEvent.Raise();
        if (numPlayers == 0) inputField.Select();
        for (var i = 0; i < numPlayers; ++i)
        {
            playerButtons[i].SetText(playerNameController.Names[i]);
            playerButtons[i].SetActive(true);
        }

        for (var i = numPlayers; i < playerButtons.Length; ++i) playerButtons[i].SetActive(false);
    }

    private void ActivatePlayButton(bool b)
    {
        playMultiplicationButton.interactable = b;
        playDivisionButton.interactable = b;
        playMultiplicationSymbol.CrossFadeAlpha(b ? 1.0F : buttonFadeAlpha, buttonFadeDuration, false);
        playDivisionSymbol.CrossFadeAlpha(b ? 1.0F : buttonFadeAlpha, buttonFadeDuration, false);
    }

    private void Play()
    {
        // todo transition
        DisableButtons();
        playerNameController.Save();
        PlayerPrefs.Save();
        sceneLoader.LoadInitialGameScene();
    }


    private void DisableButtons()
    {
        buttonsAlreadyPressed = true;
        playMultiplicationButton.enabled = false;
        playDivisionButton.enabled = false;
        foreach (var button in playerButtons) button.enabled = false;
    }
}