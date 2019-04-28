using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOfferGenerator : MonoBehaviour
{
    public GameObject offerPrefab;
    public GameObject itemPrefab;
    public GameObject canvas;

    private float frameOffset = 240;

    private List<GameObject> activeOffers = new List<GameObject>();
    private OfferData offer;

    public void CreateOffer(OfferData data)
    {
        GameObject inst = Instantiate(offerPrefab, canvas.transform);
        activeOffers.Add(inst);

        Offer o = inst.GetComponent<Offer>();
        o.gen = this;
        o.offer = data;
        inst.GetComponent<RectTransform>().anchoredPosition += new Vector2(frameOffset, 0);

        GameObject weapon = Instantiate(itemPrefab, inst.transform);
        GameObject armor = Instantiate(itemPrefab, inst.transform);

        weapon.GetComponent<ItemUI>().NewValues(data.Weapon);
        armor.GetComponent<ItemUI>().NewValues(data.Armor, -70);
        Text desc = inst.transform.Find("FlavorText").GetComponent<Text>();
        desc.text = "The gladiator master " + data.Name + " has offered to aquire you.";
        frameOffset += 155;
    }

    public void ClearOffers()
    {
        while(activeOffers.Count > 0)
        {
            Destroy(activeOffers[0]);
            activeOffers.RemoveAt(0);
        }
        frameOffset = 240;
    }
}
