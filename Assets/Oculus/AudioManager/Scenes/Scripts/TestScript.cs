using UnityEngine;
using System.Collections;

namespace OVR
{

public class TestScript : MonoBehaviour {

    public SoundFX          test;

	// Use this for initialization
	IEnumerator Start () {
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=test&tl=En-US";
        WWW www = new WWW(url);
        yield return www;

        test.soundClips[0] = www.GetAudioClip(false, true, AudioType.MPEG);
        test.PlaySound();
    }
}

} // namespace OVR
