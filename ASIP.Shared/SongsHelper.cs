using System;
using System.Collections.Generic;
using System.Linq;

namespace ASIP.Shared
{
    public static class SongsHelper
    {
        public static List<MusicalNote> NormalizeOctaves(List<MusicalNote> notes)
        {
            var uniq = notes.Where(x => x.Type != EMusicalNoteType.Delay).Select(x => x.Octave).Distinct().ToArray();
            var max = uniq.Max();
            var min = uniq.Min();
            if (max - min > 2)
            {
                throw new FormatException("More than 3 octaves used");
            }

            var diff = (byte)(1 - min);
            notes.ForEach(x => x.Octave += diff);
            return notes;
        }
    }
}