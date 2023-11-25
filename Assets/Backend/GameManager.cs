using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform m_Grid;
    [SerializeField] Transform[] m_allFruits;
    List<Transform> currentFruits = new List<Transform>();
    public Transform SpawnPoints;
    float m_enemySpawnTime = 10;
    public GameObject Goblin;
    bool m_GoblinSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        PlacePuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GoblinSpawning == false)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    public void PlacePuzzle()
    {
        for (int i = 0; i < currentFruits.Count; i++)
        {
            Destroy(currentFruits[i].gameObject);
        }
        currentFruits = new List<Transform>();
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
                Transform newFruit = Instantiate(m_allFruits[random], m_Grid.GetChild(i).GetChild(o).position, m_allFruits[random].rotation);
                currentFruits.Add(newFruit);
                lastRandom = random;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        m_GoblinSpawning = true;
        yield return new WaitForSeconds(m_enemySpawnTime);
        GameObject newGobbo = Instantiate(Goblin, SpawnPoints.GetChild(Random.Range(0, SpawnPoints.childCount)).position, Quaternion.identity);
        newGobbo.SetActive(true);
        if (m_enemySpawnTime > 2f)
        {
            m_enemySpawnTime -= 0.3f;
        }
        m_GoblinSpawning = false;
    }
}
