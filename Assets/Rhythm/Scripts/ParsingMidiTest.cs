using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParsingMidiTest
{


    public sealed class NoteInfo
    {
        public int? ProgramNumber { get; set; }
        public long Time { get; set; }
        public long Length { get; set; }
        public int NoteNumber { get; set; }
    }

    public static IEnumerable<NoteInfo> GetNotesInfo(string filePath, out TempoMap tempoMap)
    {
        var midiFile = MidiFile.Read(filePath);
        Debug.Log("Time div: " + midiFile.TimeDivision);

       tempoMap = midiFile.GetTempoMap();

        var programChanges = new Dictionary<FourBitNumber, Dictionary<long, SevenBitNumber>>();
        foreach (var timedEvent in midiFile.GetTimedEvents())
        {
            var programChangeEvent = timedEvent.Event as ProgramChangeEvent;
            //Debug.Log("Seconds: " + timedEvent.TimeAs<MetricTimeSpan>(tempoMap).Seconds);
            if (programChangeEvent == null)
                continue;

            var channel = programChangeEvent.Channel;

            Dictionary<long, SevenBitNumber> changes;
            if (!programChanges.TryGetValue(channel, out changes))
                programChanges.Add(channel, changes = new Dictionary<long, SevenBitNumber>());

            changes[timedEvent.Time] = programChangeEvent.ProgramNumber;
        }

        // collect notes info

        return midiFile.GetNotes()
                       .Select(n => new NoteInfo
                       {
                           ProgramNumber = GetProgramNumber(n.Channel, n.Time, programChanges),
                           Time = n.Time,
                           Length = n.Length,
                           NoteNumber = n.NoteNumber
                       });
    }

    private static int? GetProgramNumber(FourBitNumber channel, long time, Dictionary<FourBitNumber, Dictionary<long, SevenBitNumber>> programChanges)
    {
        Dictionary<long, SevenBitNumber> changes;
        if (!programChanges.TryGetValue(channel, out changes))
            return null;

        var times = changes.Keys.Where(t => t <= time).ToArray();
        return times.Any()
            ? (int?)changes[times.Max()]
            : null;
    }



    public static void GetTimesAndNotes(string path, out List<long> times, out List<int> allNotes, out List<int> generalNotes)
    {

        generalNotes = new List<int>();
        allNotes = new List<int>();
        times = new List<long>();
        TempoMap tempoMap;
        foreach (NoteInfo n in GetNotesInfo(path, out tempoMap))
        {
            if (!generalNotes.Exists(x => x == n.NoteNumber))
            {
                if (n.ProgramNumber != null)
                    generalNotes.Add(n.NoteNumber);
            }
            
            times.Add(TimeConverter.ConvertTo<MetricTimeSpan>(n.Time, tempoMap).TotalMicroseconds);
            allNotes.Add(n.NoteNumber);
        }
    }


    // Start is called before the first frame update
    //void Start()
    //{
    //    List<int> notenumbers = new List<int>();
    //    foreach(NoteInfo n in GetNotesInfo("Assets/Song.mid"))
    //    {
    //        if(!notenumbers.Exists(x => x == n.NoteNumber))
    //        {
    //            if(n.ProgramNumber != null)
    //                notenumbers.Add(n.NoteNumber);
    //        }
    //    }


    //    foreach(int i in notenumbers)
    //    {
    //        Debug.Log(i);
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
