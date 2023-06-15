using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;

    void Start()
    {

    }

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        // define Laser direction
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // define Laser boundaries
        if (transform.position.y > 7.5f)
        {
            // check if obj has a parent (destroy parent, too)
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        // define Laser direction
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // define Laser boundaries
        if (transform.position.y < -7.5f)
        {
            // check if obj has a parent (destroy parent, too)
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    // laser collides with Player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
