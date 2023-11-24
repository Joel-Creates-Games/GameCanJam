using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int LastFruit = -1;
    public int LastPair = -1;
    public float LastX;
    public float LastY;

    public int Chain = 0;

    bool PoisonActive = false;
    public float PoisonWaitTime = 2;

    public int Score = 0;
    public float Health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PoisonActive)
        {
            StartCoroutine(PoisonRoutine());
        }
    }

    void OnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.GetComponent<FruitController>() != null)
            {
                CheckNextFruit(hit.transform.GetComponent<FruitController>());
            }
        }
    }

    void CheckNextFruit(FruitController fruit)
    {
        if (LastFruit == -1)
        {
            LastFruit = fruit.m_fruitType;
            LastPair = fruit.m_fruitPair;
            LastX = fruit.transform.position.x; LastY = fruit.transform.position.y;
            fruit.transform.localScale *= 1.3f;
            fruit.m_clicked = true;
            return;
        }
        if ((fruit.m_fruitType == LastFruit  || (fruit.m_fruitPair != -1 && fruit.m_fruitPair == LastPair)) &&  fruit.m_clicked == false)
        {
            if (Mathf.Abs(fruit.transform.position.x - LastX) <= 1 && Mathf.Abs(fruit.transform.position.y - LastY) <= 1)
            {
                Chain++;
                Score += Chain;
                fruit.transform.localScale *= 1.3f;
                LastFruit = fruit.m_fruitType;
                LastPair = fruit.m_fruitPair;
                LastX = fruit.transform.position.x; LastY = fruit.transform.position.y;
                fruit.m_clicked = true;
            }
        }
    }

    IEnumerator PoisonRoutine()
    {
        PoisonActive = true;
        yield return new WaitForSeconds(PoisonWaitTime);
        Health -= 1;
        PoisonWaitTime -= 0.02f;
        PoisonActive = false;
    }
}
