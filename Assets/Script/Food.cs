using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{

    [SerializeField] private int amount = 10;
    
    void Start()
    {
        transform.Rotate(Vector3.forward, Random.Range(0, 180));
    }

    public void Bite()
    {
        amount--;

        if (amount < 1)
        {
            Destroy(gameObject);
        }
    }

   
}
