using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    PathPoint[] pathPointToMoveOn_;
    public PathObjectsParent pathObjParent;
    public List<PlayerPiece> playerPiecesList = new List<PlayerPiece>();

    private void Start()
    {
        pathObjParent = GetComponentInParent<PathObjectsParent>();
    }


    public bool AddPlayerPiece(PlayerPiece playerPiece_)
    {
        if(this.name== "CentreHomePoint")
        {
            Completed(playerPiece_);
        }
        if (this.name!= "PathPoint" && this.name != "PathPoint (8)" && this.name != "PathPoint (13)" && this.name != "PathPoint (21)" && this.name != "PathPoint (26)" && this.name != "PathPoint (34)" && this.name != "PathPoint (39)" && this.name != "PathPoint (47)" && this.name != "CentreHomePoint")
        {
            if (playerPiecesList.Count == 1)
            {
                string prevPlayerPieceName = playerPiecesList[0].name;
                string currentPlayerPieceName = playerPiece_.name;
                currentPlayerPieceName = currentPlayerPieceName.Substring(0, currentPlayerPieceName.Length - 4);
                if (!prevPlayerPieceName.Contains(currentPlayerPieceName))
                {
                    playerPiecesList[0].isReady = false;

                    StartCoroutine(revertOnStart(playerPiecesList[0]));


                    playerPiecesList[0].numberOfStepsAlreadyMoved = 0;
                    RemovePlayerPiece(playerPiecesList[0]);
                    playerPiecesList.Add(playerPiece_);

                    return false;
                }
            }
        }
        addPlayer(playerPiece_);
        return true;
    }

    IEnumerator revertOnStart(PlayerPiece playerPiece_)
    {
        if (playerPiece_.name.Contains("Red"))
        {
            GameManager.gm.redOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjParent.redPathPoints;
        }
        else if (playerPiece_.name.Contains("Blue"))
        {
            GameManager.gm.blueOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjParent.bluePathPoints;
        }
        else if (playerPiece_.name.Contains("Yellow"))
        {
            GameManager.gm.yellowOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjParent.yellowPathPoints;
        }
        else if (playerPiece_.name.Contains("Green"))
        {
            GameManager.gm.greenOutPlayers -= 1;
            pathPointToMoveOn_ = pathObjParent.greenPathPoints;
        }

        for (int i = playerPiece_.numberOfStepsAlreadyMoved; i >= 0; i--)
        {
            playerPiece_.transform.position = pathPointToMoveOn_[i].transform.position;
            yield return new WaitForSeconds(0.05f);
        }

        playerPiece_.transform.position = pathObjParent.BasePoints[BasePointPosition(playerPiece_.name)].transform.position;
    
    }

    int BasePointPosition(string name)
    {
        
        for (int i = 0; i < pathObjParent.BasePoints.Length; i++)
        {
            if (pathObjParent.BasePoints[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }

    void addPlayer(PlayerPiece playerPiece_)
    {
        playerPiecesList.Add(playerPiece_);
        RescaleAndRepositionAllPlayerPieces();
    }
    public void RemovePlayerPiece(PlayerPiece PlayerPiece_)
    {
        if (playerPiecesList.Contains(PlayerPiece_))
        {
            playerPiecesList.Remove(PlayerPiece_);
            RescaleAndRepositionAllPlayerPieces();
        }
    }
    public void Completed(PlayerPiece playerPiece_)
    {
        if (playerPiece_.name.Contains("Red"))
        {
            GameManager.gm.redCompletePlayer += 1;
            GameManager.gm.redOutPlayers -= 1;
            if (GameManager.gm.redCompletePlayer == 4)
            {
                ShowCelebration();
            }

        }
        else if (playerPiece_.name.Contains("Blue"))
        {
            GameManager.gm.blueCompletePlayer += 1;
            GameManager.gm.blueOutPlayers -= 1;
            if (GameManager.gm.blueCompletePlayer == 4)
            {
                ShowCelebration();
            }
        }
        else if (playerPiece_.name.Contains("Yellow"))
        {
            GameManager.gm.yellowCompletePlayer += 1;
            GameManager.gm.yellowOutPlayers -= 1;
            if (GameManager.gm.yellowCompletePlayer == 4)
            {
                ShowCelebration();
            }
        }
        else if (playerPiece_.name.Contains("Green"))
        {
            GameManager.gm.greenCompletePlayer += 1;
            GameManager.gm.greenOutPlayers -= 1;
            if (GameManager.gm.greenCompletePlayer == 4)
            {
                ShowCelebration();
            }
        }
    }
    void ShowCelebration()
    {

    }
   public void RescaleAndRepositionAllPlayerPieces()
    {
        int plsCount = playerPiecesList.Count;
        bool isOdd;
        if(plsCount%2==0)
        {
            isOdd = false;
        }
        else
        {
            isOdd = true;
        }
        int spriteLayers = 0;


        int extent = plsCount / 2;
        int counter = 0;
        if(isOdd)
        {
            for (int i = -extent; i <= extent; i++)
            {
                playerPiecesList[counter].transform.localScale = new Vector3(pathObjParent.scales[plsCount - 1], pathObjParent.scales[plsCount - 1], 1f);
                playerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjParent.positionsDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;

            }
        }
        else
        {
            for (int i = -extent; i < extent; i++)
            {
                playerPiecesList[counter].transform.localScale = new Vector3(pathObjParent.scales[plsCount - 1], pathObjParent.scales[plsCount - 1], 1f);
                playerPiecesList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjParent.positionsDifference[plsCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        for (int i = 0; i < playerPiecesList.Count; i++)
        {
            playerPiecesList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayers;
            spriteLayers++;

        }

    }
}
