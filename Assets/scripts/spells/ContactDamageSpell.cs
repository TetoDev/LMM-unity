using UnityEngine;

public class ContactDamageSpell : MonoBehaviour
{
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private float spellDamage = 10f; // Damage dealt by the attack

    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component from the enemy GameObject
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetFloat("AttackActive") > 0f)
        {
            attackCollider.enabled = true; // Enable the attack collider when the attack animation is active
            DealDamage(); // Call the DealDamage method to check for players in the attack range
        }
        else
        {
            attackCollider.enabled = false; // Disable the attack collider when the attack animation is not active
        }
    }

    private void DealDamage()
    {
        ContactFilter2D filter = new ContactFilter2D(); // Create a new ContactFilter2D object
        filter.useTriggers = true; // Use triggers for the collider detection

        Collider2D[] colliders = new Collider2D[10]; // Array to store detected colliders
        int playersHit = Physics2D.OverlapCollider(attackCollider, filter, colliders); // Detect players in the attack range

        for (int i = 0; i < playersHit; i++)
        {
            PlayerMovement player = colliders[i].GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(spellDamage); // Call the TakeDamage method on the player
            }
        }
    }
}
