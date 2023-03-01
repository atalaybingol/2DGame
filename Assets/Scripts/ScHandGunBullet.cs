using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScHandGunBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletLifeTime = 90f;

    public void shootBullet(float angle)
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * Mathf.Cos(angle * Mathf.PI / 180), bulletSpeed * Mathf.Sin(angle * Mathf.PI / 180));
        Destroy(this.gameObject, bulletLifeTime);
    }

    /*
    void OnBecameInvisible()
    {
        Destroy(this.gameObject, bulletLifeTime);
    }*/

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Obstacle"))
        {

            Collider2D m_ObjectCollider = this.gameObject.GetComponent<Collider2D>();
            m_ObjectCollider.isTrigger = false;
            Destroy(this.gameObject, 0.5f);
        }
    }

    public float getBulletDamage(){ return bulletDamage; }
}
