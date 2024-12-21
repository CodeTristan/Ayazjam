using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTile : MonoBehaviour
{
    [SerializeField] private BoxCollider attackCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem[] particleSystems;

    public void Init(float AttackDelay,float AttackTileDestroyTimer = 0.5f)
    {
        StartCoroutine(Attack(AttackDelay,AttackTileDestroyTimer));
    }


    private IEnumerator Attack(float AttackDelay, float AttackTileDestroyTimer)
    {
        animator.SetTrigger("PreAttack");
        yield return new WaitForSeconds(AttackDelay);
        animator.SetTrigger("Attack");
        ActivateParticles();
        attackCollider.enabled = true;
        yield return new WaitForSeconds(AttackTileDestroyTimer);
        Destroy(gameObject);
    }

    private void ActivateParticles()
    {
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
    }
}
