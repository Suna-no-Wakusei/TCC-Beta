using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAssets : MonoBehaviour
{
    public static SpellAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfSpellUI;
    public Transform pfSpellDescUI;

    public Sprite fireballSprite;
    public Sprite fireSwordSprite;
    public Sprite flameThrowerSprite;
    public Sprite iceShardSprite;
    public Sprite lineFreezingSprite;
    public Sprite areaFreezingSprite;
    public Sprite waterballSprite;
    public Sprite squirtSprite;
    public Sprite wavesControlSprite;
    public Sprite speedSprite;
    public Sprite pushAwaySprite;
    public Sprite tornadoSprite;
    public Sprite stoneCannonSprite;
    public Sprite spikeArmorSprite;
    public Sprite guidedCannonSprite;
    public Sprite zapSprite;
    public Sprite lightningSprite;
    public Sprite bounceLightningSprite;
}
