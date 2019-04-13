using System;
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

            this.newMenu.Click += (sender, e) => OpenProject("Select a new empty folder");
            this.openMenu.Click += (sender, e) => OpenProject("Select an existing project folder");
            this.saveMenu.Click += (sender, e) => Env.Project.Save();
            this.closeMenu.Click += (sender, e) => CloseProject();

            this.exitMenu.Click += (sender, e) => ExitApplication();

            this.Closing += (sender, e) => e.Cancel = !ConfirmChangesMade();

            this.fileMenu.SubmenuOpened += (object sender, RoutedEventArgs e) =>
            {
                this.recentFilesMenu.Items.Clear();

                foreach (var recentFilePath in Env.Config.RecentFiles)
                {
                    var menuItem = new MenuItem()
                    {
                        Header = recentFilePath,
                    };
                    menuItem.Click += (_sender, _e) => OpenRecentProject(recentFilePath); 
                    this.recentFilesMenu.Items.Add(menuItem);
                }
            };

            Env.Project.Loaded.ValueChanged += (sender, e) =>
            {
                if (!e)
                {
                    this.composer.Visibility = Visibility.Hidden;
                    SetTitle(null);
                }
                else
                {
                    this.composer.Visibility = Visibility.Visible;
                }
            };

            Env.Project.ProjectPath.ValueChanged += (sender, e) =>
            {
                SetTitle(System.IO.Path.GetFileNameWithoutExtension(e));
            };

            if (Env.Config.OpenDemoProjectOnLoad)
            {
                var rootPath = System.IO.Directory.GetParent(Env.Config.AppPath).FullName;
                rootPath = System.IO.Directory.GetParent(rootPath).FullName;
                Env.Project.Open(System.IO.Path.Combine(rootPath, @"Demo projects\Project 01"));
            }
        }

        private void SetTitle(string projectName)
        {
            this.Title = $"{(String.IsNullOrEmpty(projectName) ? String.Empty : $"{projectName} - ")}{Env.Config.ApplicationName}";
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
            if (!System.IO.Directory.Exists(path))
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
                dialog.SelectedPath = Env.Project.ProjectPath.Value ?? Env.Config.AppPath;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    if (!System.IO.Directory.Exists(dialog.SelectedPath))
                    {
                        MessageBox.Show("Selected path not found");
                        return;
                    }
                    Env.Project.Open(dialog.SelectedPath);
                }
            }
        }
    }
}
