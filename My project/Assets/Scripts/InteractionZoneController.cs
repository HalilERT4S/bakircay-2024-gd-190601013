using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZoneController : MonoBehaviour
{

    // Bu scriptin baðlý olduðu objeye çarpan diðer objeyi tanýmlýyoruz
    private void OnTriggerEnter(Collider other)
    {
        // Çarpan objenin adýný konsola yazdýrýyoruz
        Debug.Log($"{other.gameObject.name} trigger alanýna girdi!");


        // Eðer objede bir Rigidbody varsa yer çekimini kapatýyoruz
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Yerçekimini devre dýþý býrak
            rb.isKinematic = true; // Fizik motorunu devre dýþý býrak
        }

        // Objeyi 4 birim yukarýya taþýyoruz
        Vector3 targetPosition = gameObject.transform.position + Vector3.up * 3f;
        StartCoroutine(MoveAndShrink(other.gameObject, targetPosition));
    }

    private IEnumerator MoveAndShrink(GameObject obj, Vector3 targetPosition)
    {
        // Objeyi hedef pozisyona doðru hareket ettiriyoruz
        float moveSpeed = 2f; // Hýz ayarý
        while (obj != null && Vector3.Distance(obj.transform.position, targetPosition) > 0.1f)
        {
            if (obj != null) // Nesne yok mu diye kontrol ediyoruz
            {
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            yield return null; // Bir frame bekle
        }

        if (obj == null) yield break; // Objeye referans kaybolmuþsa coroutine'i sonlandýr

        // Objeyi hedef pozisyona getirdikten sonra boyutunu küçültüyoruz
        float shrinkSpeed = 1f; // Küçülme hýzý
        Vector3 originalScale = obj.transform.localScale;
        while (obj != null && obj.transform.localScale.x > 0.1f)
        {
            if (obj != null) // Nesne yok mu diye kontrol ediyoruz
            {
                obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            }
            yield return null; // Bir frame bekle
        }

        if (obj != null) // Küçültme iþlemi tamamlandýktan sonra objeyi yok ediyoruz
        {
            Destroy(obj);
        }
    }
}