using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Linq;

// Reference: https://discussions.unity.com/t/parse-a-midi-file/173657/2

public sealed class NoteInfo
{
    public int noteNumber;
    public int beatNumber;
    public int channelNumber;
}

public class Midi : MonoBehaviour
{
    [SerializeField]
    private string fileName;
    public List<NoteInfo> noteInfo = new List<NoteInfo>();
    public int numberOfBeats;
    private void Awake()
    {
        MidiFile midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileName);
        TempoMap tempoMap = midiFile.GetTempoMap();
        
        noteInfo = midiFile.GetNotes().Select(n => {
            MusicalTimeSpan musicalTime = n.TimeAs<MusicalTimeSpan>(tempoMap);
            return new NoteInfo { noteNumber = n.NoteNumber, beatNumber = (int)(4 * musicalTime.Numerator / musicalTime.Denominator), channelNumber = n.Channel};
        }).ToList();

        numberOfBeats = noteInfo.Last().beatNumber + 1;
    }

}

