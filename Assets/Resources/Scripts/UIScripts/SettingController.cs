using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] private UI _ui;

    public MapController chunkController;
    public Board boardController;
    public Game gameController;
    public Button[] cellSizeButtons;
    public GameObject mainMenu;
    public GameObject settingMenu;
    
    private int newCellSizeX= 0;
    private int newCellSizeY = 0;
    private int newBoardSizeX = 0;
    private int newBoardSizeY = 0;
    private int newDangerSize = 0;

    // Start is called before the first frame update

    public void SetMouseSensitivity(float val)
    {
        PlayerStats.currentMouseSensitivity = PlayerStats.minMouseSensitivity + ((PlayerStats.maxMouseSensitivity - PlayerStats.minMouseSensitivity) * val);
    }

    public void SetCellSize(int size)
    {
        newCellSizeX = size;
        newCellSizeY = size;
    }

    public void SetBoardWidth(string valStr)
    {
        int val = 0;
        if (valStr != "")
        {
            val = int.Parse(valStr);
        }
        if (val > 0)
        {
            newBoardSizeX = val;
        }
    }
    public void SetBoardHeight(string valStr)
    {
        int val = 0;
        if (valStr != "")
        {
            val = int.Parse(valStr);
        }
        if (val > 0)
        {
            newBoardSizeY = val;
        }
    }
    public void SetDangerNumber(string valStr)
    {
        int val = 0;
        if (valStr != "")
        {
            val = int.Parse(valStr);
        }
        if (val > 0)
        {
            newDangerSize = val;
        }
    }
    public void Done()
    {
        boardController.DestroyBoxes();
        boardController.Clear();
        chunkController.DestroyCells();
        if (newDangerSize != 0)
        {
            boardController.SetDangerAmount(newDangerSize);
        }
        if (newBoardSizeX != 0)
        {
            boardController.SetWidth(newBoardSizeX);
        }
        if (newBoardSizeY != 0)
        {
            boardController.SetHeight(newBoardSizeY);
        }
        if (newCellSizeX != 0 && newCellSizeY != 0)
        {
            chunkController.SetCellSizes(newCellSizeX, newCellSizeY);
        }
        boardController.InitializeEverything();
        gameController.SetupBoard();
        _ui.HideSettings();
        _ui.ShowBoard();
        _ui.ShowMenu();
    }

}
