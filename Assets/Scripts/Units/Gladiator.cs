using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    public bool CanMove { get; set; }

    // Internal references
    [Header("Internal References:")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected SimpleAudioEvent attackSound;
    [SerializeField] protected SimpleAudioEvent takeDamageSound;
    [SerializeField] protected SimpleAudioEvent dieSound;
    [SerializeField] protected Canvas healthBarCanvas;
    [SerializeField] protected RectTransform healthBar;
    [SerializeField] protected Text healthValue;
    [SerializeField] protected GameObject hitTextPrefab;
    [SerializeField] protected ParticleSystem bloodParticles; 
    [SerializeField] protected Text nameText;

    [SerializeField] protected Transform leftArm;
    [SerializeField] protected Transform rightArm;
    [SerializeField] protected Transform graphics;

    [SerializeField] protected List<Renderer> bodyParts = new List<Renderer>();
    [SerializeField] protected Renderer body;

    [Header("Settings:")]
    [SerializeField, Range(0f, 5f)] protected float attackDistance;
    [SerializeField, Range(0f, 5f)] protected float attackCooldown;

    [SerializeField] private Targets targets;


    public Action OnUnitKilled;


    // Internal variables
    protected float maxHealth;
    protected float currentAttackCooldown;
    protected bool canAttack;
    protected bool attackFinished = false;


    /// <summary>
    /// Initializes the data on this unit
    /// </summary>
    /// <param name="data">The data object</param>
    public virtual void SetUnitData(UnitData data) {
        Data = data;

        // TODO: Calculate max health here
        maxHealth = (Data.Health + Data.Armor.Health + Data.Weapon.Health) * 8f;
        // TODO: Set current health to max health
        CurrentHealth = maxHealth;

        IsAlive = true;

        // UI
        healthBarCanvas.enabled = true;
        healthValue.text = CurrentHealth.ToString();
        nameText.text = Data.Name;

        leftArm.localScale = Vector3.one + (Vector3.one * 0.06f * Data.Strength) - (Vector3.one * 0.25f);
        rightArm.localScale = Vector3.one + (Vector3.one * 0.06f * Data.Strength) - (Vector3.one * 0.25f);
        graphics.localScale = Vector3.one + (Vector3.one * 0.02f * Data.Strength) - (Vector3.one * 0.25f);

        body.materials[1].color = Data.SkinColor;
        foreach(var bodyPart in bodyParts) {
            bodyPart.material.color = Data.SkinColor;
        }
    }

    protected virtual void Update() {
        currentAttackCooldown -= Time.deltaTime;
        canAttack = (currentAttackCooldown < 0);
    }

    /// <summary>
    /// Attacks a target
    /// </summary>
    /// <param name="target">The target gladiator</param>
    public virtual void Attack() {
        attackFinished = false;
        animator.SetTrigger("Attack");
        StartCoroutine(DelayedAttack());
        currentAttackCooldown = attackCooldown;
    }

    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(.55f);
        foreach (var target in targets.ValidTargets)
        {
            if (IsAlive && target != null && target.TakeDamage(this, (Data.Strength + Data.Armor.Strength + Data.Weapon.Strength) * 2f))
            {
                attackSound.Play(audioSource);
            }
        }
        attackFinished = true;
    }

    /// <summary>
    /// The gladiator takes damage
    /// </summary>
    /// <param name="amount">The amount of damage</param>
    /// <returns>If the gladiator took damage</returns>
    public virtual bool TakeDamage(Gladiator attacker, float amount) {
        if(IsAlive) {
            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, maxHealth);

            if(CurrentHealth <= 0) {
                IsAlive = false;
                dieSound.Play(audioSource);
                animator.SetBool("Dead", true);
                GameManager.instance.LivingGladiators.Remove(this);
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;

                // Callback to the attacker that we died
                if(attacker.OnUnitKilled != null)
                    attacker.OnUnitKilled.Invoke();
            }
            else {
                takeDamageSound.Play(audioSource);
                animator.SetTrigger("TakeDamage");
            }

            // Update UI
            healthBarCanvas.enabled = (CurrentHealth < maxHealth && IsAlive);
            healthBar.sizeDelta = new Vector2((CurrentHealth / maxHealth) * 100f, healthBar.sizeDelta.y);
            healthValue.text = CurrentHealth.ToString();
            GameObject go = Instantiate(hitTextPrefab, transform);
            go.transform.localPosition = new Vector3(0f, 2.2f, 0f);
            go.GetComponent<HitValueScript>().Value = amount;
            bloodParticles.Play();

            return true;
        }
        return false;
    }
}
