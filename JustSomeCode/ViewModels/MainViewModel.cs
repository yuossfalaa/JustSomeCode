using JustSomeCode.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using MVVM;

namespace JustSomeCode.ViewModels
{
    public class MainViewModel : ViewModelBase 
    {
        //private System.Windows.Media.Color _color;

        public SceneViewModel SceneViewModel { get; private set; }
        public Size VisibleSize { get; set; }
        public MainViewModel() 
        {
            var scene = new Scene();
            scene.AddNewLayer();
            SceneViewModel = new SceneViewModel(scene);

        }
   
        



     
        
    }
}
