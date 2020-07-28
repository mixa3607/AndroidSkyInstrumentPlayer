using DevDirectInput.Devices.Options;

namespace ASIP.CLI
{
    public class SmartphoneTpadOptions
    {
        public TouchpadOptions TargetTouchpadOptions { get; set; }
        public DeviceOptions[] TriggerDevicesOptions { get; set; }
    }
}