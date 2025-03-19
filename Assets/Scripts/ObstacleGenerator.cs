using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private Transform shortObstacles, longObstacles;
    [SerializeField] private GameObject[] shortPrefabs, longPrefabs;
    [SerializeField] private Transform player;

    private void OnEnable()
    {
        PlatformController.OnPlatformMoved += GenerateObstacles;
    }

    private void OnDisable()
    {
        PlatformController.OnPlatformMoved -= GenerateObstacles;
    }

    private void GenerateObstacles(float platformZPos)
    {
        // Decide to generate either short or long obstacles.
        bool isShort = Random.Range(0, 2) == 0;
        // Decide which paths to put obstacles. As long obstacles cannot be jumped over, 
        // they can be max 2 per line.
        int obstacleCount = isShort ? Random.Range(1, 4) : Random.Range(1, 3);

        Dictionary<float, bool> positions = new Dictionary<float, bool>()
        {
            { -1.5f, false },
            { 0f, false },
            { 1.5f, false }
        };

        for (int i = 0; i < obstacleCount; i++)
        {
            float chosenX = PickAFreePos(positions);
            if (chosenX == float.NaN) break;

            positions[chosenX] = true;

            Vector3 pos = new Vector3(chosenX, 0.1f, platformZPos + 5);

            // Look for if there is any inactive obstacle to reuse. If not, create one.
            if (isShort)
            {
                for (int y = 0; y < shortObstacles.childCount; y++)
                {
                    if (!shortObstacles.GetChild(y).gameObject.activeSelf)
                    {
                        shortObstacles.GetChild(y).position = pos;
                        shortObstacles.GetChild(y).gameObject.SetActive(true);
                        break;
                    }
                }

                Instantiate(shortPrefabs[Random.Range(0, shortPrefabs.Length)],
                    pos, Quaternion.identity, shortObstacles);
            }
            else
            {
                for (int y = 0; y < longObstacles.childCount; y++)
                {
                    if (!longObstacles.GetChild(y).gameObject.activeSelf)
                    {
                        longObstacles.GetChild(y).position = pos;
                        longObstacles.GetChild(y).gameObject.SetActive(true);
                        break;
                    }
                }

                Instantiate(longPrefabs[Random.Range(0, longPrefabs.Length)],
                    pos, Quaternion.identity, longObstacles);
            }
        }
    }

    private float PickAFreePos(Dictionary<float, bool> positions)
    {
        List<float> availablePositions = new List<float>();

        foreach (var entry in positions)
        {
            if (!entry.Value)
                availablePositions.Add(entry.Key);
        }

        if (availablePositions.Count > 0)
            return availablePositions[Random.Range(0, availablePositions.Count)];

        return float.NaN;
    }
}
