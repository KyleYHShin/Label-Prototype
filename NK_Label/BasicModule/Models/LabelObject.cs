namespace BasicModule.Models
{
    public class LabelObject : BasicObject
    {
        #region Default Properties

        private int _selectedPrinter;
        public int SelectedPrinter
        {
            get { return _selectedPrinter; }
            set
            {
                _selectedPrinter = value;
                OnPropertyChanged();
            }
        }

        private int _selectedDpi;
        public int SelectedDpi
        {
            get { return _selectedDpi; }
            set
            {
                _selectedDpi = value;
                OnPropertyChanged();
            }
        }

        #endregion //Default Properties

        #region Grid Properties

        private double _viewSize = 1;
        public double ViewSize
        {
            get { return _viewSize; }
            set
            {
                if (value > 0)
                {
                    _viewSize = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private double _radiusX = 20;
        public double RadiusX
        {
            get { return _radiusX; }
            set
            {
                if (value >= 0)
                {
                    _radiusX = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _radiusY = 20;
        public double RadiusY
        {
            get { return _radiusY; }
            set
            {
                if (value >= 0)
                {
                    _radiusY = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion //Grid Properties
    }
}
