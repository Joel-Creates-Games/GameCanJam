using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinController : MonoBehaviour
{
    public PlayerController m_player;
    Animator m_anim;
    NavMeshAgent m_agent;

    public float m_damage = 10;
    public bool m_attacked = false;

    public float m_health = 15;
    // Start is called before the first frame update
    void Start()
    {
        m_anim = transform.GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_player.transform.position) > 15f)
        {
            m_agent.destination = m_player.transform.position - new Vector3(0, 10, 0);
            m_anim.SetBool("Walking", true);
            m_anim.SetBool("Attacking", false);
        }
        else
        {
            m_agent.destination = transform.position;
            m_anim.SetBool("Walking", false);
            m_anim.SetBool("Attacking", true);
        }
        CheckAttack();
    }

    void CheckAttack()
    {
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            print(Math.Abs(m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime%1 - 1));
            if (Math.Abs(m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime%1 - 1) < 0.3f && m_attacked == false)
            {
                m_player.TakeDamage(m_damage);
                m_attacked = true;
            }
            else if (Math.Abs(m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 -1) < 1f && Math.Abs(m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 -1) > 0.3f && m_attacked == true)
            {
                m_attacked = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
