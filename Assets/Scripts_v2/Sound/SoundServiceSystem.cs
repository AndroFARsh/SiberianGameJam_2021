using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace UnderwaterCats
{
    public class SoundServiceSystem: SoundService, IEcsRunSystem
    {
        private EcsFilter<Ref<GameObject>, Ref<Sounds.GameSound[]>>.Exclude<Ref<Dictionary<Sound, AudioSource>>> initFilder;
        private EcsFilter<Ref<Dictionary<Sound, AudioSource>>> filder;
         
        public void Run()
        {
            foreach (var index in initFilder)
            {
                var entity = initFilder.GetEntity(index);
                var gameObject = initFilder.Get1(index).value;
                var sounds = initFilder.Get2(index).value;

                var sources = new Dictionary<Sound, AudioSource>(sounds.Length);
                for (var i=0; i<sounds.Length; ++i)
                {
                    var sound = sounds[i];
                    var source = gameObject.AddComponent<AudioSource>();
                    
                    source.clip = sound.clip;
                    source.volume = sound.volume;
                    source.pitch = sound.pitch;
                    source.loop = sound.loop;

                    sources[sound.sound] = source;
                }
                entity.Replace(new Ref<Dictionary<Sound, AudioSource>>(sources));
                
                PlaySound(Sound.Level);
            }
        }

        public void PlaySound(Sound sound) {
            var audioSource = GetAudioSource(sound);
            if (audioSource != null) {
                audioSource.Play();
            }
        }

        public void StopSound(Sound sound) {
            var audioSource = GetAudioSource(sound);
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        private AudioSource GetAudioSource(Sound sound)
        {
            var sounds = filder.GetEntitiesCount() > 0 
                ? filder.Get1(0).value 
                : null;

            var soundSource = sounds?[sound];
            if (soundSource == null)
            {
                Debug.LogError("Sound " + sound + " not found!"); 
                return null;  
            }
            
            return soundSource;
        }
    }
}