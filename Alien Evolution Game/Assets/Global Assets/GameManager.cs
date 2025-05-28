using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class plantSpawner
{
    public int count;
    public GameObject prefab;
    public Transform parent;
    public Vector2 ageRange;
}

public class GameManager : MonoBehaviour
{
    public List<GameObject> plants;
    public plantSpawner[] plantInits;
    void Start()
    {
        foreach (plantSpawner p in plantInits)
        {
            for (int i = 0; i < p.count; i++)
            {
                Vector3 newPos;
                bool validPos;
                do
                {
                    newPos = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), 0);
                    validPos = true;
                    foreach (GameObject plant in plants)
                    {
                        float minDist = Mathf.Min(p.prefab.GetComponent<Plant>().minSpawnDist, plant.GetComponent<Plant>().minSpawnDist);
                        if (Vector3.Distance(newPos, plant.transform.position) < minDist)
                        {
                            validPos = false;
                            break;
                        }
                    }

                }
                while (!validPos);
                GameObject g = Instantiate(p.prefab, newPos, Quaternion.identity, p.parent);
                g.GetComponent<Plant>().age = Random.Range(p.ageRange.x, p.ageRange.y);
            }
        }
    }
}
