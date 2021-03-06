﻿using UnityEngine;

public class FallDown : AbstractAnimation
{
    private static readonly int FallDownAnim = Animator.StringToHash("FallDown");
    private float timer = 0.2f;
    private ShaderCharacterEffect shader;
    private new Rigidbody rigidbody;
    
    protected override void Awake()
    {
        base.Awake();
        controller.EndDuel();
        gameObject.layer = 10;
        rigidbody = GetComponent<Rigidbody>();  
        transform.position += new Vector3(0,0,0.5f);
        transform.GetComponentInChildren<Animator>().SetTrigger(FallDownAnim);
        controller.characterInfo.characterAssigned = false;
    }
    private void Start()
    {
        shader = GetComponent<ShaderCharacterEffect>();
    }
    protected override void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                shader.BeginConsum();
                GameObject particle = Instantiate(Resources.Load("Characters/Braise"), transform) as GameObject;
                particle.transform.eulerAngles = new Vector3(-90, 0, 90);
                shader.particle = particle.GetComponent<ParticleSystem>();
                
            }
        }
        base.Update();
        rigidbody.velocity = Vector3.zero;
        if (!inPosition)
        {
            if (gameManager.StateName == GameStateName.EndRound) gameManager.CharacterInPosition(controller.PlayerNum);
            inPosition = true;
        }
    }
}
