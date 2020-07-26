namespace ASIP.Shared
{
    public struct MusicalNote
    {
        public EMusicalNoteType Type;
        public byte Octave;

        public MusicalNote(EMusicalNoteType type, byte octave)
        {
            Type = type;
            Octave = octave;
        }

        public override string ToString()
        {
            return Type == EMusicalNoteType.Delay ? $"{Type}" : $"{Type}{Octave}";
        }
    }
}