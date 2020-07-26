using System;
using System.IO;
using ASIP.Shared;
using DevDirectInput.Devices.Touchpads;
using Newtonsoft.Json;

namespace ASIP.Parsers.SkyStudio
{
    public class SongReplayBuilder: ISongReplayBuilder
    {
        private const float BitsPerColumn = 20f;
        private const float TimeMultiplier = 3000f;

        private int PageMs;
        private int OneColLen;

        private Song _song;
        private readonly NoteCoordinates _noteCoords;
        private readonly ITouchpad _touchpad;

        public float TickRate { get; private set; }

        public SongReplayBuilder(NoteCoordinates coords, ITouchpad touchpad)
        {
            _noteCoords = coords;
            _touchpad = touchpad;
        }

        public void Parse(string scriptPath)
        {
            try
            {
                var songs = JsonConvert.DeserializeObject<Song[]>(File.ReadAllText(scriptPath));
                if (songs.Length != 1)
                {
                    throw new FormatException($"Read {songs.Length} but expected 1");
                }

                _song = songs[0];
            }
            catch (Exception e)
            {
                _song = JsonConvert.DeserializeObject<Song>(File.ReadAllText(scriptPath));
            }

            PageMs = (int)(_song.BitsPerPage * BitsPerColumn / _song.Bpm * TimeMultiplier);
            OneColLen = (int)(BitsPerColumn * TimeMultiplier / _song.Bpm);
            TickRate = (float)(1000f / OneColLen * 2);
        }

        public void Parse(string[] scriptLines)
        {
            throw new NotImplementedException();
        }

        public ITouchpad BuildTouchpad(uint firstNNotes = 0)
        {
            for (var noteIdx = 0; noteIdx < _song.SongNotes.Length || (noteIdx < firstNNotes && firstNNotes != 0); noteIdx++)
            {
                var songNote = _song.SongNotes[noteIdx];
                var tick = songNote.Time / OneColLen;
                _touchpad.Tap(_noteCoords.CoordsById[songNote.KeyId], (tick + 1) * 2 - 1);
                //Console.WriteLine($"{tick}\t{songNote.KeyId}\t{_noteCoords.CoordsById[songNote.KeyId].Note}");
            }

            return _touchpad;
        }

        public string GetName()
        {
            return _song.Name;
        }

        public string GetAuthor()
        {
            return _song.Author;
        }

        public string GetAbout()
        {
            return "Convert from SkyStudio by ASIP";
        }

        public void NormalizeOctaves()
        {
            
        }
    }
}