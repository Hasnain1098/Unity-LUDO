using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingDice : MonoBehaviour
{
    [SerializeField] int numberGot;
    [SerializeField] GameObject rollingDiceAnim;
    [SerializeField] SpriteRenderer numberedSpriteHolder;
    [SerializeField] Sprite[] numberedSprites;
    
    Coroutine generateRandNumOnDice_Coroutine;
    public int outPieces;
    public PathObjectsParent pathParent;
    PlayerPiece[] currentPlayerPieces;
    PathPoint[] pathPointToMoveOn_;
    Coroutine moveSteps_Coroutine;
    PlayerPiece outPlayerPiece;

    public Dice DiceSound;

    private void Awake()
    {
        pathParent = FindObjectOfType<PathObjectsParent>();

    }
    public void OnMouseDown()
    {
        generateRandNumOnDice_Coroutine = StartCoroutine(GenerateRandomNumberOnDice_Enum());
    }
    public void mouseRoll()
    {
        generateRandNumOnDice_Coroutine = StartCoroutine(GenerateRandomNumberOnDice_Enum());
    }
    IEnumerator GenerateRandomNumberOnDice_Enum()
    {
        DiceSound.PlaySound();
        GameManager.gm.transferDice = false;
        yield return new WaitForEndOfFrame();
        if (GameManager.gm.canDiceRoll)
        {
            GameManager.gm.canDiceRoll = false;
            numberedSpriteHolder.gameObject.SetActive(false);
            rollingDiceAnim.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            numberGot = Random.Range(0, 6);
            numberedSpriteHolder.sprite = numberedSprites[numberGot];
            numberGot += 1;

            GameManager.gm.numOfStepsToMove = numberGot;
            GameManager.gm.rolledDice = this;

            numberedSpriteHolder.gameObject.SetActive(true);
            rollingDiceAnim.SetActive(false);
            yield return new WaitForEndOfFrame();


            int numGot = GameManager.gm.numOfStepsToMove;

            if (PlayerCannotMove())
            {
                yield return new WaitForSeconds(0.5f);
                if (numGot != 6)
                {
                    GameManager.gm.transferDice = true;
                }
                else
                {
                    GameManager.gm.selfDice = true;

                }
            }
            else
            {

                if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[0])
                {
                    outPieces = GameManager.gm.redOutPlayers;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[1])
                {
                    outPieces = GameManager.gm.blueOutPlayers;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[2])
                {
                    outPieces = GameManager.gm.yellowOutPlayers;
                }
                else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[3])
                {
                    outPieces = GameManager.gm.greenOutPlayers;
                }

                if (outPieces == 0 && numGot != 6)
                {
                    yield return new WaitForSeconds(0.5f);
                    GameManager.gm.transferDice = true;
                }
                else
                {
                    if (outPieces == 0 && numGot == 6)
                    {
                        MakePlayerReadyToMove(0);
                    }
                    else if (outPieces == 1 && numGot != 6 && GameManager.gm.canMove)
                    {
                        int playerPiecePosition = checkOutPlayer();
                        if (playerPiecePosition >= 0)
                        {
                            GameManager.gm.canMove = false;
                            moveSteps_Coroutine = StartCoroutine(MoveSteps_Enum(playerPiecePosition));
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                            if (numberGot != 6)
                            {
                                GameManager.gm.transferDice = true;
                            }
                            else
                            {
                                GameManager.gm.selfDice = true;

                            }
                        }
                    }
                    else if (GameManager.gm.totalPlayerCanPlay == 1 && GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[2])
                    {
                        if (numberGot == 6 && outPieces < 4)
                        {
                            MakePlayerReadyToMove(outPlayerToMove());
                        }
                        else
                        {
                            int playerPiecePosition = checkOutPlayer();
                            if (playerPiecePosition >= 0)
                            {
                                GameManager.gm.canMove = false;
                                moveSteps_Coroutine = StartCoroutine(MoveSteps_Enum(playerPiecePosition));

                            }
                            else
                            {
                                yield return new WaitForSeconds(0.5f);
                                if (numGot != 6)
                                {
                                    GameManager.gm.transferDice = true;
                                }
                                else
                                {
                                    GameManager.gm.selfDice = true;

                                }
                            }

                        }
                    }
                    else
                    {
                        if (checkOutPlayer() < 0)
                        {
                            yield return new WaitForSeconds(0.5f);
                            if (numGot != 6)
                            {
                                GameManager.gm.transferDice = true;
                            }
                            else
                            {
                                GameManager.gm.selfDice = true;

                            }
                        }
                    }
                }  
            }
           
            GameManager.gm.RollingDiceManager();
            if (generateRandNumOnDice_Coroutine != null)
            {
                StopCoroutine(GenerateRandomNumberOnDice_Enum());
            }
        }
    }
    int outPlayerToMove()
    {
        for (int i = 0; i < 4; i++) 
        {
            if(!GameManager.gm.yellowPlayerPiece[i].isReady)
            {
                return i;
            }
        }
        return 0;
    }

    int checkOutPlayer()
    {
        if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[0])
        {
            currentPlayerPieces = GameManager.gm.redPlayerPiece;
            pathPointToMoveOn_ = pathParent.redPathPoints;

        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[1])
        {
            currentPlayerPieces = GameManager.gm.bluePlayerPiece;
            pathPointToMoveOn_ = pathParent.bluePathPoints;

        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[2])
        {
            currentPlayerPieces = GameManager.gm.yellowPlayerPiece;
            pathPointToMoveOn_ = pathParent.yellowPathPoints;

        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[3])
        {
            currentPlayerPieces = GameManager.gm.greenPlayerPiece;
            pathPointToMoveOn_ = pathParent.greenPathPoints;

        }
        for (int i = 0; i < currentPlayerPieces.Length; i++)
        {
            if (currentPlayerPieces[i].isReady && isPathPointsAvailableToMove(GameManager.gm.numOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMoved, pathPointToMoveOn_))

            {
                return i;
            }
        }
        return -1;
    }

    public bool PlayerCannotMove()
    {
        if (outPieces > 0)
        {
            bool cannotMove = false;

            if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[0])
            {
                currentPlayerPieces = GameManager.gm.redPlayerPiece;
                pathPointToMoveOn_ = pathParent.redPathPoints;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[1])
            {
                currentPlayerPieces = GameManager.gm.bluePlayerPiece;
                pathPointToMoveOn_ = pathParent.bluePathPoints;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[2])
            {
                currentPlayerPieces = GameManager.gm.yellowPlayerPiece;
                pathPointToMoveOn_ = pathParent.yellowPathPoints;
            }
            else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[3])
            {
                currentPlayerPieces = GameManager.gm.greenPlayerPiece;
                pathPointToMoveOn_ = pathParent.greenPathPoints;
            }
            for (int i = 0; i < currentPlayerPieces.Length; i++)
            {
                if (currentPlayerPieces[i].isReady)
                {
                    if (isPathPointsAvailableToMove(GameManager.gm.numOfStepsToMove, currentPlayerPieces[i].numberOfStepsAlreadyMoved, pathPointToMoveOn_))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!cannotMove)
                    {
                        cannotMove = true;
                    }
                }
            }
                if (cannotMove)
                {
                    return true;
                }
        }
        return false;
    }
    bool isPathPointsAvailableToMove(int numOfStepsToMove_, int numOfStepsAlreadyMoved_, PathPoint[] pathPointsToMoveOn_)
    {
        int leftNumOfPathPoints = pathPointsToMoveOn_.Length - numOfStepsAlreadyMoved_;
        if (leftNumOfPathPoints >= numOfStepsToMove_)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void MakePlayerReadyToMove(int outPlayer)
    {

        if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[0])
        {
            outPlayerPiece= GameManager.gm.redPlayerPiece[outPlayer];
            pathPointToMoveOn_=pathParent.redPathPoints;
            GameManager.gm.redOutPlayers += 1;
        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[1])
        {
            outPlayerPiece = GameManager.gm.bluePlayerPiece[outPlayer];
            pathPointToMoveOn_ = pathParent.bluePathPoints;
            GameManager.gm.blueOutPlayers += 1;
        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[2])
        {
            outPlayerPiece = GameManager.gm.yellowPlayerPiece[outPlayer];
            pathPointToMoveOn_ = pathParent.yellowPathPoints;
            GameManager.gm.yellowOutPlayers += 1;
        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[3])
        {
            outPlayerPiece = GameManager.gm.greenPlayerPiece[outPlayer];
            pathPointToMoveOn_ = pathParent.greenPathPoints;
            GameManager.gm.greenOutPlayers += 1;
        }
        outPlayerPiece.isReady = true;
        outPlayerPiece.transform.position = pathPointToMoveOn_[0].transform.position;
        outPlayerPiece.numberOfStepsAlreadyMoved = 1;

        outPlayerPiece.previousPathPoint = pathPointToMoveOn_[0];
        outPlayerPiece.currentPathPoint = pathPointToMoveOn_[0];
        outPlayerPiece.currentPathPoint.AddPlayerPiece(outPlayerPiece);

        GameManager.gm.RemovePathPoint(outPlayerPiece.previousPathPoint);
        GameManager.gm.AddPathPoint(outPlayerPiece.currentPathPoint);
        GameManager.gm.canDiceRoll = true;
        GameManager.gm.selfDice = true;
        GameManager.gm.transferDice = false;
        GameManager.gm.numOfStepsToMove = 0;
        GameManager.gm.SelfRoll();

    }
    IEnumerator MoveSteps_Enum(int movePlayer)
    {
        if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[0])
        {
            outPlayerPiece = GameManager.gm.redPlayerPiece[movePlayer];
            pathPointToMoveOn_ = pathParent.redPathPoints;
            
        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[1])
        {
            outPlayerPiece = GameManager.gm.bluePlayerPiece[movePlayer];
            pathPointToMoveOn_ = pathParent.bluePathPoints;
            
        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[2])
        {
            outPlayerPiece = GameManager.gm.yellowPlayerPiece[movePlayer];
            pathPointToMoveOn_ = pathParent.yellowPathPoints;
           
        }
        else if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[3])
        {
            outPlayerPiece = GameManager.gm.greenPlayerPiece[movePlayer];
            pathPointToMoveOn_ = pathParent.greenPathPoints;
           
        }

        GameManager.gm.transferDice = false;
        yield return new WaitForSeconds(0.25f);
        int numOfStepsToMove = GameManager.gm.numOfStepsToMove;

      //  if (GameManager.gm.canMove)
       // {
            outPlayerPiece.currentPathPoint.RescaleAndRepositionAllPlayerPieces();
        
            for (int i = outPlayerPiece.numberOfStepsAlreadyMoved; i < (outPlayerPiece.numberOfStepsAlreadyMoved + numOfStepsToMove); i++)
            {
                if (isPathPointsAvailableToMove(numOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMoved, pathPointToMoveOn_))
                {
                    outPlayerPiece.transform.position = pathPointToMoveOn_[i].transform.position;

                    yield return new WaitForSeconds(0.25f);
                }
            }
        //}
        if (isPathPointsAvailableToMove(numOfStepsToMove, outPlayerPiece.numberOfStepsAlreadyMoved, pathPointToMoveOn_))
        {

            outPlayerPiece.numberOfStepsAlreadyMoved += numOfStepsToMove;

            GameManager.gm.RemovePathPoint(outPlayerPiece.previousPathPoint);
            outPlayerPiece.previousPathPoint.RemovePlayerPiece(outPlayerPiece);
            outPlayerPiece.currentPathPoint = pathPointToMoveOn_[outPlayerPiece.numberOfStepsAlreadyMoved - 1];

            if (outPlayerPiece.currentPathPoint.AddPlayerPiece(outPlayerPiece))
            {
                if (outPlayerPiece.numberOfStepsAlreadyMoved == 57)
                {
                    GameManager.gm.selfDice = true;

                }
                else
                {
                    if (GameManager.gm.numOfStepsToMove != 6)
                    {
                        GameManager.gm.transferDice = true;
                    }
                    else
                    {
                        GameManager.gm.selfDice = true;
                    }
                }
            }
            else
            {
                GameManager.gm.selfDice = true;
            }

            GameManager.gm.AddPathPoint(outPlayerPiece.currentPathPoint);
            outPlayerPiece.previousPathPoint = outPlayerPiece.currentPathPoint;


            GameManager.gm.numOfStepsToMove = 0;

        }
        GameManager.gm.canMove = true;
        GameManager.gm.RollingDiceManager();
        if (moveSteps_Coroutine != null)
        {
            StopCoroutine("moveSteps_Coroutine");
        }
    }
}
