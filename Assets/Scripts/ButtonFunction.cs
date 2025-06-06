using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunction : MonoBehaviour
{
    public MenuManager.MainMenuButtons buttonName;
    [SerializeField] static public int level;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public ColorBlock defaultCB;

    void Start()
    {
        defaultCB = easyButton.colors;
        level = 1;
        Color lightRed = new Color(1f, 0.5f, 0.5f);
        ColorBlock cb = defaultCB;
        cb.normalColor = lightRed;
        cb.highlightedColor = lightRed;
        cb.pressedColor = lightRed;
        cb.selectedColor = lightRed;
        easyButton.colors = cb;
        mediumButton.colors = defaultCB;
        hardButton.colors = defaultCB;
    }


    public void ClickStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ClickMenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ClickExitButton()
    {
        Application.Quit();
    }

    public void ClickEasyLevel()
    {
        level = 0;
        Color lightRed = new Color(1f, 0.5f, 0.5f);
        ColorBlock cb = easyButton.colors;
        cb.normalColor = lightRed;
        cb.highlightedColor = lightRed;
        cb.pressedColor = lightRed;
        cb.selectedColor = lightRed;
        easyButton.colors = cb;


        mediumButton.colors = defaultCB;
        hardButton.colors = defaultCB;
    }

    public void ClickMediumLevel()
    {
        level = 1;
        Color lightRed = new Color(1f, 0.5f, 0.5f);
        ColorBlock cb = mediumButton.colors;
        cb.normalColor = lightRed;
        cb.highlightedColor = lightRed;
        cb.pressedColor = lightRed;
        cb.selectedColor = lightRed;
        mediumButton.colors = cb;

        easyButton.colors = defaultCB;
        hardButton.colors = defaultCB;
    }

    public void ClickHardLevel()
    {
        level = 2;
        Color lightRed = new Color(1f, 0.5f, 0.5f);
        ColorBlock cb = hardButton.colors;
        cb.normalColor = lightRed;
        cb.highlightedColor = lightRed;
        cb.pressedColor = lightRed;
        cb.selectedColor = lightRed;
        hardButton.colors = cb;

        mediumButton.colors = defaultCB;
        easyButton.colors = defaultCB;
    }


}
