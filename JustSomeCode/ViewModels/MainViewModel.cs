using JustSomeCode.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using MVVM;
using System.Windows.Input;
using Microsoft.Win32;
using System;

namespace JustSomeCode.ViewModels
{
    public class MainViewModel : ViewModelBase 
    {
        #region Private Variables
        private const string ExportFilter = "PNG|*.jpg";
        private const string ExportErrorMessage = "Error occurred on saving to PNG:\r\n{0}";
        #endregion
        public SceneViewModel SceneViewModel { get; private set; }
        public Size VisibleSize { get; set; }
        public MainViewModel() 
        {
            var scene = new Scene();
            scene.AddNewLayer();
            SceneViewModel = new SceneViewModel(scene);
            ExportCommand = new RelayCommand(parma => Export());
            NewCommand = new RelayCommand(parma => New());

        }

        #region Commands
        public ICommand ExportCommand { get; private set; }
        public ICommand NewCommand { get; private set; }

        #endregion

        #region Private Methods
        private void New()
        {
            var scene = new Scene();
            scene.AddNewLayer();
            LoadScene(scene);
        }
        private void LoadScene(Scene scene)
        {
            SceneViewModel.Scene = scene;
        }

        private void Export()
        {
            try
            {
                var sfd = new SaveFileDialog { Filter = ExportFilter, AddExtension = true };
                if (sfd.ShowDialog().Value)
                {
                    SceneViewModel.Scene.Export(sfd.FileName, new System.Drawing.Size((int)VisibleSize.Width, (int)VisibleSize.Height));
                }
            }
            catch (Exception e)
            {
                Error(ExportErrorMessage, e);
            }
        }
        private void Error(string message, Exception e)
        {
            MessageBox.Show(string.Format(message, e.Message));
        }
        #endregion
    }
}
