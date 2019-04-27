using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gladiator : MonoBehaviour
{
    /// <summary>
    /// The unit data reference for this gladiator
    /// </summary>
    public UnitData Data { get; private set; }
    /// <summary>
    /// Returns the current health of the gladiator
    /// </summary>
    public float CurrentHealth { get; private set; }
    /// <summary>
    /// If the gladiator is still alive
    /// </summary>
    public bool IsAlive { get; private set; }

    // Internal references
    [Header("Internal References:")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SimpleAudioEvent attackSound;
    [SerializeField] private SimpleAudioEvent takeDamageSound;
    [SerializeField] private SimpleAudioEvent dieSound;

    // Internal variables
    private float maxHealth;

    /// <summary>
    /// Initializes the data on this unit
    /// </summary>
    /// <param name="data">The data object</param>
    public virtual void SetUnitData(UnitData data) {
        Data = data;

        // TODO: Calculate max health here
        maxHealth = Data.Health * 10f;
        // TODO: Set current health to max health
        CurrentHealth = maxHealth;

        IsAlive = true;
    }

    /// <summary>
    /// Attacks a target
    /// </summary>
    /// <param name="target">The target gladiator</param>
    public virtual void Attack(Gladiator target) {
        if(IsAlive && target.TakeDamage(Data.Strength * 2f)) {
            attackSound.Play(audioSource);
        }
    }

    /// <summary>
    /// The gladiator takes damage
    /// </summary>
    /// <param name="amount">The amount of damage</param>
    /// <returns>If the gladiator took damage</returns>
    public virtual bool TakeDamage(float amount) {
        if(IsAlive) {
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, maxHealth);

            if(CurrentHealth <= 0) {
                IsAlive = false;
                dieSound.Play(audioSource);
                animator.SetBool("Dead", true);
                GameManager.instance.LivingGladiators.Remove(this);

                transform.GetChild(0).rotation = new Quaternion(90f, transform.rotation.y, transform.rotation.z, 0);
            }
            else {
                takeDamageSound.Play(audioSource);
                animator.SetTrigger("TakeDamage");
            }
            return true;
        }
        return false;
    }
}
