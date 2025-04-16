using System;
using UnityEngine;

public class Gem : MonoBehaviour, Iitem
{
    public static event Action<int> OnGemCollect;
    public int worth = 5;
    public void Collect()
    {
        OnGemCollect.Invoke(worth);
        Destroy(gameObject);
    }

   
}
