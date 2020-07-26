using System.IO;
using DevDirectInput.Devices.Touchpads.Configurable;
using Newtonsoft.Json;
using PowerArgs;

namespace ASIP.CLI
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class ASIPOptions
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgShortcut("-v"), ArgShortcut("--verbose"), ArgDescription("Print extended info")]
        public bool Verbose { get; set; }

        [ArgRequired, ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("-e"), ArgShortcut("--events"),
         ArgExistingFile, ArgDescription("Path to json with touchpad events")]
        public TouchpadConfiguration TouchpadConfig { get; set; }

        [ArgRequired, ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("-c"), ArgShortcut("--config"),
         ArgExistingFile, ArgDescription("Path to json with smartphone and notes positions configuration")]
        public SmartphoneOptions SmartphoneConfig { get; set; }

        [ArgRequired, ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("-i"), ArgShortcut("--input"),
         ArgExistingFile, ArgDescription("Path to file that be processed")]
        public string InFilePath { get; set; }

        [ArgRequired, ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("-o"), ArgShortcut("--output"),
         ArgDescription("Path to output file")]
        public string OutFilePath { get; set; }

        [ArgRequired, ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("-t"), ArgShortcut("--tracker"),
         ArgDescription("Tracker type")]
        public ETrackerType TrackerType { get; set; }

        [ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("-n"), ArgShortcut("--name"),
         ArgDescription("Song name override")]
        public string SongName { get; set; }

        [ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("-a"), ArgShortcut("--author"),
         ArgDescription("Song author override")]
        public string SongAuthor { get; set; }

        [ArgShortcut(ArgShortcutPolicy.ShortcutsOnly), ArgShortcut("--about"),
         ArgDescription("Song about override")]
        public string SongAbout { get; set; }

        [ArgReviver]
        public static TouchpadConfiguration ReviveTouchpadConfiguration(string key, string path)
        {
            var jsonConfig = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<TouchpadConfiguration>(jsonConfig);
        }

        [ArgReviver]
        public static SmartphoneOptions ReviveSmartphoneOptions(string key, string path)
        {
            var jsonConfig = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SmartphoneOptions>(jsonConfig);
        }
    }
}