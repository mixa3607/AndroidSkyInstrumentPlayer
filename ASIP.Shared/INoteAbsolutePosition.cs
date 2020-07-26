using DevDirectInput.Devices;

namespace ASIP.Shared
{
    public interface INoteAbsolutePosition : IAbsolutePosition
    {
        MusicalNote Note { get; }
    }
}