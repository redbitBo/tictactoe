using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{

    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;

    public Player playerX;  // X for human
    public Player playerO;  // O for AI

    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;

    public string currentAISide;
    private string playerSide;  // current side
    private string startingSide;  // starting side
    private int moveCount;
    private int winningIndex;

    void Awake()
    {
        // initialize the board
        SetGameControllerReferenceOnButtons();
        gameOverPanel.SetActive(false);
        moveCount = 0;
        restartButton.SetActive(false);
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetPlayerSide(string clickSide)
    {
        playerSide = clickSide;
        startingSide = clickSide;
        if (playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
            StartGame();
        }
        else
        {
            SetPlayerColors(playerO, playerX);
            StartGame();
            ChangeSides(); // the magic code that makes X goes first
        }

        
    }
    void ChangeSides()
    {
        if (startingSide == "X")
        {
            playerSide = (playerSide == "X") ? "O" : "X";
            if (playerSide == "X")
            {
                SetPlayerColors(playerX, playerO);
            }
            else
            {
                SetPlayerColors(playerO, playerX);
                aiMove();
            }
        }
        else
        {
            playerSide = (playerSide == "X") ? "O" : "X";
            if (playerSide == "X")
            {
                SetPlayerColors(playerX, playerO);
                aiMove();
            }
            else
            {
                SetPlayerColors(playerO, playerX);
            }
        }
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);

        if (startingSide == "X")
            currentAISide = "O";
        else
            currentAISide = "X";
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;
        // winning if
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        else if (moveCount >= 9)
        {
            GameOver("draw");
        }
        else
        {
            ChangeSides();
        }
    }



    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);
        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a Draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            SetGameOverText("Player " + winningPlayer + " Wins!");
        }
        restartButton.SetActive(true);
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
    
    void aiMove()
    {
        // a random AI, just want to know if calling proper function
        //int randomNum;
        //do
        //{
        //    randomNum = Random.Range(0, 8);
        //} while (buttonList[randomNum].text!="");
        //// below lines show how ai acts
        //buttonList[randomNum].text = playerSide;
        //buttonList[randomNum].GetComponentInParent<GridSpace>().button.onClick.Invoke();
        
        Text[] cur = buttonList;
        Vector2 move = Maximize(cur, 8);
        Debug.Log(move);
        //Debug.Log("getmoves" + getMoves());
        // below lines show how ai acts
        buttonList[(int)move.x].text = playerSide;
        buttonList[(int)move.x].GetComponentInParent<GridSpace>().button.onClick.Invoke();
    }

    // scoring by checking the game board (buttonList)
    int scoreMove(Text[] List)
    {
        // AI is gonna win
        if (List[0].text == playerO.text.ToString() && List[1].text == playerO.text.ToString() && List[2].text == playerO.text.ToString() ||
            List[3].text == playerO.text.ToString() && List[4].text == playerO.text.ToString() && List[5].text == playerO.text.ToString() ||
            List[6].text == playerO.text.ToString() && List[7].text == playerO.text.ToString() && List[8].text == playerO.text.ToString() ||
            List[0].text == playerO.text.ToString() && List[3].text == playerO.text.ToString() && List[6].text == playerO.text.ToString() ||
            List[1].text == playerO.text.ToString() && List[4].text == playerO.text.ToString() && List[7].text == playerO.text.ToString() ||
            List[2].text == playerO.text.ToString() && List[5].text == playerO.text.ToString() && List[8].text == playerO.text.ToString() ||
            List[0].text == playerO.text.ToString() && List[4].text == playerO.text.ToString() && List[8].text == playerO.text.ToString() ||
            List[2].text == playerO.text.ToString() && List[4].text == playerO.text.ToString() && List[6].text == playerO.text.ToString())
        {
            return 1;
        }
        // human is gonna win
        else if (List[0].text == playerX.text.ToString() && List[1].text == playerX.text.ToString() && List[2].text == playerX.text.ToString() ||
            List[3].text == playerX.text.ToString() && List[4].text == playerX.text.ToString() && List[5].text == playerX.text.ToString() ||
            List[6].text == playerX.text.ToString() && List[7].text == playerX.text.ToString() && List[8].text == playerX.text.ToString() ||
            List[0].text == playerX.text.ToString() && List[3].text == playerX.text.ToString() && List[6].text == playerX.text.ToString() ||
            List[1].text == playerX.text.ToString() && List[4].text == playerX.text.ToString() && List[7].text == playerX.text.ToString() ||
            List[2].text == playerX.text.ToString() && List[5].text == playerX.text.ToString() && List[8].text == playerX.text.ToString() ||
            List[0].text == playerX.text.ToString() && List[4].text == playerX.text.ToString() && List[8].text == playerX.text.ToString() ||
            List[2].text == playerX.text.ToString() && List[4].text == playerX.text.ToString() && List[6].text == playerX.text.ToString())
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    Vector2 Maximize(Text[] buttonList, int depth)
    {
        int score = scoreMove(buttonList);
        if (depth == 0 || moveCount >=9) return new Vector2(-1,score);
        //vec2 to store index and score
        Vector2 max = Vector2.zero;
        max.Set(-1,-99999);
        //Debug.Log("getmoves" + getMoves());
        for (int i = 0; i < 9; i++)
        {
            Text[] cur = buttonList;
            // if AI plays here
            if (aiAct(i, playerO)) 
            {
                Vector2 nextMove = Minimize(cur, depth - 1);
                
                //check if this move finish the game
                if(max.x == -1 || nextMove.y >max.y)
                {
                    max.x = i;
                    max.y = nextMove.y;
                    //Debug.Log("max" + max);
                }
            }

        }
        return max;
    }

    Vector2 Minimize(Text[] buttonList, int depth)
    {
        //int score = scoreMove(playerSide);
        if (depth == 0 || moveCount >= 9) return new Vector2(-1, 0);
        //vec2 to store index and score
        Vector2 min = Vector2.zero;
        min.Set(-1, 99999);
        for (int i = 0; i < 8; i++)
        {
            Text[] cur = buttonList;
            if (aiAct(i,playerX))
            {
                Vector2 nextMove = Maximize(cur, depth - 1);

                if(min.x==0 || nextMove.y < min.y)
                {
                    min.x = i;
                    min.y = nextMove.y;
                }
            }

        }
        return min;
    }

    // return array of int index of available moves
    List<int> getMoves()
    {
        List<int> res = null;
        for(int i = 0;i<9;i++)
        {
            if (buttonList[i].text == "")
            {
                res.Add(i);
            }
        }
        return res;
    }

    bool aiAct(int index, Player player)
    {
        if (buttonList[index].text == "")
        {

            moveCount += 1;
            return true;
        } 
        return false;
    }
}