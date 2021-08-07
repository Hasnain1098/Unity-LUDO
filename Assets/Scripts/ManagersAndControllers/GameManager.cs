using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;    
    public RollingDice rolledDice;
    public int numOfStepsToMove;
    public bool canMove = true;

    List<PathPoint> playerOnPathPointsList = new List<PathPoint>();
    public bool canDiceRoll = true;
    public bool transferDice = false;
    public bool selfDice = false;
   

    public int blueOutPlayers;
    public int redOutPlayers;
    public int greenOutPlayers;
    public int yellowOutPlayers;

    public int blueCompletePlayer;
    public int redCompletePlayer;
    public int greenCompletePlayer;
    public int yellowCompletePlayer;

    public RollingDice[] manageRollingDice;

    public PlayerPiece[] redPlayerPiece;
    public PlayerPiece[] bluePlayerPiece;
    public PlayerPiece[] yellowPlayerPiece;
    public PlayerPiece[] greenPlayerPiece;

    public int totalPlayerCanPlay;

    public AudioSource audioSource;

    private void Awake()
    {
        gm = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void AddPathPoint(PathPoint pathPoint_)
    {
        playerOnPathPointsList.Add(pathPoint_);
    }
    public void RemovePathPoint(PathPoint pathPoint_)
    {
        if(playerOnPathPointsList.Contains(pathPoint_))
        {
            playerOnPathPointsList.Remove(pathPoint_);
        }
        else
        {
            Debug.Log("Path Point is not Found to be removed.");
        }
    }
    public void RollingDiceManager()
    {
        if(GameManager.gm.transferDice)
        {
            if (GameManager.gm.numOfStepsToMove != 6)
            {
                ShiftDice();
            }
            GameManager.gm.canDiceRoll = true;
        }
      
        else
        {
            if(GameManager.gm.selfDice)
            {
                GameManager.gm.selfDice = false;
                GameManager.gm.canDiceRoll = true;
                GameManager.gm.SelfRoll();
            }
        }

    }
    public void SelfRoll()
    {
        if(GameManager.gm.totalPlayerCanPlay==1 && GameManager.gm.rolledDice==GameManager.gm.manageRollingDice[2])
        {
            Invoke("rolled", 0.6f);
        }
    }
    void rolled()
    {
        GameManager.gm.manageRollingDice[2].mouseRoll();
    }
    void ShiftDice()
    {
        int nextDice;
        if(GameManager.gm.totalPlayerCanPlay==1)
        {
            if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[0])
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);

                passout(0);
                GameManager.gm.manageRollingDice[2].mouseRoll();
            }
            else
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);
                passout(2);
            }
        }
        else if (GameManager.gm.totalPlayerCanPlay==2)
        {
            if(GameManager.gm.rolledDice==GameManager.gm.manageRollingDice[0])
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(false);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(true);
                passout(0);
            }
            else
            {
                GameManager.gm.manageRollingDice[0].gameObject.SetActive(true);
                GameManager.gm.manageRollingDice[2].gameObject.SetActive(false);
                passout(2);
            }
        } 
        else if (GameManager.gm.totalPlayerCanPlay==3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                {
                    nextDice = 0;
                }
                else
                {
                    nextDice = i + 1;
                }
                i = passout(i);
                if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[i])
                {
                    GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                }

            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    nextDice = 0;
                }
                else
                { 
                    nextDice = i + 1;
                }
                i = passout(i);
                if (GameManager.gm.rolledDice == GameManager.gm.manageRollingDice[i])
                {
                    GameManager.gm.manageRollingDice[i].gameObject.SetActive(false);
                    GameManager.gm.manageRollingDice[nextDice].gameObject.SetActive(true);
                }

            }
        }
    }

    int passout(int i)
    {
        if (i == 0){ if (GameManager.gm.blueCompletePlayer == 4) { return (i + 1); } }
        else if (i == 1){ if (GameManager.gm.blueCompletePlayer == 4) { return (i + 1); } }
        else if (i == 2){ if (GameManager.gm.blueCompletePlayer == 4) { return (i + 1); } }
        else if (i == 3){ if (GameManager.gm.blueCompletePlayer == 4) { return (i + 1); } }
                return i;
    }
}

