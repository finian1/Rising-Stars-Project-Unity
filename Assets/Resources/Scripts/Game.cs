using UnityEngine;

public class Game : MonoBehaviour
{
    public MapController chunkController;
    public GameObject playerObject;
    private Board _board;
    private UI _ui;
    private double _gameStartTime;
    private bool _gameInProgress;
    public GameObject mainMenu;
    public GameObject settingMenu;
    public ShopController shop;
    
    public bool IsGameInProgress()
    {
        return _gameInProgress;
    }
    public void OnClickedNewGame()
    {
        CleanupScript.Cleanup();
        if (_board != null)
        {
            //If board is initialized

            _board.RechargeBoxes();
            //chunkController.PopulateArrays(_board.GetWidth(), _board.GetHeight());
            //playerObject.SetActive(true);
        }

        if (_ui != null)
        {
            _ui.HideMenu();
            _ui.ShowBoard();
            _ui.ShowGame();
        }
    }

    public void OnClickedExit()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    public void OnClickedReset()
    {
        if (_board != null)
        {
            _board.Clear();
            CleanupScript.Cleanup();
            chunkController.DestroyCells();
            chunkController.DestroyBoarders();
            chunkController.SetPlayerMarkerActive(false);
            playerObject.SetActive(false);
        }

        if (_ui != null)
        {
            _ui.HideResult();
            _ui.ShowBoard();
            _ui.ShowMenu();
        }
    }

    public void OnClickedShop()
    {
        _ui.HideBoard();
        _ui.HideMenu();
        _ui.ShowShop();
        shop.SwitchedTo();
    }

    public void OnClickedSettings()
    {
        _ui.HideMenu();
        _ui.ShowSettings();
    }

    private void Awake()
    {
        _board = transform.parent.GetComponentInChildren<Board>();
        _ui = transform.parent.GetComponentInChildren<UI>();
        _gameInProgress = false;

        PlayerStats.primaryWeapon = PlayerStats.weaponsOwned[0];
        PlayerStats.secondaryWeapon = PlayerStats.weaponsOwned[0];
    }

    public void SetupBoard()
    {
        _board.Setup(BoardEvent);
    }

    private void Start()
    {
        if (_board != null)
        {
            chunkController.SetPlayerMarkerActive(false);
            SetupBoard();
        }

        if (_ui != null)
        {
            _ui.ShowMenu();
        }
    }

    private void Update()
    {
        if(_ui != null)
        {
            _ui.UpdateTimer(_gameInProgress ? Time.realtimeSinceStartupAsDouble - _gameStartTime : 0.0);
        }
    }

    private void BoardEvent(Board.Event eventType)
    {
        if(eventType == Board.Event.ClickedDanger && _ui != null)
        {
            //EndGame(false);
        }

        if (eventType == Board.Event.Win && _ui != null)
        {
            EndGame(true);
        }

        if (!_gameInProgress)
        {
            _gameInProgress = true;
            _gameStartTime = Time.realtimeSinceStartupAsDouble;
        }
    }

    public void EndGame(bool winGame)
    {
        _ui.HideGame();
        CleanupScript.Cleanup();
        Reset();
        chunkController.SetPlayerMarkerActive(false);
        _ui.ShowResult(success: winGame);
    }

    private void Reset()
    {
        Cursor.lockState = CursorLockMode.None;
        playerObject.transform.position = new Vector3(0, 0, 0);
        playerObject.SetActive(false);
        _board.gameStarted = false;
    }
}
