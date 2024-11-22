using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;
    private Vector3 offset;
    private bool isDragging = false;

    private Vector3 previousPosition;  // �nceki frame'deki pozisyon
    private Vector3 currentPosition;   // �imdiki frame'deki pozisyon

    public float throwForce = 10f;  // F�rlatma kuvveti

    void Start()
    {
        mainCamera = Camera.main; // Ana kameray� al�yoruz
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol t�klama
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Fare konumundan bir ray olu�turuyoruz

            if (Physics.Raycast(ray, out hit)) // Ray'in bir objeye �arp�p �arpmad���n� kontrol ediyoruz
            {
                if (hit.collider.CompareTag("Movable")) // Yaln�zca "Movable" etiketine sahip objeleri se�
                {
                    selectedObject = hit.collider.gameObject;
                    offset = selectedObject.transform.position - hit.point + Vector3.up * 2f; // Objeyi hareket ettirmek i�in ofseti hesapl�yoruz
                    isDragging = true; // Objeyi s�r�klemeye ba�l�yoruz

                    // Objeyi ilk se�ti�imizde �nceki pozisyonu da kaydediyoruz
                    previousPosition = selectedObject.transform.position;
                }
            }
        }

        if (isDragging && selectedObject != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPos = ray.GetPoint(20); // Fare pozisyonunu 3D d�nyada al�yoruz
            selectedObject.transform.position = worldPos + offset; // Objeyi s�r�kl�yoruz

            // Objeyi s�r�klerken her frame'de �imdiki pozisyonu g�ncelliyoruz
            currentPosition = selectedObject.transform.position;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            // Objeyi f�rlat�yoruz
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // F�rlatma i�lemi i�in kinematik olmayan durumu ayarl�yoruz

                // Hareket vekt�r�n� hesapl�yoruz (1 frame �nceki pozisyon ile �imdiki pozisyon aras�ndaki fark)
                //Vector3 movementDirection = currentPosition - previousPosition;

                
                // Bu y�n vekt�r�n� kullanarak f�rlatma kuvvetini uyguluyoruz
                //rb.AddForce(movementDirection.normalized * throwForce, ForceMode.VelocityChange);
            }

            // Objeyi s�r�klemeyi bitiriyoruz
            isDragging = false;
            selectedObject = null; // Se�ili objeyi null yap�yoruz
        }
    }
}

