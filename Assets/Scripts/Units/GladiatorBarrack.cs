using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GladiatorBarrack : MonoBehaviour
{
    [SerializeField] protected SimpleAudioEvent liftSuccess;
    [SerializeField] protected SimpleAudioEvent liftFail;
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

        foreach(var weight in weigths) {
            weight.localScale = new Vector3(2f + (0.2f * data.Strength), 0.2f + (0.02f * data.Strength), 2f + (0.2f * data.Strength));
        }


        UpdateMaterials();
    }

    public void UpdateMaterials() {
        body.materials[1].color = data.SkinColor;
        foreach(var bodyPart in bodyParts) {
            bodyPart.material.color = data.SkinColor;
        }
    }

    public void PlayLiftSound(bool succeed) {
        if(succeed)
            liftSuccess.Play(liftSource);
        else
            liftFail.Play(liftSource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
