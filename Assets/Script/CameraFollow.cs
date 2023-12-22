using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
     public Transform player; // Referência ao Transform do personagem que a câmera deve seguir
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera

    public float minX,maxX;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + new Vector3(6,0,-10); // Posição desejada da câmera
            desiredPosition.y = 1.1f;


            // Configurando a posição da câmera com uma transição suave
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
        }
    }
}