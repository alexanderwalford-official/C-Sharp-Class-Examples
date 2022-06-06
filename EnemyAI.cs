using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int Health = 100;
    public GameObject player;
    public GameObject AttackAnim;
    public GameObject WalkAnim;
    public GameObject IdleAnim;
    public GameObject DeathAnim;
    public GameObject TakeHitAnim;
    public bool ChasePlayer = true;
    public float MaxRange = 10;
    public float AttackRange = 2;
    public int AttackInterval = 2;
    public float MoveSpeed = 0.001f;
    public int Damage = 5;

    bool isattacking = false;
    bool ishit = false;
    bool isdead = false;

    // Update is called once per frame
    void Update()
    {
        if (!isdead)
        {
            // always look at player
            this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.transform.position.z));
        }

        float playerposx = player.transform.position.x;
        float playerposz = player.transform.position.z;
        float enemyposx = this.gameObject.transform.position.x;
        float enemyposy = this.gameObject.transform.position.y;
        float enemyposz = this.gameObject.transform.position.z;

        // now we need to calculate the distance from the player

        float xdist = playerposx - enemyposx + AttackRange;
        float zdist = playerposz - enemyposz + AttackRange;
        

        if ((player.transform.position - this.transform.position).sqrMagnitude > MaxRange * MaxRange && !ishit && !isdead)
        {
            // idle
            IdleAnim.SetActive(true);
            WalkAnim.SetActive(false);
            AttackAnim.SetActive(false);
        }
        else
        {
            if ((player.transform.position - this.transform.position).sqrMagnitude < AttackRange * AttackRange && !ishit && !isdead)
            {
                // enemy can stop moving and should try to attack
                AttackPlayer();
            }
            else if (Health != 0 && !ishit)
            {
                // now move the enemy towards the player
                // we need to check if the enemy's position is less or more than the player's position

                if (enemyposx > playerposx)
                {
                    WalkAnim.SetActive(true);
                    IdleAnim.SetActive(false);
                    AttackAnim.SetActive(false);
                    // x pos is greater than player pos x
                    if (enemyposz > playerposz)
                    {
                        this.gameObject.transform.position = new Vector3(enemyposx - MoveSpeed, enemyposy, enemyposz - MoveSpeed);
                    }
                    else
                    {
                        // x pos is greater than player pos x
                        // z pos is less than player pos z
                        this.gameObject.transform.position = new Vector3(enemyposx - MoveSpeed, enemyposy, enemyposz + MoveSpeed);
                    }
                }
                else
                {
                    WalkAnim.SetActive(true);
                    IdleAnim.SetActive(false);
                    AttackAnim.SetActive(false);
                    // x pos is less than player pos x
                    if (enemyposz > playerposz)
                    {
                        // x pos is less than player pos x
                        // z pos is greater than player pos z
                        this.gameObject.transform.position = new Vector3(enemyposx + MoveSpeed, enemyposy, enemyposz - MoveSpeed);
                    }
                    else
                    {
                        // x pos is less than pos x
                        // z pos is less than player pos z
                        this.gameObject.transform.position = new Vector3(enemyposx + MoveSpeed, enemyposy, enemyposz + MoveSpeed);
                    }
                }
            }
        }

        // health handler

        if (Health <= 0)
        {
            isdead = true;
            TakeHitAnim.SetActive(false);
            WalkAnim.SetActive(false);
            IdleAnim.SetActive(false);
            AttackAnim.SetActive(false);
            DeathAnim.SetActive(true);
        }
    }

    public void TakeHit ()
    {
        if (!isdead)
        {
            ishit = true;
            TakeHitAnim.SetActive(true);
            WalkAnim.SetActive(false);
            IdleAnim.SetActive(false);
            AttackAnim.SetActive(false);
            StartCoroutine(WaitChangeAnim());
        }
    }

    public void AttackPlayer ()
    {
        if (!isattacking && !isdead)
        {
            isattacking = true;
            WalkAnim.SetActive(false);
            IdleAnim.SetActive(false);
            AttackAnim.SetActive(true);
            StartCoroutine(DealDamage());
        }
    }

    IEnumerator DealDamage ()
    {
        yield return new WaitForSecondsRealtime(AttackInterval);
        isattacking = false;
        player.GetComponent<PlayerStats>().Health = player.GetComponent<PlayerStats>().Health - Damage;
    }

    IEnumerator WaitChangeAnim ()
    {
        yield return new WaitForSecondsRealtime(1.4f);
        AttackAnim.SetActive(false);
        TakeHitAnim.SetActive(false);
        IdleAnim.SetActive(true);
        ishit = false;
    }

}
