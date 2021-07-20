using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public Shop shop;

    public void Awake()
    {
        shop = transform.parent.parent.parent.parent.parent.GetComponent<Shop>();
    }

    public void Buy()
    {
        shop.TryToBuySkin(transform.parent);
    }

    public void Apply()
    {
        shop.TryToApplySkin(transform.parent);
    }
}
