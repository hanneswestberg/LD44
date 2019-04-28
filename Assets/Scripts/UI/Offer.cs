using UnityEngine;

public class Offer : MonoBehaviour
{
    public OfferData offer { get; set; }
    public UIOfferGenerator gen { get; set; }

    public void Accept()
    {
        GameManager.instance.TakeOffer(offer);
        gen.ClearOffers();
    }
}
