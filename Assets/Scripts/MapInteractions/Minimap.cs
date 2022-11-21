using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public RectTransform playerInMap;
    public RectTransform minimap;
    public Vector2 maxReal, minReal;
    public bool trackPlayer;

    private float widthFactor, heightFactor;
    private Transform player;

    private void Start()
    {
        widthFactor = minimap.rect.width / (maxReal.x - minReal.x);
        heightFactor = minimap.rect.height / (maxReal.y - minReal.y);
    }

    private void FixedUpdate()
    {
        player = GameManager.instance.hero.transform;
        
        if(trackPlayer) playerInMap.anchoredPosition = new Vector2(player.localPosition.x * widthFactor, player.localPosition.y * heightFactor);
    }

}
