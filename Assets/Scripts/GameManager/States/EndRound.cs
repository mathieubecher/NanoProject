using UnityEngine;
using UnityEngine.SceneManagement;

public class EndRound : AbstractGameState
{
    private readonly AbstractController _winner;
    private readonly AbstractController _looser;
    
    public EndRound(GameManager gameManager) :
        base(gameManager)
    {
        gameManager.cinematic.Activate();
        _winner = gameManager.Controller1.Points > gameManager.Controller2.Points
            ? gameManager.Controller1
            : gameManager.Controller2;
        _looser = gameManager.Controller1.Points > gameManager.Controller2.Points
            ? gameManager.Controller2
            : gameManager.Controller1;
        
        ++_winner.RoundWon;

        if (_winner.RoundWon == 2 || _looser.RoundWon == 2)
        {
            AkSoundEngine.PostEvent("FB_EndGame", gameManager.soundManager);
            if (_winner.characterInfo.characterAssigned)
            {
                _winner.characterInfo.Character.AddComponent<LeaveEnd>();
            }
        }
        else
        {
            AkSoundEngine.PostEvent("FB_EndRound", gameManager.soundManager);
            if (_winner.characterInfo.characterAssigned)
            {
                _winner.characterInfo.Character.AddComponent<Leave>();
            }
        }

        if (_looser.characterInfo.characterAssigned) _looser.characterInfo.Character.AddComponent<FallDown>();
    }

    public override GameStateName Name()
    {
        return GameStateName.EndRound;
    }

    public override void Update()
    {
        base.Update();
        if (characterInPosition[0] && characterInPosition[1])
        {
            if (_winner.RoundWon == 2 || _looser.RoundWon == 2)
            {
                EndGame();
            }
            else
            {
                Object.Destroy(_winner.characterInfo.Character);
                _winner.characterInfo.characterAssigned = false;
                
                gameManager.SetState(new NewRound(gameManager));
            }
        }
    }

    private void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}