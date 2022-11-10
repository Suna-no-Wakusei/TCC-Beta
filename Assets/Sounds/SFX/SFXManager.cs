using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource ambient, button, castingEarth, castingFire, castingWater, castingIce, fireImpact, iceImpact, earthImpact, waterImpact, 
        wind, item, punchHit, footstepWood, shortGrass, longGrass, treantHurt, humanHurt, thunder, 
        woodAttack, dialogSound, dialogSound1, dialogSound2, earthStep, swordHit, peopleTalkingAmbient, swordSwing, dropItem, paper, pickupItem, mobHit, magicAmbient, chestOpening, door, dash;

    public static SFXManager instance;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayAmbient()
    {
        ambient.Play();
    }

    public void StopAmbient()
    {
        ambient.Stop();
    }

    public void PlayButton()
    {
        button.Play();
    }

    public void PlayCastingEarth()
    {
        castingEarth.Play();
    }

    public void PlayCastingFire()
    {
        castingFire.Play();
    }

    public void PlayCastingWater()
    {
        castingWater.Play();
    }

    public void PlayCastingIce()
    {
        castingIce.Play();
    }

    public void PlayFireImpact()
    {
        fireImpact.Play();
    }

    public void PlayIceImpact()
    {
        iceImpact.Play();
    }

    public void PlayEarthImpact()
    {
        earthImpact.Play();
    }

    public void PlayWaterImpact()
    {
        waterImpact.Play();
    }

    public void PlayWind()
    {
        wind.Play();
    }

    public void StopWind()
    {
        wind.Stop();
    }

    public void PlayItem()
    {
        item.Play();
    }

    public void PlayPunchHit()
    {
        punchHit.Play();
    }

    public void PlayFootstepWood()
    {
        footstepWood.Play();
    }

    public void StopFootstepWood()
    {
        footstepWood.Stop();
    }

    public void PlayShortGrass()
    {
        shortGrass.Play();
    }

    public void StopShortGrass()
    {
        shortGrass.Stop();
    }

    public void PlayLongGrass()
    {
        longGrass.Play();
    }

    public void StopLongGrass()
    {
        longGrass.Stop();
    }

    public void PlayTreantHurt()
    {
        treantHurt.Play();
    }

    public void PlayHumanHurt()
    {
        humanHurt.Play();
    }

    public void PlayThunder()
    {
        thunder.Play();
    }

    public void PlayWoodAttack()
    {
        woodAttack.Play();
    }

    public void PlayDialogSound()
    {
        if (!dialogSound.isPlaying)
            StartCoroutine(playDialogSound());
    }

    public void PlayDialogSound1()
    {
        if (!dialogSound1.isPlaying)
            StartCoroutine(playDialogSound1());
    }

    public void PlayDialogSound2()
    {
        if (!dialogSound2.isPlaying)
            StartCoroutine(playDialogSound2());
    }

    IEnumerator playDialogSound()
    {
        dialogSound.Play();
        yield return new WaitForSeconds(dialogSound.clip.length);
    }

    IEnumerator playDialogSound1()
    {
        dialogSound1.Play();
        yield return new WaitForSeconds(dialogSound1.clip.length);
    }

    IEnumerator playDialogSound2()
    {
        dialogSound2.Play();
        yield return new WaitForSeconds(dialogSound2.clip.length);
    }

    public void PlayEarthStep()
    {
        earthStep.Play();
    }

    public void StopEarthStep()
    {
        earthStep.Stop();
    }

    public void PlaySwordHit()
    {
        swordHit.Play();
    }

    public void PlayPeopleTalkingAmbient()
    {
        peopleTalkingAmbient.Play();
    }

    public void PlaySwordSwing()
    {
        swordSwing.Play();
    }

    public void PlayDropItem()
    {
        dropItem.Play();
    }

    public void PlayPaper()
    {
        paper.Play();
    }

    public void PlayPickupItem()
    {
        pickupItem.Play();
    }

    public void PlayMobHit()
    {
        mobHit.Play();
    }

    public void PlayMagicAmbient()
    {
        magicAmbient.Play();
    }

    public void StopMagicAmbient()
    {
        magicAmbient.Stop();
    }

    public void PlayChestOpening()
    {
        chestOpening.Play();
    }

    public void PlayDoor()
    {
        door.Play();
    }

    public void PlayDash()
    {
        dash.Play();
    }

    public void Update()
    {
        dialogSound.ignoreListenerPause = true;
        dialogSound1.ignoreListenerPause = true;
        dialogSound2.ignoreListenerPause = true;
        button.ignoreListenerPause = true;
        dropItem.ignoreListenerPause = true;

        if (Time.deltaTime == 0)
            AudioListener.pause = true;
        else
            AudioListener.pause = false;
    }
}
