using UnityEngine;

public class Reset : AbstractGameState
{
    public Reset(GameManager gameManager) :
        base(gameManager)
    {
        gameManager.cinematic.Activate();
        if (gameManager.Controller1.characterInfo.characterAssigned)
        {
            if (gameManager.Controller1.characterInfo.Character.transform.position.x >
                gameManager.posReposition1.position.x)
            {
                gameManager.Controller1.characterInfo.Character.AddComponent<Replace>();
            }
            else
            {
                gameManager.Controller1.characterInfo.Character.AddComponent<Wait>();
            }
        }
        else
        {
            gameManager.Controller1.NewCharacter();
        }
        
        if (gameManager.Controller2.characterInfo.characterAssigned)
        {
            if (gameManager.Controller2.characterInfo.Character.transform.position.x <
                gameManager.posReposition2.position.x)
            {
                gameManager.Controller2.characterInfo.Character.AddComponent<Replace>();
            }
            else
            {
                gameManager.Controller2.characterInfo.Character.AddComponent<Wait>();
            }
        }
        else
        {
            gameManager.Controller2.NewCharacter();
        }
    }

    public override GameStateName Name()
    {
        return GameStateName.Reset;
    }

    public override void Update()
    {
        base.Update();
        gameManager.RoundTimer -= Time.deltaTime;

        if (characterInPosition[0] && characterInPosition[1])
        {
            Object.Destroy(gameManager.Controller1.characterInfo.Character.GetComponent<AbstractAnimation>());
            Object.Destroy(gameManager.Controller2.characterInfo.Character.GetComponent<AbstractAnimation>());
            gameManager.SetState(new Fight(gameManager));
        }
    }
}