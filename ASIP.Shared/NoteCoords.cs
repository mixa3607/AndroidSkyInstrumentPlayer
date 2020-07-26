using System;
using System.Collections.Generic;
using System.Linq;

namespace ASIP.Shared
{
    public class NoteCoordinates
    {
        public const int NotesLines = 3;
        public const int NotesPerLine = 5;
        public const int NotesCount = NotesLines * NotesPerLine;

        private readonly NotesPositionOptions _posInfo;

        public IReadOnlyDictionary<EMusicalNoteType, IReadOnlyDictionary<byte, INoteAbsolutePosition>> CoordsByNote
        {
            get;
            private set;
        }
        public INoteAbsolutePosition[] CoordsById { get; private set; }
        public EMusicalNoteType[] NotesOrderList { get; }

        public NoteCoordinates(NotesPositionOptions posInfo) : this(posInfo, new EMusicalNoteType[]
        {
            EMusicalNoteType.C,
            EMusicalNoteType.D,
            EMusicalNoteType.E,
            EMusicalNoteType.F,
            EMusicalNoteType.G,
            EMusicalNoteType.A,
            EMusicalNoteType.B,
        })
        { }

        public NoteCoordinates(NotesPositionOptions posInfo, EMusicalNoteType[] notesOrderList)
        {
            NotesOrderList = notesOrderList;
            _posInfo = posInfo;
        }

        public void Calculate()
        {
            var coordsByNote = new Dictionary<EMusicalNoteType, Dictionary<byte, INoteAbsolutePosition>>(); //[note][octave]
            var coordsById = new INoteAbsolutePosition[NotesCount];
            foreach (var noteType in NotesOrderList)
                coordsByNote[noteType] = new Dictionary<byte, INoteAbsolutePosition>();

            for (int noteId = 0; noteId < NotesCount; noteId++)
            {
                var row = !_posInfo.InverseAxis? noteId % NotesPerLine : noteId / NotesPerLine;
                var line = !_posInfo.InverseAxis ? noteId / NotesPerLine : noteId % NotesPerLine;
                var noteIdxInOrderList = noteId % NotesOrderList.Length;
                var octave = (byte)(noteId / NotesOrderList.Length + 1);

                var pos = new NoteAbsolutePosition(
                    _posInfo.StartX + _posInfo.StepX * row,
                    _posInfo.StartY + _posInfo.StepY * line,
                    new MusicalNote(NotesOrderList[noteIdxInOrderList], octave));
                coordsById[noteId] = pos;
                coordsByNote[NotesOrderList[noteIdxInOrderList]][octave] = pos;
            }
            CoordsById = coordsById;
            CoordsByNote = new Dictionary<EMusicalNoteType, IReadOnlyDictionary<byte, INoteAbsolutePosition>>(
                coordsByNote.Select(x => new KeyValuePair<EMusicalNoteType, IReadOnlyDictionary<byte, INoteAbsolutePosition>>(x.Key, x.Value)));
        }

        public void CheckTrackNotesPositions(IEnumerable<MusicalNote> notes)
        {
            foreach (var musicalNote in notes.Where(musicalNote => musicalNote.Type != EMusicalNoteType.Delay))
            {
                if (CoordsByNote.ContainsKey(musicalNote.Type))
                {
                    if (!CoordsByNote[musicalNote.Type].ContainsKey(musicalNote.Octave))
                    {
                        Console.WriteLine($"Note {musicalNote.Type}{musicalNote.Octave} not found");
                    }
                }
                else
                {
                    Console.WriteLine($"Note {musicalNote.Type} not found");
                }
            }
        }
    }
}