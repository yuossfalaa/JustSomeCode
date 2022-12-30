using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using MVVM;
using JustSomeCode.Models;
using System.Xml.Linq;

namespace JustSomeCode.ViewModels
{
    public class SceneViewModel:ViewModelBase
    {
        #region Private Variables
        private Scene _scene;
        #endregion
       
        #region commands
        public ICommand ModeChangeToDDALinePaintingCommand { get; private set; }
        public ICommand ModeChangeToBresenhamLinePaintingCommand { get; private set; }
        public ICommand ModeChangeToMoveCommand { get; private set; }
        public ICommand ModeChangeToCircleCommand { get; private set; }
        public ICommand ModeChangeToEllipseCommand { get; private set; }
        public ICommand ModeChangeToRectangleCommand { get; private set; }
        public ICommand ModeChangeToDrawCommand { get; private set; }
        public ICommand ModeChangeToEraseCommand { get; private set; }
        public ICommand RemoveLayerCommand { get; private set; }

        #endregion

        #region public properties
        public enum ModeValues
        {
            DDALine,
            BresenhamLine,
            Circle,
            Ellipse,
            Rectangle,
            Draw,
            Erase,
            Move

        }
        public ObservableCollection<LayerViewModel> Layers { get; private set; }
        public int SelectedLayerIndex
        {
            get { return Scene.SelectedLayerIndex; }
            set
            { 
                if (value == -1)
                    return;
                Scene.SelectedLayerIndex = value;
                RaisePropertyChanged("SelectedLayerIndex");
                RaisePropertyChanged("SelectedLayerViewModel");
            }
        }
        public LayerViewModel SelectedLayerViewModel
        {
            get
            {
                return Layers[SelectedLayerIndex];
            }
        }

      
        //Gets or sets color for scene brush
        public Color Color
        {
            get { return _scene.Color; }
            set
            {
                Scene.Color = value;
                RaisePropertyChanged("Color");
                RaisePropertyChanged("ColorString");
            }
        }
        /// Gets color in string format
        public string ColorString
        {
            get
            {
                var cc = new ColorConverter();
                return cc.ConvertToString(Color);
            }
        }

        public int Thickness
        {
            get { return Scene.Thickness; }
            set
            {
                Scene.Thickness = value;
                RaisePropertyChanged("Thickness");
            }
        }
        public int Mode
        {
            get { return Scene.Mode; }
            set
            {
                Scene.Mode = value;

                RaisePropertyChanged("Mode");
            }
        }


        public Scene Scene
        {
            get { return _scene; }
            set
            {
                if (value==null)
                    throw new ArgumentNullException("Scene can not be null");
                if (_scene!=null)
                    _scene.Dispose();
                _scene = value;
                _scene.LayersOrderChanged += _scene_LayersOrderChanged;
               
                InvalidateLayerList();
                RaisePropertyChanged("Scene");
            }
        }
        #endregion

        #region Constructor
        public SceneViewModel(Scene scene)
        {
            Layers = new ObservableCollection<LayerViewModel>();
            Scene = scene;
            Thickness = Scene.Thickness;
            Mode = (int)ModeValues.Move;

            ModeChangeToDDALinePaintingCommand = new RelayCommand(prama => ModeChangeToDDALinePainting());
            RemoveLayerCommand = new RelayCommand(param => Scene.RemoveSelectedLayer(), param => Scene.CanRemoveSelectedLayer());
            ModeChangeToMoveCommand = new RelayCommand(prama => ModeChangeToMove());
            ModeChangeToDrawCommand = new RelayCommand(prama => ModeChangeToDraw());
            ModeChangeToCircleCommand = new RelayCommand(prama => ModeChangeToCircle());
            ModeChangeToEllipseCommand = new RelayCommand(prama => ModeChangeToEllipse());
            ModeChangeToEraseCommand = new RelayCommand(prama => ModeChangeToErase());
            ModeChangeToBresenhamLinePaintingCommand = new RelayCommand(prama => ModeChangeToBresenhamLinePainting());
            ModeChangeToRectangleCommand = new RelayCommand(prama => ModeChangeToRectangle());
        }
        #endregion

        #region Private Methods
        private void ModeChangeToRectangle()
        {
            Mode = (int)ModeValues.Rectangle;
            Scene.CanAddLayer = true;

        }
        private void ModeChangeToEllipse()
        {
            Mode = (int)ModeValues.Ellipse;
            Scene.CanAddLayer = true;

        }
        private void ModeChangeToBresenhamLinePainting()
        {
            Mode = (int)ModeValues.BresenhamLine;
            Scene.CanAddLayer = true;

        }
        private void ModeChangeToErase()
        {
            Mode = (int)ModeValues.Erase;
        }
        private void ModeChangeToCircle()
        {
            Mode = (int)ModeValues.Circle;
            Scene.CanAddLayer = true;

        }
        private void ModeChangeToDraw()
        {
            Mode = (int)ModeValues.Draw;
            Scene.CanAddLayer = true;
        }

        private void ModeChangeToDDALinePainting()
        {
            Mode = (int)ModeValues.DDALine;
            Scene.CanAddLayer = true;
        }
        private void ModeChangeToMove()
        {
            Mode = (int)ModeValues.Move;
            Scene.CanAddLayer = true;

        }

        private void _scene_LayersOrderChanged(object sender, EventArgs e)
        {
            InvalidateLayerList();   
        }

        private void InvalidateLayerList()
        {
            Layers.Clear();
            for (var i = 0; i < Scene.Layers.Count; i++)
            {
                Layers.Add(new LayerViewModel(Scene.Layers[i]) { Name = String.Format("Layer {0}", i+1) });
            }

            SelectedLayerIndex = Scene.SelectedLayerIndex;
        }
        #endregion
    }
}
