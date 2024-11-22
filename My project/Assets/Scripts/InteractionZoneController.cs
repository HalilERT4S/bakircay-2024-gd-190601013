using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZoneController : MonoBehaviour
{

    // Bu scriptin ba�l� oldu�u objeye �arpan di�er objeyi tan�ml�yoruz
    private void OnTriggerEnter(Collider other)
    {
        // �arpan objenin ad�n� konsola yazd�r�yoruz
        Debug.Log($"{other.gameObject.name} trigger alan�na girdi!");


        // E�er objede bir Rigidbody varsa yer �ekimini kapat�yoruz
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Yer�ekimini devre d��� b�rak
            rb.isKinematic = true; // Fizik motorunu devre d��� b�rak
        }

        // Objeyi 4 birim yukar�ya ta��yoruz
        Vector3 targetPosition = gameObject.transform.position + Vector3.up * 3f;
        StartCoroutine(MoveAndShrink(other.gameObject, targetPosition));
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

        // Objeyi hedef pozisyona getirdikten sonra boyutunu k���lt�yoruz
        float shrinkSpeed = 1f; // K���lme h�z�
        Vector3 originalScale = obj.transform.localScale;
        while (obj != null && obj.transform.localScale.x > 0.1f)
        {
            if (obj != null) // Nesne yok mu diye kontrol ediyoruz
            {
                obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            }
            yield return null; // Bir frame bekle
        }

        if (obj != null) // K���ltme i�lemi tamamland�ktan sonra objeyi yok ediyoruz
        {
            Destroy(obj);
        }
    }
}