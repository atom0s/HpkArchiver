using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HpkArchiver
{
    public static class Static
    {
        public static readonly string ErrorFile = PlatformPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)) + PlatformSlash() + "error.log";
        public static readonly string ConfigurationFile = PlatformPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6)) + PlatformSlash() + "hpk_archiver.cfg";
        public static readonly string TempFile = PlatformPath(Environment.GetEnvironmentVariable("TEMP")) + PlatformSlash() + "hpk_archiver.tmp";
        public static readonly string TempZippedFile = PlatformPath(Environment.GetEnvironmentVariable("TEMP")) + PlatformSlash() + "hpk_archiver_zipped.hpk";


        public static readonly byte[] HGO_SRP_SRP_INIT_BAT = new byte[] { 0x5A, 0x4C, 0x49, 0x42, 0x36, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x40, 0x65, 0x63, 0x68, 0x6F, 0x20, 0x6F, 0x66, 0x66, 0x0D, 0x0A, 0x2E, 0x2E, 0x5C, 0x2E, 0x2E, 0x5C, 0x74, 0x6F, 0x6F, 0x6C, 0x73, 0x5C, 0x68, 0x67, 0x6C, 0x5C, 0x68, 0x67, 0x6C, 0x2E, 0x65, 0x78, 0x65, 0x20, 0x73, 0x72, 0x70, 0x2D, 0x69, 0x6E, 0x69, 0x74, 0x2E, 0x6C, 0x75, 0x61, 0x0D, 0x0A, 0x70, 0x61, 0x75, 0x73, 0x65 };

        public static string PlatformPath(string path)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return path.Replace('\\', '/');
                default:
                    return path;
            }
        }
        public static string PlatformPathReverse(string path)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return path;
                default:
                    return path.Replace('/', '\\');
            }
        }
        public static string PlatformPathToUnix(string path)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return path;
                default:
                    return path.Replace('\\', '/');
            }
        }
        public static char PlatformSlash()
        {

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return '/';
                default:
                    return '\\';
            }
        }
        public static string PlatformStripFile(string file)
        {
            return file.Substring(0, file.LastIndexOf(PlatformSlash()));
        }

        public static void SplitOnce(string value, char character, out string a, out string b)
        {
            int index = value.LastIndexOf(character);
            a = value.Substring(0, index);
            b = value.Substring(index + 1);
        }

        public static double GetSeconds(TimeSpan duration)
        {
            return ((duration.Days * 86400) + (duration.Hours * 3600) + (duration.Minutes * 60) + duration.Seconds) + ((double)duration.Milliseconds / 1000);
        }

        private static readonly string[] HPK_FILES = new string[]
        {
            @"Local\voice\English.hpk",
            @"Local\English.hpk",
            @"Packs\boot\persist\Data.hpk",
            @"Packs\boot\persist\Shaders.hpk",
            @"Packs\boot\Assets.hpk",
            @"Packs\boot\lscr.hpk",
            @"Packs\boot\Music.hpk",
            @"Packs\boot\Sounds.hpk",
            @"Packs\boot\UI.hpk",
            @"Packs\dirs\Cliffs.hpk",
            @"Packs\dirs\Decors.hpk",
            @"Packs\dirs\Roads.hpk",
            @"Packs\dirs\Rocks.hpk",
            @"Packs\dirs\Sky.hpk",
            @"Packs\dirs\Structures.hpk",
            @"Packs\dirs\Terrains.hpk",
            @"Packs\dirs\Units.hpk",
            @"Packs\dirs\Vegetation.hpk",
            @"Packs\dirs\Vehicles.hpk",
            @"Packs\Patches\Boot\Data.hpk",
            @"Packs\Patches\Preload\fallback.hpk",
            @"Packs\Patches\binassets.hpk",
            @"Packs\persist\memAssets.hpk",
            @"Packs\Preload\fallback.hpk",
            @"Packs\Preload\prefabs.hpk",
            @"Packs\billboard.hpk",
            @"Packs\binassets.hpk",
            @"Packs\Missions.hpk"
        };
        public static void ExtractAll(string tropico, string destdir)
        {
            HpkFile file = new HpkFile();
            foreach (string path in HPK_FILES)
                file.Extract(new HpkExtractArgs(tropico + Static.PlatformSlash() + Static.PlatformPath(path), Static.PlatformStripFile(destdir + Static.PlatformSlash() + Static.PlatformPath(path))));
        }

        public static void TryDeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try { File.Delete(path); }
                catch { }
            }
        }
    }
}
