using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public int ghostMultiplier { get; private set; }

    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        NewGame();
    }

    
    private void Update()
    {
        // Checks when player loses all their lives and starts a New Game
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    // Defines what a new game is
    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    // Returns everything to its original state with all pellets active
    private void NewRound()
    {
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(true);
        }

        this.pacman.gameObject.SetActive(true);
    }

    // Returns everything to its original state without touching pellets
    private void ResetState()
    {
        ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(true);
        }

        this.pacman.gameObject.SetActive(true);
    }

    // When player loses, hides all ghosts and pacman from the screen
    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    // Update score based on incoming score()
    private void SetScore(int score)
    {
        this.score = score;
    }

    // Update score based on incoming lives()
    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    // Determines what happens when a Ghost is eaten (with Multiplier when multiple ghosts are eaten during duration)
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    // Determines what happens when a Ghost eats Pacman
    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if(this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    // Determines what happens when a pellet is eaten
    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    // Determines what happens when a pellet/powerpellet is eaten
    public void PowerPelletEaten(PowerPellet pellet)
    {
        PelletEaten(pellet);

        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    // Checks if there are still pellets on the board
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    // Resets ghost multiplier with one command
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
