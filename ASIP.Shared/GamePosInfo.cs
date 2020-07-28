namespace ASIP.Shared
{
    public class MusicalInstrumentOptions
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int NotesCount => Columns * Rows;

        public int StartX { get; set; }
        public int StartY { get; set; }
        public int StepX { get; set; }
        public int StepY { get; set; }
        public bool InverseAxis { get; set; }
    }
}