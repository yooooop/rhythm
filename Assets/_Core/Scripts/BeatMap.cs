using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BeatMap : OnBeatBehaviour
{
    [Serializable]
    public class BeatEntry
    {
        public Vector3Int cell;
        public GameObject prefab;
    }
    
    [Serializable]
    public class BeatList
    {
        public List<BeatEntry> BeatEntries;
    }
    [Serializable]
    public class Attack
    {
        public GameObject prefab;
        public int anticipation;
    }
    public Grid grid;
    public List<BeatList> beatmap;
    public Midi midi;
    public List<Attack> attacks;
    public int gridWidth;

    private int _index = 4;

    protected override void Start()
    {
        base.Start();
        conductor = FindObjectOfType<Conductor>();
        grid = FindObjectOfType<Grid>();
        midi = FindObjectOfType<Midi>();
        InitializeBeatMapFromMidi();
    }

    private void InitializeBeatMapFromMidi()
    {
        for (int i = 0; i < midi.numberOfBeats; i++) 
        {
            beatmap.Add(new BeatList { BeatEntries = new List<BeatEntry>()});
        }
        foreach (var note in midi.noteInfo)
        {
            // Arbitrary conversion from note value of midi to a grid location (60 is middle C)
            int row = (note.noteNumber - 60) / gridWidth;
            int col = (note.noteNumber - 60) % gridWidth;
            Attack attack = attacks[note.channelNumber];
            BeatEntry beat = new BeatEntry { cell = new Vector3Int(col, row, 0), prefab = attack.prefab};
            if (note.beatNumber - attack.anticipation >= 0) 
            {
                beatmap[note.beatNumber - attack.anticipation].BeatEntries.Add(beat);
            }   
        }
    }

    protected override void OnBeat()
    {
        if (_index < beatmap.Count)
        {
            foreach (var beatEntry in beatmap[_index].BeatEntries)
            {
                if (beatEntry.prefab) 
                {
                    Instantiate(beatEntry.prefab, grid.GetCellCenterWorld(beatEntry.cell), Quaternion.identity);
                }
               
            }
        }

        ++_index;
    }
}