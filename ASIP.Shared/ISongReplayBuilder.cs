using DevDirectInput.Devices.Touchpads;

namespace ASIP.Shared
{
    public interface ISongReplayBuilder
    {
        float TickRate { get; }
        ITouchpad BuildTouchpad(uint firstNNotes = 0);
        void NormalizeOctaves();
        void Parse(string scriptPath);
        void Parse(string[] scriptLines);

        string GetName();
        string GetAuthor();
        string GetAbout();
    }
}