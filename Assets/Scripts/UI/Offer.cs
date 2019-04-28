using UnityEngine;

public class Offer : MonoBehaviour
{
    public OfferData offer { get; set; }
    public UIOfferGenerator gen { get; set; }

    [SerializeField] private SimpleAudioEvent acceptSound;

    public void Accept()
    {
        GameManager.instance.TakeOffer(offer);
        gen.ClearOffers();
        acceptSound.Play(UIManager.instance.GetComponent<AudioSource>());
    }
}
