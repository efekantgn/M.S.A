using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    private GameObject _player;

    public void GetDamage(int amount)
    {
        health -= amount;
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
   


    


}
