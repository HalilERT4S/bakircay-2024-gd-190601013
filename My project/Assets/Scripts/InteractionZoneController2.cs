using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZoneController2 : MonoBehaviour
{
    private GameObject storedObject; // Alan içinde saklanan obje
    // Bu scriptin baðlý olduðu objeye çarpan diðer objeyi tanýmlýyoruz
    private void OnTriggerEnter(Collider other)
    {
        if (storedObject == null)
        {
            // Obje alana girer ve kaydedilir
            storedObject = other.gameObject;
            Debug.Log($"{storedObject.name} alan içerisine yerleþti.");
            // Eðer objede bir Rigidbody varsa yer çekimini kapatýyoruz
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false; // Yerçekimini devre dýþý býrak
                rb.isKinematic = true; // Fizik motorunu devre dýþý býrak
            }

            Collider col = storedObject.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false; // Çarpýþmalarý etkisiz hale getir
            }

            // Objeyi 4 birim yukarýya taþýyoruz
            Vector3 targetPosition = gameObject.transform.position + Vector3.up * 3f;
            StartCoroutine(MoveAndShrink(other.gameObject, targetPosition));
        }
        else
        {
            // Eðer alan doluysa yeni gelen objeyi fýrlat
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = (other.transform.position - transform.position).normalized;
                if (forceDirection.magnitude < 0.5f) // 0.1 deðerini ihtiyacýnýza göre ayarlayýn
                {
                    Debug.Log("DÜÞÜK");
                    forceDirection = Vector3.back.normalized; // Varsayýlan yön yukarý
                }
                rb.AddForce(forceDirection * 1000f); // Kuvvet miktarýný istediðiniz gibi ayarlayabilirsiniz
                Debug.Log($"{other.gameObject.name} alan dolu olduðu için fýrlatýldý.");
            }
        }
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
    }
}
