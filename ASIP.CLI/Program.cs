using System;
using System.IO;
using ASIP.Shared;
using DevDirectInput.Devices;
using DevDirectInput.Devices.Touchpads.Configurable;
using DevDirectInput.Enums;
using DevDirectInput.Replay;
using PowerArgs;

namespace ASIP.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new[] {
            //    "-e", "StaticFiles/Configs/Touchpads/memu.json", 
            //    "-c", "StaticFiles/Configs/Smartphones/memu_ss.json", 
            //    "-i", "StaticFiles/SkyStudioTracker/Example_Canon.C.txt",
            //    "-o", @"./Example_Canon.C.json",
            //    "-t", "skyStudio"
            //};
            var mainOptions = Args.Parse<ASIPOptions>(args);
            if (mainOptions == null)
                return;
            ASIPGlobalOpts.Logging = mainOptions.Verbose;

            var replay = Build(mainOptions);
            File.WriteAllText(mainOptions.OutFilePath, replay.ToJson());
            Console.WriteLine($"Saved to {mainOptions.OutFilePath}");
        }

        static InputReplay Build(ASIPOptions asipOptions)
        {
            Console.Write("Creating touchpad... ");
            var touchpad = new ConfigurableTouchpad(asipOptions.TouchpadConfig, asipOptions.SmartphoneConfig.TargetTouchpadOptions, EDevicePurpose.Target);
            Console.WriteLine("OK");
            Console.Write("Calculating notes coordinates... ");
            var noteCoords = new NoteCoordinates(asipOptions.SmartphoneConfig.NotesPositionOptions);
            noteCoords.Calculate();
            Console.WriteLine("OK");

            ISongReplayBuilder parser = asipOptions.TrackerType switch
            {
                ETrackerType.SkyStudio => new Parsers.SkyStudio.SongReplayBuilder(noteCoords, touchpad),
                ETrackerType.COTL => new Parsers.COTLTracker.SongReplayBuilder(noteCoords, touchpad),
                _ => throw new NotImplementedException()
            };

            Console.Write("Parsing... ");
            parser.Parse(asipOptions.InFilePath);
            parser.NormalizeOctaves();
            Console.WriteLine("OK");
            Console.Write("Building replay... ");
            parser.BuildTouchpad(0);

            var replayBuilder = new InputReplayBuilder(parser.TickRate)
            {
                Name = asipOptions.SongName ?? parser.GetName(),
                Author = asipOptions.SongAuthor ?? parser.GetAuthor(),
                About = asipOptions.SongAbout ?? parser.GetAbout() + " " + "https://asip.arkprojects.space"
            };
            replayBuilder.AddDevice(touchpad);
            foreach (var devicesOptions in asipOptions.SmartphoneConfig.TriggerDevicesOptions)
            {
                replayBuilder.AddDevice(new GenericTriggerDevice(devicesOptions));
            }
            var replay = replayBuilder.Build();
            Console.WriteLine("OK");
            return replay;
        }
    }
}