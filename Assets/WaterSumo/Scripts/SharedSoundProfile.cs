using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedSoundProfile : SoundProfileBehaviour
{
    public List<AudioClip> SumoBumps = new List<AudioClip>();
    public List<AudioClip> Stomps = new List<AudioClip>();

    public List<SumoSoundProfile> SumoProfiles = new List<SumoSoundProfile>();
}
