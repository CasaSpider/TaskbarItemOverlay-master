using System.ComponentModel;
using System.Windows;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;


namespace WpfApplication1
{

    public partial class MainWindow : Window
    {

        public static List<string> PhaseList = new List<string>() { " ", "T", "A", "P" };
        public static string Phase = string.Empty;

        public static string BaseDir = @"D:\Temp\etc";
        public static string Program2Run = @"C:\Windows\System32\calc.exe";

        public MainWindow()
        {
            InitializeComponent();

            cbx_phase.ItemsSource = PhaseList;
            cbx_phase.SelectedIndex = 0;

            var viewModel = new ViewModel();
            viewModel.Phase = cbx_phase.Text;
            DataContext = viewModel;

            Loaded += delegate
            {
                var thread = new Thread(() =>
                {
                    while (true)
                    {
                        viewModel.Phase = Phase;
                    };
                })
                {
                    IsBackground = true
                };
                thread.Start();
            };
        }

        private void CopyTheFile()
        {
            string from = $@"{BaseDir}\hosts_{Phase}";
            string to = $@"{BaseDir}\hosts";
            File.Copy(from, to, true);
        }

        private void StartTheProgram()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = string.Empty;
            start.FileName = Program2Run;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();
                exitCode = proc.ExitCode;
            }
        }

        private void cbx_dropdownclosed(object sender, EventArgs e)
        {
            Phase = cbx_phase.Text;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CopyTheFile();
            StartTheProgram();
        }
    }
}