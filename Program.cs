using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HpkArchiver
{
    static class Program
    {
        private static HpkFile _File;

        #region " Console Commands "
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        [STAThread]
        internal static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                _File = new HpkFile();
                _File.FileSystemCount += _File_FileSystemCount;
                _File.FileSystemEntryProcessed += _File_FileSystemEntryProcessed;
                _File.Completed += _File_Completed;

                bool wait = false;
                bool zip = false;
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())
                    {
                        case "-wait":
                            wait = true;
                            break;
                        case "-zip":
                            zip = true;
                            break;
                        case "-unzip":
                            Console.WriteLine("Unzipping " + args[i + 1] + "...");
                            FileInfo fi = new FileInfo(args[i + 1]);
                            if (fi.Exists)
                            {
                                FragmentedFile file = new FragmentedFile();
                                file.Fragments.Add(new Fragment(0, Convert.ToInt32(fi.Length)));

                                FileStream fs = new FileStream(fi.FullName, FileMode.Open);
                                BinaryReader br = new BinaryReader(fs);
                                int errors = file.ExtractZippedFile(ref br, args[i + 2]);
                                if (errors == 0)
                                    Console.WriteLine("  File unzipped successfully.");
                                else
                                    Console.WriteLine("  " + errors + " caught.  See error.log for details.");
                                br.Close();
                                fs.Close();
                            }
                            else
                                Console.WriteLine("  " + args[i + 1] + " does not exist.");
                            break;
                        case "-extract":
                            Console.WriteLine("Extracting " + args[i + 1] + "...");
                            _File.Extract(new HpkExtractArgs(args[i + 1], args[i + 2]));
                            break;
                        case "-archive":
                            Console.WriteLine("Archiving " + args[i + 1] + "...");
                            _File.Archive(new HpkArchiveArgs(args[i + 1], args[i + 2], zip));
                            break;
                    }
                }
                if (wait)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to close program.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Launching GUI and hiding console...");
                ShowWindow(GetConsoleWindow(), 0);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Main frm = new Main();
                frm.Show();
                frm.BringToFront();
                frm.Activate(); // Puts the cursor in the window.
                Application.Run(frm);
            }
        }

        private static Percentage _Percentage;
        private static void _File_Completed(TimeSpan duration, string description)
        {
            if (_Percentage != null)
            {
                _Percentage.Update(); // Push last update to 100%.
                _Percentage = null; // Clear it so that we don't use this same spot by accident.
            }
            Console.WriteLine();
            Console.WriteLine("  Operation completed in " + duration.ToString() + " seconds.");
        }
        private static void _File_FileSystemEntryProcessed(FragmentType type)
        {
            if (_Percentage != null)
                _Percentage.Update();
        }
        private static void _File_FileSystemCount(int count)
        {
            Console.Write("  " + count + " directories and files found. ");
            _Percentage = new Percentage(Console.CursorLeft, Console.CursorTop, count);
        }
    }
}