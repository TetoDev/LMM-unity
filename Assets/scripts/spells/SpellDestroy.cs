using UnityEngine;

public class SpellDestroy : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f; // Delay before destroying the spell object

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyDelay); // Destroy the spell object after the specified delay
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
