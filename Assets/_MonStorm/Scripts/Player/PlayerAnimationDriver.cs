using MonStorm.Core.Player;
using UnityEngine;

public class PlayerAnimationDriver : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public float crossfadeTime = 0.2f;
    [SerializeField] public float dampTime = 0.15f;
    public PlayerContext context;

    private int lastPlayed = 0;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void LateUpdate()
    {
        if(context == null) return;
        if (context.forceAnimationRestart)
        {
            lastPlayed = 0;
            context.forceAnimationRestart = false;
        }
        
        if(context.velocity.X == 0 && context.velocity.Z == 0)
        {
            animator.SetFloat("Speed", 0, dampTime, Time.deltaTime);
        }
        else
        {
            float xzMagnitude = Mathf.Sqrt(context.velocity.X * context.velocity.X + context.velocity.Z * context.velocity.Z);
            animator.SetFloat("Speed", xzMagnitude, dampTime, Time.deltaTime);
        }
        
        if(context.AnimationIntent != lastPlayed && context.AnimationIntent != 0)
        {
            
            float time = context.crossFadeOverride >= 0f ? context.crossFadeOverride : crossfadeTime;
           
            animator.CrossFadeInFixedTime(context.AnimationIntent, time);

            lastPlayed = context.AnimationIntent;
        }
         context.crossFadeOverride = -1f;
    }
}