using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASIP.Shared;
using DevDirectInput.Devices.Touchpads;

namespace ASIP.Parsers.COTLTracker
{
    public class SongReplayBuilder : ISongReplayBuilder
    {
        private List<MusicalNote> _notes = new List<MusicalNote>();
        private readonly NoteCoordinates _noteCoords;
        private readonly ITouchpad _touchpad;

        public float TickRate { get; private set; }

        public SongReplayBuilder(NoteCoordinates noteCoords, ITouchpad touchpad)
        {
            _noteCoords = noteCoords;
            _touchpad = touchpad;
            TickRate = 10;
        }

        public string GetName()
        {
            return "";
        }

        public string GetAuthor()
        {
            return "";
        }

        public string GetAbout()
        {
            return "Convert from COTLTracker by ASIP";
        }

        public ITouchpad BuildTouchpad(uint firstNNotes = 0)
        {
            for (var i = 0; i < _notes.Count; i++)
            {
                var musicalNote = _notes[i];
                if (musicalNote.Type == EMusicalNoteType.Delay)
                    continue;

                var notePos = _noteCoords.CoordsByNote[musicalNote.Type][musicalNote.Octave];
                _touchpad.Tap(notePos, (i + 1) * 2 - 1);

                if (firstNNotes != 0 && i > firstNNotes)
                    break;
            }

            return _touchpad;
        }

        public void NormalizeOctaves()
        {
            _notes = SongsHelper.NormalizeOctaves(_notes);
        }

        public void Parse(string scriptPath)
        {
            var lines = File.ReadAllLines(scriptPath);
            Parse(lines);
        }

        public void Parse(string[] scriptLines)
        {
            var notesList = new List<MusicalNote>();
            foreach (var line in scriptLines)
            {
                if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                    continue;

                var strNotes = line.Split(" ");
                foreach (var strNote in strNotes)
                {
                    if (strNote.StartsWith("#") || string.IsNullOrWhiteSpace(strNote))
                        continue;
                    if (strNote.StartsWith("-"))
                    {
                        notesList.AddRange(Enumerable.Repeat(new MusicalNote(EMusicalNoteType.Delay, 0),
                            strNote.Length));
                    }
                    else
                    {
                        notesList.Add(new MusicalNote(Enum.Parse<EMusicalNoteType>(strNote[0].ToString()),
                            byte.Parse(strNote[1].ToString())));
                    }
                }
            }

            _notes = notesList;
        }
    }
}