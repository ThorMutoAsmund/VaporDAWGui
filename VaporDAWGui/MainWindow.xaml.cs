using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VaporDAWGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Env.MainWindow = this;

            this.Closing += (sender, e) => e.Cancel = !ConfirmChangesMade();

            this.fileMenu.SubmenuOpened += (object sender, RoutedEventArgs e) =>
            {
                this.recentFilesMenu.Items.Clear();

                foreach (var recentFilePath in Env.Conf.RecentFiles)
                {
                    var menuItem = new MenuItem()
                    {
                        Header = recentFilePath,
                    };
                    menuItem.Click += (_sender, _e) => OpenRecentProject(recentFilePath); 
                    this.recentFilesMenu.Items.Add(menuItem);
                }
            };

            this.newScriptMenuItem.Click += (sender, e) => CreateNewScript();
            this.importSamplesMenuItem.Click += (sender, e) => ImportSamples();

            Env.Project.Loaded.ValueChanged += loaded =>
            {
                if (!loaded)
                {
                    this.composer.Visibility = Visibility.Hidden;
                    SetTitle(null);
                    this.saveMenu.IsEnabled = false;
                    this.closeMenu.IsEnabled = false;
                    SetTitle(null);
                }
                else
                {
                    this.composer.Visibility = Visibility.Visible;
                    this.saveMenu.IsEnabled = true;
                    this.closeMenu.IsEnabled = true;
                    SetTitle(Path.GetFileNameWithoutExtension(Env.Project.ProjectPath));
                }
            };


            if (Env.Conf.OpenDemoProjectOnLoad)
            {
                var rootPath = Directory.GetParent(Env.Conf.AppPath).FullName;
                rootPath = Directory.GetParent(rootPath).FullName;
                Env.Project.Open(Path.Combine(rootPath, @"Demo projects\Project 01"));
            }
        }

        private void OnNew(object sender, RoutedEventArgs e) => OpenProject("Select a new empty folder");
        private void OnOpen(object sender, RoutedEventArgs e) => OpenProject("Select an existing project folder");
        private void OnClose(object sender, RoutedEventArgs e) => CloseProject();
        private void OnSave(object sender, RoutedEventArgs e) => Env.Project.Save();
        private void OnExit(object sender, RoutedEventArgs e) => ExitApplication();
        private void OnCloseTab(object sender, RoutedEventArgs e) => CloseTab();

        private void CreateNewScript()
        {
            int i = 0;
            string newScriptName;
            var allScripts = Directory.EnumerateFiles(System.IO.Path.Combine(Env.Project.ProjectPath, Env.Conf.ScriptsFolder), "*.js").
            Select(x => Path.GetFileName(x)).ToArray();
            do
            {
                newScriptName = $"Script{++i}.js";
            }
            while (allScripts != null && allScripts.Any(s => s == newScriptName));

            var window = new EditStringWindow()
            {
                Title = "Enter Script Name",
                Value = newScriptName,
                Label = "Script name"
            };
            if (window.ShowDialog() ?? false)
            {
                // Create empty script file
                var fileName = Path.Combine(Env.Project.ProjectPath, Env.Conf.ScriptsFolder, newScriptName);
                using (File.Create(fileName)) { }
            }
        }

        private void ImportSamples()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = true;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var cnt = 0;
                    foreach (var sourceFilePath in dialog.FileNames)
                    {
                        var sampleName = Path.GetFileName(sourceFilePath);
                        var destinationFilePath = Path.Combine(Env.Project.ProjectPath, Env.Conf.SamplesFolder, sampleName);
                        if (File.Exists(destinationFilePath))
                        {
                            MessageBox.Show($"File {sampleName} already exists. Not copied");
                            continue;
                        }
                        try
                        {
                            File.Copy(sourceFilePath, destinationFilePath);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"Error copying file {sampleName}");
                            continue;
                        }
                        cnt++;
                    }
                    MessageBox.Show($"Successfully imported {cnt} file(s)");
                }
            }

        }

        private void SetTitle(string projectName)
        {
            this.Title = $"{(String.IsNullOrEmpty(projectName) ? String.Empty : $"{projectName} - ")}{Env.Conf.ApplicationName}";
        }

        private bool ConfirmChangesMade()
        {
            if (Env.Project.ChangesMade.Value)
            {
                if (MessageBox.Show("Changes have been made. Continue without saving?", "Changes Made", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        private void CloseProject()
        {
            if (!ConfirmChangesMade())
            {
                return;
            }
            Env.Project.Close();
        }

        private void ExitApplication()
        {
            if (!ConfirmChangesMade())
            {
                return;
            }
            Application.Current.Shutdown();
        }

        private void OpenRecentProject(string path)
        {
            if (!ConfirmChangesMade())
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Selected path not found");
                return;
            }
            Env.Project.Open(path);
        }

        private void OpenProject(string description)
        {
            if (!ConfirmChangesMade())
            {
                return;
            }
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = description;
                dialog.SelectedPath = Env.Project.ProjectPath ?? Env.Conf.AppPath;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Env.Project.Open(dialog.SelectedPath);
                }
            }
        }

        private void CloseTab()
        {
            // Check if saved... TBD

            // Close tab
            if (this.tabControl.SelectedItem is ScriptTabItem)
            {
                this.tabControl.Items.RemoveAt(this.tabControl.SelectedIndex);
            }
        }

        public void EditScript(ScriptInfo script)
        {
            // Check if already open
            foreach (var scriptTabItem in this.tabControl.Items.WhereIs<ScriptTabItem>())
            {
                if (scriptTabItem.Script.Path == script.Path)
                {
                    this.tabControl.SelectedIndex = this.tabControl.Items.IndexOf(scriptTabItem);
                    return;
                }
            }

            // Else create new tab item and select it
            this.tabControl.Items.Insert(this.tabControl.Items.Count, new ScriptTabItem(script));
            this.tabControl.SelectedIndex = this.tabControl.Items.Count - 1;
        }

    }
}
