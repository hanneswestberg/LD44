using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladiatorBarrack : MonoBehaviour
{
    [SerializeField] protected SimpleAudioEvent liftSound;
    [SerializeField] protected AudioSource liftSource;
    [SerializeField] protected List<Transform> weigths = new List<Transform>();

    [Space(10)]
    [SerializeField] protected Transform leftArm;
    [SerializeField] protected Transform rightArm;
    [SerializeField] protected Transform graphics;

    [Space(10)]
    [SerializeField] protected List<Renderer> bodyParts = new List<Renderer>();
    [SerializeField] protected Renderer body;

    private UnitData data;

    public Animator Anim { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        data = GameManager.instance.PlayerData;
        Anim = GetComponent<Animator>();

        leftArm.localScale = Vector3.one + (Vector3.one * 0.05f * data.Strength) - (Vector3.one * 0.25f);
        rightArm.localScale = Vector3.one + (Vector3.one * 0.05f * data.Strength) - (Vector3.one * 0.25f);
        graphics.localScale = Vector3.one + (Vector3.one * 0.05f * data.Strength) - (Vector3.one * 0.25f);

        body.materials[1].color = data.SkinColor;
        foreach(var bodyPart in bodyParts) {
            bodyPart.material.color = data.SkinColor;
        }
    }

    public void PlayLiftSound() {
        liftSound.Play(liftSource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
