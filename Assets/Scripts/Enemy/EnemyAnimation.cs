using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAnimationState
{
    Idle,
    Move,
    Attack,
    Hurt,
    Die,
    Stun,
    Throw,
    Roll
}

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool faceRightByDefault = true;
    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayState(EnemyAnimationState state)
    {
        if (state == EnemyAnimationState.Stun)
            state = EnemyAnimationState.Idle;
        animator.Play(state.ToString());
    }
    public void SetDirection(Vector2 direction)
    {
        if (direction.x == 0) return;
        bool movingRight = direction.x > 0;
        if (faceRightByDefault) 
            spriteRenderer.flipX = !movingRight;
        else
            spriteRenderer.flipX = movingRight;
    }
}
