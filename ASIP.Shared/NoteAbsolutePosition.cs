namespace ASIP.Shared
{
    public readonly struct NoteAbsolutePosition : INoteAbsolutePosition
    {
        public int X { get; }
        public int Y { get; }
        public MusicalNote Note { get; }

        public NoteAbsolutePosition(int x, int y, MusicalNote note)
        {
            X = x;
            Y = y;
            Note = note;
        }

        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}