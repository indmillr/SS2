using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("The Player is NULL!");
        }

        _audioSource = GetComponent<AudioSource>();

        // assign component to Anim
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL!");
        }
       
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            // handle possible unknown quantity of lasers assigned to Enemy
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        // move Enemy down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // respawn Enemy at top with random Xpos
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if Enemy hits Player: damage Player, destroy Enemy
        if (other.tag == "Player")
        {
            // make sure Player exists
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            // trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }

        // if Enemy hits Laser: destroy Laser, destroy Enemy
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            // call to Player to add points
            if (_player != null)
            {
                _player.AddScore(10);
            }

            // trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            // remove the collider to prevent double explosion audio
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
