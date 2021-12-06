using UnityEngine;

public class Game : MonoBehaviour
{
    public ChunkController chunkController;
    public GameObject playerObject;
    private Board _board;
    private UI _ui;
    private double _gameStartTime;
    private bool _gameInProgress;
    public GameObject mainMenu;
    public GameObject settingMenu;
    

    public void OnClickedNewGame()
    {
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
            chunkController.DestroyCells();
            playerObject.SetActive(false);
        }

        if (_ui != null)
        {
            _ui.HideResult();
            _ui.ShowMenu();
        }
    }

    public void OnClickedSettings()
    {
        settingMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    private void Awake()
    {
        _board = transform.parent.GetComponentInChildren<Board>();
        _ui = transform.parent.GetComponentInChildren<UI>();
        _gameInProgress = false;
    }

    public void SetupBoard()
    {
        _board.Setup(BoardEvent);
    }

    private void Start()
    {
        if (_board != null)
        {
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
            _ui.HideGame();
            playerObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            _ui.ShowResult(success: false);
        }

        if (eventType == Board.Event.Win && _ui != null)
        {
            _ui.HideGame();
            _ui.ShowResult(success: true);
        }

        if (!_gameInProgress)
        {
            _gameInProgress = true;
            _gameStartTime = Time.realtimeSinceStartupAsDouble;
        }
    }
}
