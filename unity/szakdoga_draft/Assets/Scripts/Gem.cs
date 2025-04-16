using UnityEngine;

public class Gem : MonoBehaviour, Iitem
{
    public void Collect()
    {
        Destroy(gameObject);
    }

   
}
