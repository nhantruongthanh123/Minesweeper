using UnityEngine;

public class number : MonoBehaviour
{
    private Animator animator;

    public void SetBombCount(int count)
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetInteger("bomb", count);
        }
    }
}
