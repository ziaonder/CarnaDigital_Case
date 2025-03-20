using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private Transform shortObstacles, longObstacles;
    [SerializeField] private Transform shurikens, cleavers, pickaxes;
    [SerializeField] private GameObject[] shortPrefabs, longPrefabs;
    [SerializeField] private GameObject shuriken, cleaver, pickaxe;
    [SerializeField] private Transform player;
    private Vector3 lastPlatformZPos;
    private bool isWeaponGenerating = false;
    private Vector3 shurikenPos = new Vector3(5f, 1f, 10f);
    private Vector3 cleaverPos = new Vector3(5f, 1.5f, 10f);
    private Vector3 pickaxePos = new Vector3(0f, 5f, 10f);
    private Transform[] parentsWeapon;
    private GameObject[] prefabsWeapon;
    private Vector3[] positionsWeapon;

    private void OnEnable()
    {
        PlatformController.OnPlatformMoved += GenerateObstacles;
        UIManager.OnRestart += Restart;
    }

    private void OnDisable()
    {
        PlatformController.OnPlatformMoved -= GenerateObstacles;
        UIManager.OnRestart -= Restart;
    }

    private void Start()
    {
        parentsWeapon = new Transform[] { shurikens, cleavers, pickaxes };
        prefabsWeapon = new GameObject[] { shuriken, cleaver, pickaxe };
        positionsWeapon = new Vector3[] { shurikenPos, cleaverPos, pickaxePos };
    }

    private void Update()
    {
        // Weapons start appearing after 15 seconds.
        if (Timer.Instance.TimeElapsed > 15 && !isWeaponGenerating)
        {
            isWeaponGenerating = true;
            StartCoroutine(GenerateWeapon());
        }
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

            Vector3 pos = new Vector3(chosenX, 0.1f, platformZPos);

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

        lastPlatformZPos = new Vector3(0f, 0f, platformZPos);
    }

    // Weapons are different from obstacles. Obstacles are static and do not move. 
    // Obstacles occupy constant space. Weapons, however, are dynamic.
    // Weapons are always generated related to the last obstacles' z position.
    private IEnumerator GenerateWeapon()
    {
        while(GameManager.Instance.StateProperty == GameManager.State.Play)
        {
            int index = Random.Range(0, 3);
            ActivateOrInstantiateWeapon(parentsWeapon[index], 
                prefabsWeapon[index], positionsWeapon[index]);

            yield return new WaitForSeconds(5f);
        }
    }

    private void ActivateOrInstantiateWeapon(Transform parent, GameObject prefab, Vector3 position)
    {
        // This is to change sides. 
        if(parent.name == "Cleavers")
        {
            int value = Random.Range(0, 1);
            value = value == 0 ? -1 : 1;
            position = new Vector3(position.x * value, position.y, position.z);
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            if (!parent.GetChild(i).gameObject.activeSelf)
            {
                parent.GetChild(i).position = position + lastPlatformZPos;
                parent.GetChild(i).gameObject.SetActive(true);
                return;
            }
        }

        Instantiate(prefab, position + lastPlatformZPos, Quaternion.identity, parent);
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

    private void Restart()
    {
        isWeaponGenerating = false;
    }
}
