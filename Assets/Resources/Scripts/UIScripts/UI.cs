using UnityEngine;
using TMPro;
using System.Collections;

public class UI : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;
    [SerializeField] private CanvasGroup Menu;
    [SerializeField] private Color menuColor;
    [SerializeField] private CanvasGroup Game;
    [SerializeField] private Color gameColor;
    [SerializeField] private CanvasGroup Result;
    [SerializeField] private Color resultColor;
    [SerializeField] private CanvasGroup Settings;
    [SerializeField] private Color settingColor;
    [SerializeField] private CanvasGroup Shop;
    [SerializeField] private Color shopColor;
    [SerializeField] private CanvasGroup Board;
    [SerializeField] private Color boardColor;
    [SerializeField] private CanvasGroup Buffs;
    [SerializeField] private Color buffsColor;
    [SerializeField] private CanvasGroup HUD;
    [SerializeField] private CanvasGroup Paused;

    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private TMP_Text ResultText;

    private static readonly string[] ResultTexts = { "Game Over!", "You Win!!" };
    private static readonly float AnimationTime = 0.5f;

    public void ShowMenu()
    {
        StartCoroutine(ShowCanvas(Menu, 1.0f));
        StartCoroutine(SetBackgroundColour(menuColor));
    }

    public void ShowGame()
    {
        StartCoroutine(ShowCanvas(Game, 1.0f));
        StartCoroutine(SetBackgroundColour(gameColor));
    }

    public void ShowResult(bool success)
    {
        if (ResultText != null)
        {
            ResultText.text = ResultTexts[success ? 1 : 0];
        }

        StartCoroutine(ShowCanvas(Result, 1.0f));
        StartCoroutine(SetBackgroundColour(resultColor));
    }

    public void ShowPaused()
    {
        StartCoroutine(ShowCanvas(Paused, 1.0f));
    }

    public void ShowSettings()
    {
        StartCoroutine (ShowCanvas(Settings, 1.0f));
        StartCoroutine(SetBackgroundColour(settingColor));
    }

    public void ShowShop()
    {
        StartCoroutine(ShowCanvas(Shop, 1.0f));
        StartCoroutine(SetBackgroundColour(shopColor));
    }

    public void ShowBoard()
    {
        StartCoroutine(ShowCanvas(Board, 1.0f));
        StartCoroutine(SetBackgroundColour(boardColor));
    }

    public void ShowHUD()
    {
        StartCoroutine(ShowCanvas(HUD, 1.0f));
        
    }

    public void ShowBuffs()
    {
        StartCoroutine(ShowCanvas(Buffs, 1.0f));
        StartCoroutine(SetBackgroundColour(buffsColor));
    }

    public void HideMenu()
    {
        StartCoroutine(ShowCanvas(Menu, 0.0f));
    }

    public void HidePaused()
    {
        StartCoroutine(ShowCanvas(Paused, 0.0f));
    }

    public void HideGame()
    {
        StartCoroutine(ShowCanvas(Game, 0.0f));
    }

    public void HideResult()
    {
        StartCoroutine(ShowCanvas(Result, 0.0f));
    }

    public void HideSettings()
    {
        StartCoroutine(ShowCanvas(Settings, 0.0f));
    }

    public void HideShop()
    {
        StartCoroutine(ShowCanvas(Shop, 0.0f));
    }

    public void HideBoard()
    {
        StartCoroutine(ShowCanvas(Board, 0.0f));
    }

    public void HideHUD()
    {
        StartCoroutine(ShowCanvas(HUD, 0.0f));
    }
    public void HideBuffs()
    {
        StartCoroutine (ShowCanvas(Buffs, 0.0f));
    }

    public void UpdateTimer(double gameTime)
    {
        if (TimerText != null)
        {
            TimerText.text = FormatTime(gameTime);
        }
    }

    private void Awake()
    {
        if (Menu != null)
        {
            Menu.alpha = 0.0f;
            Menu.interactable = false;
            Menu.blocksRaycasts = false;
        }

        if (Game != null)
        {
            Game.alpha = 0.0f;
            Game.interactable = false;
            Game.blocksRaycasts = false;
        }

        if (Result != null)
        {
            Result.alpha = 0.0f;
            Result.interactable = false;
            Result.blocksRaycasts = false;
        }

        if(Settings != null)
        {
            Settings.alpha = 0.0f;
            Settings.interactable = false;
            Settings.blocksRaycasts = false;
        }

        if(Shop != null)
        {
            Shop.alpha = 0.0f;
            Shop.interactable = false;
            Shop.blocksRaycasts = false;
        }

        if(Board != null)
        {
            Board.alpha = 0.0f;
            Board.interactable = false;
            Board.blocksRaycasts = false;
        }

        if(HUD != null)
        {
            HUD.alpha = 0.0f;
            HUD.interactable = false;
            HUD.blocksRaycasts = false;
        }

        if(Buffs != null)
        {
            Buffs.alpha = 0.0f;
            Buffs.interactable = false;
            Buffs.blocksRaycasts = false;
        }

        if(Paused != null)
        {
            Paused.alpha = 0.0f;
            Paused.interactable = false;
            Paused.blocksRaycasts = false;
        }
    }

    private static string FormatTime(double seconds)
    {
        float m = Mathf.Floor((int)seconds / 60);
        float s = (float)seconds - (m * 60);
        string mStr = m.ToString("00");
        string sStr = s.ToString("00.000");
        return string.Format("{0}:{1}", mStr, sStr);
    }

    private IEnumerator ShowCanvas(CanvasGroup group, float target)
    {
        if (group != null)
        {
            float startAlpha = group.alpha;
            float t = 0.0f;

            group.interactable = target >= 1.0f;
            group.blocksRaycasts = target >= 1.0f;

            while (t < AnimationTime)
            {
                t = Mathf.Clamp(t + Time.deltaTime, 0.0f, AnimationTime);
                group.alpha = Mathf.SmoothStep(startAlpha, target, t / AnimationTime);
                yield return null;
            }
        }
    }
    private IEnumerator SetBackgroundColour(Color newColour)
    {
        if (newColour != null)
        {
            Color currentColour = uiCamera.backgroundColor;
            float t = 0.0f;

            while (t < AnimationTime)
            {
                t = Mathf.Clamp(t + Time.deltaTime, 0.0f, AnimationTime);
                uiCamera.backgroundColor = Color.Lerp(currentColour, newColour, t / AnimationTime);
                yield return null;
            }
        }
    }
}
