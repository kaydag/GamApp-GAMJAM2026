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
    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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
        animator.Play(state.ToString());
    }
    public void SetDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetFloat("Horizontal", direction.x > 0 ? 1 : -1);
            animator.SetFloat("Vertical", 0);
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", direction.y > 0 ? 1 : -1);
        }
    }
}
