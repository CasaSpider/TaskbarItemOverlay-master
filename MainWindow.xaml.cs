using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace WpfApplication1
{

    public partial class MainWindow : Window
    {
        public static List<string> PhaseList = new List<string>() { "T", "A", "P" };
        public static string Phase = string.Empty;
        public static string HostfilePhase = string.Empty;

        public static string HostfileDir = ConfigurationManager.AppSettings["HostfileDir"];
        public static string Program2Run = ConfigurationManager.AppSettings["Program2Run"];       

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Title = "GIS Environment Selector";

                GetHostfilePhase();

                cbx_phase.ItemsSource = PhaseList;
                cbx_phase.SelectedIndex = GetHostfileIndex();

                var viewModel = new ViewModel();
                
                DataContext = viewModel;
                Phase = HostfilePhase;
                viewModel.Phase = Phase;
                viewModel.Phase = cbx_phase.Text;

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetHostfileIndex()
        {
            for (int i = 0; i < PhaseList.Count; i++)
            {
                if (PhaseList[i] == HostfilePhase)
                    return i;
            }
            throw new Exception($"GetHostfileIndex: No such phase in Hosts file [{HostfilePhase}]");
        }

        private void GetHostfilePhase()
        {
            HostfilePhase = "NOTFOUND";
            string hostfile = $@"{HostfileDir}\hosts";
            var textArr = File.ReadAllLines(hostfile);
            foreach (var line in textArr)
            {
                if (line.Contains("dbserver"))
                {
                    if (line.Contains("TEST"))
                        HostfilePhase = "T";
                    else if (line.Contains("ACCEPTATIE"))
                        HostfilePhase = "A";
                    else if (line.Contains("PRODUCTIE"))
                        HostfilePhase = "P";
                    else
                        throw new Exception($"GetHostfilePhase: No valid phase found in [{hostfile}]");
                    break;
                }
            }
        }

        private void LogSomething()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var log = new Logger(userName, HostfilePhase, Phase);
            log.Write();
        }

        private void CopyTheFile()
        {
            if (Phase == string.Empty)
                Phase = "X";
            string from = $@"{HostfileDir}\hosts_{Phase}";
            string to = $@"{HostfileDir}\hosts";
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
            try
            {
                CopyTheFile();
                LogSomething();
                StartTheProgram();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}