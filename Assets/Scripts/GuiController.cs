using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiController : MonoBehaviour
{
    private int hitCountNumber = 0;

    Text latestHit;
    Text hitCount;

    // Start is called before the first frame update
    void Start()
    {
        latestHit = GameObject.FindWithTag("LatestHit").GetComponent<Text>();
        hitCount = GameObject.FindWithTag("HitCount").GetComponent<Text>();
    }

    public void setLatestHit(string text)
    {
        latestHit.text = text;
    }

    public void setHitCount(string text)
    {
        hitCount.text = text;
    }

    public void addHitCount()
    {
        hitCountNumber++;

        hitCount.text = "Hit Count: " + hitCountNumber;
    }

    public void resetHitCount()
    {
        hitCountNumber = 0;
        hitCount.text = "Hit Count: 0";
    }
}
