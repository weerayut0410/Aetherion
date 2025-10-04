using UnityEngine;

public class MultiSpawn : MonoBehaviour
{
    public string namemon;
    public int nummon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 direction = Vector3.right;
        GameObject prefabToSpawn = Resources.Load<GameObject>(namemon);

        for (int i = 0; i < nummon; i++)
        {
            Vector3 spawnPos = transform.position;

            float offsetAmount = 2f;

            if (i == 0) spawnPos -= direction * offsetAmount; 
            else spawnPos += direction * offsetAmount;        

            Quaternion rotation = Quaternion.Euler(0, 180f, 0);
            Instantiate(prefabToSpawn, spawnPos, rotation);
        }
    
        TurnManager.characters = FindObjectsByType<Character>(FindObjectsSortMode.None);

    }


}
