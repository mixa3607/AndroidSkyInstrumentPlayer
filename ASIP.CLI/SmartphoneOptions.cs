using ASIP.Shared;
using DevDirectInput.Devices.Options;

namespace ASIP.CLI
{
    public class SmartphoneOptions
    {
        public NotesPositionOptions NotesPositionOptions { get; set; }
        public TouchpadOptions TargetTouchpadOptions { get; set; }
        public DeviceOptions[] TriggerDevicesOptions { get; set; }
    }
}