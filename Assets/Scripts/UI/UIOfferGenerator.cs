using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOfferGenerator : MonoBehaviour
{
    public GameObject offerPrefab;
    public GameObject soldPrefab;
    public GameObject itemPrefab;
    public GameObject canvas;

    private float frameOffset = 300;

    private List<GameObject> activeOffers = new List<GameObject>();

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
        desc.text = "The gladiator master <color=yellow>" + data.Name + "</color> has offered to aquire you.";
        frameOffset += 200;
    }

    public void CreateSold(OfferData data)
    {
        GameObject inst = Instantiate(soldPrefab, canvas.transform);
        activeOffers.Add(inst);

        Offer o = inst.GetComponent<Offer>();
        o.gen = this;
        o.offer = data;
        inst.GetComponent<RectTransform>().anchoredPosition += new Vector2(frameOffset, 0);

        Text desc = inst.transform.Find("FlavorText").GetComponent<Text>();
        desc.text = "Due to your performance you were sold to the gladiator master <color=yellow>" + data.Name + "</color> and are forced to accept their equipment.";
        frameOffset += 200;
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
