namespace Gameplay
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class SoundManager : MonoBehaviour
    {
        [InfoBox("AudioSources")]
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        
        [InfoBox("AudioClips")]
        [SerializeField] private AudioClip bgMusic;
        [SerializeField] private AudioClip slotClickAudioClip;
        [SerializeField] private AudioClip buyCultistAudioClip;
        [SerializeField] private AudioClip buyTrapAudioClip;
        [SerializeField] private List<AudioClip> swordTrapAudioClips;
        [SerializeField] private List<AudioClip> maceTrapAudioClips;
        [SerializeField] private List<AudioClip> bootTrapAudioClips;
        [SerializeField] private AudioClip lifeDrainAudioClip;
        [SerializeField] private AudioClip bodyHitFloorAudioClip;
        [SerializeField] private AudioClip flipperClickAudioClip;

        [InfoBox("Pitch range")]
        private Vector2 _minMaxPitch = new(1, 3);

        public void PlayMusic()
        {
            musicAudioSource.clip = bgMusic;
            musicAudioSource.Play();
        }

        public void PlaySfx(SfxType sfxType)
        {
            var randomPitch = Random.Range(_minMaxPitch.x, _minMaxPitch.y);
            sfxAudioSource.pitch = randomPitch;
            
            switch (sfxType)
            {
                case SfxType.SlotClick:
                    sfxAudioSource.PlayOneShot(slotClickAudioClip);
                    break;
                case SfxType.BuyCultist:
                    sfxAudioSource.PlayOneShot(buyCultistAudioClip);
                    break;
                case SfxType.BuyTrap:
                    sfxAudioSource.PlayOneShot(buyTrapAudioClip);
                    break;
                case SfxType.SwordTrapDmg:
                    var randomSwordSoundFromList = GetRandomSoundOfType(sfxType);
                    sfxAudioSource.PlayOneShot(randomSwordSoundFromList);
                    break;
                case SfxType.MaceTrapDmg:
                    var randomMaceSoundFromList = GetRandomSoundOfType(sfxType);
                    sfxAudioSource.PlayOneShot(randomMaceSoundFromList);
                    break;
                case SfxType.BootTrapDmg:
                    var randomBootSoundFromList = GetRandomSoundOfType(sfxType);
                    sfxAudioSource.PlayOneShot(randomBootSoundFromList);
                    break;
                case SfxType.LifeDrain:
                    sfxAudioSource.PlayOneShot(lifeDrainAudioClip);
                    break;
                case SfxType.BodyHitFloor:
                    sfxAudioSource.PlayOneShot(bodyHitFloorAudioClip);
                    break;
                case SfxType.FlipperClick:
                    sfxAudioSource.PlayOneShot(flipperClickAudioClip);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sfxType), sfxType, null);
            }
        }

        private AudioClip GetRandomSoundOfType(SfxType sfxType)
        {
            int randomIndex;
            switch (sfxType)
            {
                case SfxType.SwordTrapDmg:
                    randomIndex = Random.Range(0, swordTrapAudioClips.Count);
                    return swordTrapAudioClips[randomIndex];
                case SfxType.MaceTrapDmg:
                    randomIndex = Random.Range(0, maceTrapAudioClips.Count);
                    return maceTrapAudioClips[randomIndex];
                case SfxType.BootTrapDmg:
                    randomIndex = Random.Range(0, bootTrapAudioClips.Count);
                    return bootTrapAudioClips[randomIndex];
            }

            return null;
        }
    }
    
    public enum SfxType
    {
        SlotClick,
        BuyCultist,
        BuyTrap,
        SwordTrapDmg,
        MaceTrapDmg,
        BootTrapDmg,
        LifeDrain,
        BodyHitFloor,
        FlipperClick
    
    }
}