using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player P1;
    public Player P2;

    public PlayerStats defaultPlayerStats = new PlayerStats
    {
        MaxHealth = 100,
        DamageValue = 10
    };

    public GameState State = GameState.ChooseAttack;

    public GameObject  gameOverPanel;

    public TMP_Text winnerText;

    private Player damagedPlayer;

    public enum GameState
    {
        ChooseAttack,

        Attacks,

        Damages,

        Draw,

        GameOver,
    }
    private void Start()
    {
        gameOverPanel.SetActive(false);
        P1.SetStats(defaultPlayerStats, true);
        P2.SetStats(defaultPlayerStats, true);
        P1.IsReady = true;
        P2.IsReady = true;
    }

    private void Update() {
        //ChooseAttack,
        switch (State)
        {
            case GameState.ChooseAttack:
                if(P1.AttackValue != null && P2.AttackValue != null)
                {
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.isClickable(false);
                    P2.isClickable(false);

                    State = GameState.Attacks;
                }
                break;

            case GameState.Attacks:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    damagedPlayer = GetDamagedPlayer();
                    if(damagedPlayer != null)
                    {
                        damagedPlayer.AnimateDamage();
                        State = GameState.Damages;
                    }
                    else
                    {
                        P1.AnimateDraw();
                        P2.AnimateDraw();
                        State = GameState.Draw;
                    }
                }
                break;
            case GameState.Damages:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                {

                    if (damagedPlayer == P1)
                    {
                        P1.ChangeHealth(-P2.stats.DamageValue);
                        P2.ChangeHealth(0);
                    }
                    else
                    {
                        P1.ChangeHealth(0);
                        P2.ChangeHealth(-P1.stats.DamageValue);
                    }

                    var winner = GetWinner();

                    if(winner == null)
                    {
                        ResetPlayer();
                        P1.isClickable(true);
                        P2.isClickable(true);
                        State = GameState.ChooseAttack;
                    }
                    else
                    {
                        gameOverPanel.SetActive(true);
                        winnerText.text = winner == P1 ? "Player 1 wins" : "Player 2 wins";
                        ResetPlayer();
                        State = GameState.GameOver;
                    }
                }
                break;
            case GameState.Draw:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false)
                {
                    ResetPlayer();
                    P1.isClickable(true);
                    P2.isClickable(true);
                    State = GameState.ChooseAttack;
                }
                break;
        }
        //GameOver,
    }

    private void ResetPlayer()
    {
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }

    private Player GetDamagedPlayer()
    {
        Attack? PlayerAtk1 = P1.AttackValue;
        Attack? PlayerAtk2 = P2.AttackValue;

        if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Paper)
        {
            return P1;
        }
        else if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Scissor)
        {
            return P2;
        }
         else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Rock)
        {
            return P2;
        }
         else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Scissor)
        {
            return P1;
        }
         else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Rock)
        {
            return P1;
        }
         else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Paper)
        {
            return P2;
        }
        return null;
    }

    private Player GetWinner()
    {
        if (P1.Health==0)
        {
            return P2;
        }
        else if(P2.Health == 0)
        {
            return P1;
        }
        else
        {
            return null;
        }
    }

    public void LoadScene (int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }


}
