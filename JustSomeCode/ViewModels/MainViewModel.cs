using JustSomeCode.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace JustSomeCode.ViewModels
{
    public class MainViewModel : ObeservableObject 
    {
        private System.Windows.Media.Color _color;
        ObservableCollection<Polygon> _polygon;

        public SceneViewModel SceneViewModel { get; private set; }
        public Size VisibleSize { get; set; }
        public MainViewModel() 
        {
            Color = (Color)ColorConverter.ConvertFromString("white");
            var scene = new Scene();
            scene.AddNewLayer();
            SceneViewModel = new SceneViewModel(scene);

        }
        public System.Windows.Media.Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }
        ObservableCollection<Polygon> Polygon
        {
            get
            {
                return _polygon;
            }
            set
            {
                _polygon = value;
                OnPropertyChanged("Shapes");
            }
        }



     
        
    }
}
