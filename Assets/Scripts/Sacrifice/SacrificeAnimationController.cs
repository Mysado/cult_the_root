using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject RagdollParent;
    public bool IsWalking;
    public bool IsFalling;
    private static readonly int Falling = Animator.StringToHash("IsFalling");
    private static readonly int Walking = Animator.StringToHash("IsWalking");
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWalkBool(bool value)
    {
        IsWalking = value;
        animator.SetBool(Walking,value);
    }

    public void SetFallingBool(bool value)
    {
        IsFalling = value;
        RagdollParent.SetActive(value);
        animator.SetBool(Falling,value);
        animator.enabled = !value;
    }
}
