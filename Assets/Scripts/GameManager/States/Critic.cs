using UnityEngine;

public class Critic : AbstractGameState
{
    public Critic(GameManager gameManager, GameObject character) :
        base(gameManager)
    {
        gameManager.cinematic.Activate(false);
        gameManager.touchCooldown = gameManager.TOUCHCOOLDOWN;
        gameManager.touched = (gameManager.Controller1.characterInfo.Character == character) ? gameManager.Controller1 : gameManager.Controller2;
        gameManager.touchValue = ((gameManager.Controller1.characterInfo.Character == character ?
                           gameManager.Controller2 :
                           gameManager.Controller1).StateName == ControllerStateName.VerticalAttack) ? 2 : 1;
        gameManager.vibration.Begin(gameManager.Controller1);
        gameManager.vibration.Begin(gameManager.Controller2);
    }

    public override GameStateName Name()
    {
        return GameStateName.Critic;
    }

    public override void Exit()
    {
        base.Exit();
        gameManager.touched = null;
        
        if (gameManager.RoundTimer > 5 || gameManager.Controller1.Points == gameManager.Controller2.Points)
        {
            gameManager.SetState(new Reset(gameManager));
        }
        else
        {
            gameManager.SetState(new EndRound(gameManager));
        }
    }

    public override void Update()
    {
        base.Update();
        
        gameManager.RoundTimer -= Time.deltaTime;
        gameManager.CheckDir();
        gameManager.touchCooldown -= Time.deltaTime;
        
        // Check if timer is over
        if (gameManager.touchCooldown < 0)
        {
            Kill(gameManager.touched);
            PlayKillSound(gameManager.touched);
            Exit();
        }
    }

    private void PlayKillSound(AbstractController killed)
    {
        AkSoundEngine.PostEvent("SFX_Hit_Kill", gameManager.soundManager);
        if (killed.PlayerNum == 1)
        {
            AkSoundEngine.PostEvent("PONE_Kill_Taiko", gameManager.soundManager);
        }
        else
        {
            AkSoundEngine.PostEvent("PTWO_Kill_Taiko", gameManager.soundManager);
        }
    }

    public override void Touch(GameObject character)
    {
        if (gameManager.touched.characterInfo.Character == character) return;
        
        int actualTouchValue = (((gameManager.Controller1.characterInfo.Character == character) ?
                                    gameManager.Controller2 :
                                    gameManager.Controller1).StateName == ControllerStateName.VerticalAttack) ? 2 : 1;
        
        // if both attack have same value
        if (actualTouchValue == gameManager.touchValue) { 
            Kill(gameManager.Controller1);
            Kill(gameManager.Controller2);
            AkSoundEngine.PostEvent("SFX_Hit_Draw", gameManager.soundManager);
            AkSoundEngine.PostEvent("SFX_Hit_Draw_Taiko", gameManager.soundManager);
        }
        else 
        {
            // if new attack value is greater than the first, reverse touched
            if (actualTouchValue > gameManager.touchValue)
            {
                gameManager.touched = (gameManager.touched == gameManager.Controller1) ? gameManager.Controller2 : gameManager.Controller1;
            }
            Kill(gameManager.touched);
            PlayKillSound(gameManager.touched);
        }

        Exit();
    }
    
    private void Kill(AbstractController character)
    {
        GameObject dieCharacter = character.characterInfo.Character;
        Object.Destroy(character.characterInfo.Saber1.gameObject);
        Object.Destroy(character.characterInfo.Saber2.gameObject);
        
        character.characterInfo.characterAssigned = false;
        
        AbstractController killer = character == gameManager.Controller1 ? gameManager.Controller2 : gameManager.Controller1;
        ++killer.Points;
        
        dieCharacter.AddComponent<Die>();
    }
}