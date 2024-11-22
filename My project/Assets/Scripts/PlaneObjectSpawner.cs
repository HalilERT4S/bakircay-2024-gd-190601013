using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class PlaneObjectSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] prefabs;
    public int objectQuantity = 20;
    void Start()
    {
        for (int i = 0; i < objectQuantity; i++)
        {
            if (prefabs.Length == 0 || gameObject == null)
            {
                //Debug.LogError("Prefab'lar veya Plane tanýmlanmamýþ!");
                return;
            }
            float planeWidth = gameObject.transform.localScale.x * 10; // Plane geniþliði
            float planeHeight = gameObject.transform.localScale.z * 10; // Plane yüksekliði

            Vector3 planePosition = gameObject.transform.position; // Plane merkez pozisyonu


            // Rastgele bir pozisyon hesapla
            float randomX = Random.Range(planePosition.x - planeWidth / 2, planePosition.x + planeWidth / 2);
            float randomZ = Random.Range(planePosition.z , planePosition.z + planeHeight / 2);

            // Yüksekliði sabit tut (Plane'in y ekseninde pozisyonunu koruyun)
            float yPosition = planePosition.y + 5;

            // Rastgele pozisyon
            Vector3 randomPosition = new Vector3(randomX, yPosition, randomZ);

            // Rastgele bir prefab seç
            int randomIndex = Random.Range(0, prefabs.Length);
            GameObject selectedPrefab = prefabs[randomIndex];

            // Prefab'i oluþtur
            Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
        }

    }

}
