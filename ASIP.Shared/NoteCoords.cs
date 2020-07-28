using System;
using System.Collections.Generic;
using System.Linq;

namespace ASIP.Shared
{
    public class NoteCoordinates
    {
        //public const int NotesLines = 3;
        //public const int NotesPerLine = 5;
        //public const int NotesCount = NotesLines * NotesPerLine;

        private readonly MusicalInstrumentOptions _options;

        public IReadOnlyDictionary<EMusicalNoteType, IReadOnlyDictionary<byte, INoteAbsolutePosition>> CoordsByNote
        {
            get;
            private set;
        }

        public INoteAbsolutePosition[] CoordsById { get; private set; }
        public EMusicalNoteType[] NotesOrderList { get; }

        public NoteCoordinates(MusicalInstrumentOptions options) : this(options, new EMusicalNoteType[]
        {
            EMusicalNoteType.C,
            EMusicalNoteType.D,
            EMusicalNoteType.E,
            EMusicalNoteType.F,
            EMusicalNoteType.G,
            EMusicalNoteType.A,
            EMusicalNoteType.B,
        })
        {
        }

        public NoteCoordinates(MusicalInstrumentOptions options, EMusicalNoteType[] notesOrderList)
        {
            NotesOrderList = notesOrderList;
            _options = options;
        }

        public void Calculate()
        {
            var coordsByNote =
                new Dictionary<EMusicalNoteType, Dictionary<byte, INoteAbsolutePosition>>(); //[note][octave]
            var coordsById = new INoteAbsolutePosition[_options.NotesCount];
            foreach (var noteType in NotesOrderList)
                coordsByNote[noteType] = new Dictionary<byte, INoteAbsolutePosition>();

            for (int noteId = 0; noteId < _options.NotesCount; noteId++)
            {
                var row = !_options.InverseAxis ? noteId % _options.Columns : noteId / _options.Columns;
                var line = !_options.InverseAxis ? noteId / _options.Columns : noteId % _options.Columns;
                var noteIdxInOrderList = noteId % NotesOrderList.Length;
                var octave = (byte) (noteId / NotesOrderList.Length + 1);

                var pos = new NoteAbsolutePosition(
                    _options.StartX + _options.StepX * row,
                    _options.StartY + _options.StepY * line,
                    new MusicalNote(NotesOrderList[noteIdxInOrderList], octave));
                coordsById[noteId] = pos;
                coordsByNote[NotesOrderList[noteIdxInOrderList]][octave] = pos;
            }

            CoordsById = coordsById;
            CoordsByNote = new Dictionary<EMusicalNoteType, IReadOnlyDictionary<byte, INoteAbsolutePosition>>(
                coordsByNote.Select(x =>
                    new KeyValuePair<EMusicalNoteType, IReadOnlyDictionary<byte, INoteAbsolutePosition>>(x.Key,
                        x.Value)));
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