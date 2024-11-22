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

    private Vector3 previousPosition;  // Önceki frame'deki pozisyon
    private Vector3 currentPosition;   // Þimdiki frame'deki pozisyon

    public float throwForce = 10f;  // Fýrlatma kuvveti

    void Start()
    {
        mainCamera = Camera.main; // Ana kamerayý alýyoruz
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol týklama
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Fare konumundan bir ray oluþturuyoruz

            if (Physics.Raycast(ray, out hit)) // Ray'in bir objeye çarpýp çarpmadýðýný kontrol ediyoruz
            {
                if (hit.collider.CompareTag("Movable")) // Yalnýzca "Movable" etiketine sahip objeleri seç
                {
                    selectedObject = hit.collider.gameObject;
                    offset = selectedObject.transform.position - hit.point + Vector3.up * 2f; // Objeyi hareket ettirmek için ofseti hesaplýyoruz
                    isDragging = true; // Objeyi sürüklemeye baþlýyoruz

                    // Objeyi ilk seçtiðimizde önceki pozisyonu da kaydediyoruz
                    previousPosition = selectedObject.transform.position;
                }
            }
        }

        if (isDragging && selectedObject != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPos = ray.GetPoint(20); // Fare pozisyonunu 3D dünyada alýyoruz
            selectedObject.transform.position = worldPos + offset; // Objeyi sürüklüyoruz

            // Objeyi sürüklerken her frame'de þimdiki pozisyonu güncelliyoruz
            currentPosition = selectedObject.transform.position;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            // Objeyi fýrlatýyoruz
            Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Fýrlatma iþlemi için kinematik olmayan durumu ayarlýyoruz

                // Hareket vektörünü hesaplýyoruz (1 frame önceki pozisyon ile þimdiki pozisyon arasýndaki fark)
                //Vector3 movementDirection = currentPosition - previousPosition;

                
                // Bu yön vektörünü kullanarak fýrlatma kuvvetini uyguluyoruz
                //rb.AddForce(movementDirection.normalized * throwForce, ForceMode.VelocityChange);
            }

            // Objeyi sürüklemeyi bitiriyoruz
            isDragging = false;
            selectedObject = null; // Seçili objeyi null yapýyoruz
        }
    }
}

