using UnityEngine;

public class EnemyCastSpell : MonoBehaviour
{
    [SerializeField] private EnemyFindPlayer findPlayer;
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyDirection direction;
    [SerializeField] private GameObject spellPrefab;

    [SerializeField] private float minimumDistance = 1f; // Minimum distance to cast the spell
    [SerializeField] private float maximumDistance = 10f; // Maximum distance to cast the spell
    [SerializeField] private float spellCooldown = 1f; // Cooldown time between spells

    private Animator anim;

    private Vector3 spawnPosition;
    private bool autoSpell = true;
    private float lastSpellTime = 0f; // Time of the last spell cast
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPosition = transform.position; // Store the initial position of the enemy
        anim = GetComponent<Animator>();
        if (spellPrefab == null) {
            Debug.LogError("Spell prefab is not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!autoSpell) return; // If auto spell is disabled, do not cast spells
        if (checkDistance() && !health.IsStunned() && !health.IsDead()) {
            if (Time.time - lastSpellTime >= spellCooldown) {
                CastSpell(spawnPosition, true);
                spellCooldown = Time.time + spellCooldown; // Reset the cooldown
            }
        }
    }

    private bool checkDistance () {
        return findPlayer.distance >= minimumDistance && findPlayer.distance <= maximumDistance;
    }

    public void CastSpell(Vector3 spawnPosition, bool playAnimation)
    {
        // Instantiate the spell prefab at the enemy's position
        this.spawnPosition = spawnPosition; // Update the spawn position
        Instantiate(spellPrefab, spawnPosition, Quaternion.identity);
        lastSpellTime = Time.time; // Update the last spell time
        if (playAnimation) {
            anim.SetTrigger("cast"); // Trigger the casting animation
        }
        
    }

    public void SetSpawnPosition(Vector3 position)
    {
        spawnPosition = position; // Update the spawn position
    }
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition; // Return the spawn position
    }

    public bool IsCasting(){
        return Time.time - lastSpellTime < spellCooldown; // Check if the enemy is currently casting a spell
    }

    public void DisableAutoSpell () {
        autoSpell = false; // Disable auto spell casting
    }
    public void EnableAutoSpell () {
        autoSpell = true; // Enable auto spell casting
    }

}
