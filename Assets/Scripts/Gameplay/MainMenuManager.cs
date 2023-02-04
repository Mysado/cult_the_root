using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [InfoBox("Button Colors")]
    [SerializeField] private Color buttonsNormalColor;
    [SerializeField] private Color buttonsHighlightColor;
    [SerializeField] private Color buttonsPressedColor;
    
    [InfoBox("Button References")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button exitGameButton;

    private void Awake()
    {
        SetButtonColors();
        startGameButton.onClick.AddListener(StartGame);
        exitGameButton.onClick.AddListener(ExitGame);
    }

    private void SetButtonColors()
    {
        var colorBlock = new ColorBlock
        {
            normalColor = buttonsNormalColor,
            highlightedColor = buttonsHighlightColor,
            pressedColor = buttonsPressedColor
        };
        
        startGameButton.colors = colorBlock;
        exitGameButton.colors = colorBlock;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }
    
    private void ExitGame()
    {
        Application.Quit();
    }
}
