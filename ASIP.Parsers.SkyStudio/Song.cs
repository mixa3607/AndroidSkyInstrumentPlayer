namespace ASIP.Parsers.SkyStudio
{
    public class Song
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string ArrangedBy { get; set; }
        public string TranscribedBy { get; set; }
        public string Permission { get; set; }
        public int Bpm { get; set; }
        public int BitsPerPage { get; set; }
        public int PitchLevel { get; set; }
        public SongNote[] SongNotes { get; set; }
    }
}