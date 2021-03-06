using UnityEngine;

public class NewRound : AbstractGameState
{
    public NewRound(GameManager gameManager) :
        base(gameManager)
    {
        gameManager.cinematic.Activate();
        gameManager.Controller1.NewCharacter();
        gameManager.Controller2.NewCharacter();
        gameManager.RoundTimer = gameManager.ROUNDTIMER;
        gameManager.Controller1.Points = 0;
        gameManager.Controller2.Points = 0;
    }

    public override GameStateName Name()
    {
        return GameStateName.NewRound;
    }

    public override void Update()
    {
        base.Update();
        if (characterInPosition[0] && characterInPosition[1])
        {
            Object.Destroy(gameManager.Controller1.characterInfo.Character.GetComponent<Arrive>());
            Object.Destroy(gameManager.Controller2.characterInfo.Character.GetComponent<Arrive>());

            gameManager.SetState(new Fight(gameManager));
        }
    }
}