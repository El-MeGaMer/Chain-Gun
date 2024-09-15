using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class chainManager : MonoBehaviour
{
    public static chainManager chainInstance;
    Vector3 lastCoord;

    bool first = true;

    [SerializeField] GameObject chainLink;
    [SerializeField] float linkLenght = 1;

    void Awake(){
        chainInstance = this;
    }

    public void makeChain(Vector2 coords){
        if(first){
            first = false;
            lastCoord = coords;
            return;
        }

        int numberOfLinks = Mathf.CeilToInt(Vector3.Distance(coords, lastCoord) / linkLenght);

        Vector3 chainDirection = new Vector3(coords.x-lastCoord.x, coords.y-lastCoord.y,0).normalized;

        for(int i = 0; i<numberOfLinks;i++) {
            Instantiate(chainLink, lastCoord+(chainDirection*linkLenght*i), Quaternion.LookRotation(transform.forward, chainDirection));

        }
        lastCoord = coords;
    }
}
