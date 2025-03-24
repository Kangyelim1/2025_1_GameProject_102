using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public float Timer = 1.0f;
    public int AttackPoint = 50;

    // Start is called before the first frame update
    void Start()
    {
        Health += 100;
    }

    // Update is called once per frame
    void Update()
    {
      

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Health -= AttackPoint;
        }

        CheckDeath();
    }

    void CharacterHealthUp()
    {
        Timer -= Time.deltaTime;

        if(Timer <=0)
        {
            Timer = 1;
            Health += 10;
        }
    }
    public void CharcterHit(int Damage)
    {
        Health -= Damage;
    }
    void CheckDeath()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
