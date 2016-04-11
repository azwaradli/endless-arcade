using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;
    PickupManager pickupManager;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        pickupManager = GameObject.Find("PickupManager").GetComponent<PickupManager>();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        SpawnPickup();
        Destroy (gameObject, 2f);
    }

    void SpawnPickup() {
        Vector3 spawnPosition = transform.position + new Vector3(0, 0.3f, 0);

        // Spawns one of our 2 powerup pickups randomly.
        // It's set up to spawn a pickup 40% of the time.
        // And the pickups are selected accordingly:
        // - 40% of the time it will be a bullet pickup
        // - 60% of the time it will be a health pickup
        float rand = Random.value;
        if (rand <= 0.4f) {
            // Bounce.
            if (rand <= 0.08f) {
                Instantiate(pickupManager.healthPickup, spawnPosition, transform.rotation);
            }
            // Health.
            else {
                Instantiate(pickupManager.bulletPickup, spawnPosition, transform.rotation);
            }
        }
    }
}
