using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZoneController2 : MonoBehaviour
{
    private GameObject storedObject; // Alan i�inde saklanan obje
    // Bu scriptin ba�l� oldu�u objeye �arpan di�er objeyi tan�ml�yoruz
    private void OnTriggerEnter(Collider other)
    {
        if (storedObject == null)
        {
            // Obje alana girer ve kaydedilir
            storedObject = other.gameObject;
            Debug.Log($"{storedObject.name} alan i�erisine yerle�ti.");
            // E�er objede bir Rigidbody varsa yer �ekimini kapat�yoruz
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false; // Yer�ekimini devre d��� b�rak
                rb.isKinematic = true; // Fizik motorunu devre d��� b�rak
            }

            Collider col = storedObject.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false; // �arp��malar� etkisiz hale getir
            }

            // Objeyi 4 birim yukar�ya ta��yoruz
            Vector3 targetPosition = gameObject.transform.position + Vector3.up * 3f;
            StartCoroutine(MoveAndShrink(other.gameObject, targetPosition));
        }
        else
        {
            // E�er alan doluysa yeni gelen objeyi f�rlat
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = (other.transform.position - transform.position).normalized;
                if (forceDirection.magnitude < 0.5f) // 0.1 de�erini ihtiyac�n�za g�re ayarlay�n
                {
                    Debug.Log("D���K");
                    forceDirection = Vector3.back.normalized; // Varsay�lan y�n yukar�
                }
                rb.AddForce(forceDirection * 1000f); // Kuvvet miktar�n� istedi�iniz gibi ayarlayabilirsiniz
                Debug.Log($"{other.gameObject.name} alan dolu oldu�u i�in f�rlat�ld�.");
            }
        }
    }

    private IEnumerator MoveAndShrink(GameObject obj, Vector3 targetPosition)
    {
        // Objeyi hedef pozisyona do�ru hareket ettiriyoruz
        float moveSpeed = 2f; // H�z ayar�
        while (obj != null && Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
        {
            if (obj != null) // Nesne yok mu diye kontrol ediyoruz
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            yield return null; // Bir frame bekle
        }

        if (obj == null) yield break; // Objeye referans kaybolmu�sa coroutine'i sonland�r
    }
}
