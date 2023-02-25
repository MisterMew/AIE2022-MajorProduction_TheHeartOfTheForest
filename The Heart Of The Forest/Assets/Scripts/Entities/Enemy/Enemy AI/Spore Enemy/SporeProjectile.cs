/*
 * Date Created: 28/09/2022
 * Author: Nicholas Connell
 */

using UnityEngine;
using UnityEngine.Events;

public class SporeProjectile : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;

    [Tooltip("The trail particle effect")]
    [SerializeField] private GameObject sporeTrail;
    [Tooltip("The particles to spawn when the projectile is destroyed")]
    [SerializeField] private GameObject sporeParticle;
    [Tooltip("When the projectile hts something")]
    [SerializeField] private UnityEvent OnProjectileHit;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject trail = Instantiate(sporeTrail, transform);
        Destroy(trail, 3.0f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //If a spore particle exists, spawn it 
        if (sporeParticle)
            Instantiate(sporeParticle, col.contacts[0].point, sporeParticle.transform.rotation);

        //Invoke unity event
        OnProjectileHit.Invoke();

        //Disable the mesh renderer
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false; // added by Nghia to prevent additional colisions
        m_rigidBody.gravityScale = 0;
        m_rigidBody.velocity = Vector2.zero;

        //Destroy the gameobject after 3 seconds
        Destroy(gameObject, 3.0f);
    }
}
