using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour
{
    BGMManager BGM;
    public int playMusicTrack;
    // Start is called before the first frame update
    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();
        BGM.Play(playMusicTrack);
    }

    // Update is called once per frame
    void Update()
    {/*
        if (this.gameObject.activeSelf == true)
            BGM.Play(playMusicTrack);

        if (this.gameObject.activeSelf == false)
            BGM.Stop();
        */
    }
}
