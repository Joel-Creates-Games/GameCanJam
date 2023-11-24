using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform m_Grid;
    [SerializeField] Transform[] m_allFruits;
    // Start is called before the first frame update
    void Start()
    {
        PlacePuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePuzzle()
    {
        int lastRandom = -1;
        for (int i = 0; i < m_Grid.childCount; i++)
        {
            for (int o = 0; o < m_Grid.GetChild(i).childCount; o++)
            {
                int random = lastRandom;
                int loopcount = 0;
                while (random == lastRandom)
                {
                    random = Random.Range(0, 9);
                    if (loopcount == 10)
                    {
                        break;
                    }
                    loopcount++;
                }
                Instantiate(m_allFruits[random], m_Grid.GetChild(i).GetChild(o).position, m_allFruits[random].rotation);
                lastRandom = random;
            }
        }
    }
}
