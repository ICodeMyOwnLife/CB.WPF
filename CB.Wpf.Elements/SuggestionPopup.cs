using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using CB.Xaml.AttachedProperties;


namespace CB.Wpf.Elements
{
    public class SuggestionPopup
    {
        #region Fields
        private ListBox _listBox;
        private Popup _popup;
        #endregion


        #region  Constructors & Destructor


        #region Constructors
        public SuggestionPopup()
        {
            InitilizeComponents();
        }
        #endregion


        #endregion


        #region  Properties & Indexers
        public bool IsOpen => _popup.IsOpen;

        public double MaxHeight
        {
            get { return _popup.MaxHeight; }
            set { _popup.MaxHeight = value; }
        }

        public double MaxWidth
        {
            get { return _popup.MaxWidth; }
            set { _popup.MaxWidth = value; }
        }

        public double MinHeight
        {
            get { return _popup.MinHeight; }
            set { _popup.MinHeight = value; }
        }

        public double MinWidth
        {
            get { return _popup.MinWidth; }
            set { _popup.MinWidth = value; }
        }

        public int SelectedIndex
        {
            get { return _listBox.SelectedIndex; }
            set { _listBox.SelectedIndex = value; }
        }

        public object SelectedItem
        {
            get { return _listBox.SelectedItem; }
            set { _listBox.SelectedItem = value; }
        }

        public IEnumerable SuggestionItems
        {
            get { return _listBox.ItemsSource; }
            set { _listBox.ItemsSource = value; }
        }

        public UIElement Target
        {
            get { return _popup.PlacementTarget; }
            set { _popup.PlacementTarget = value; }
        }
        #endregion


        #region Methods
        public void Hide()
        {
            _popup.IsOpen = false;
        }

        public void SelectNextItem()
        {
            var currentIndex = _listBox.SelectedIndex;
            var itemCount = _listBox.Items.Count;

            if (itemCount == 0)
            {
                return;
            }

            var newIndex = currentIndex < itemCount - 1 ? currentIndex + 1 : 0;
            _listBox.SelectedIndex = newIndex;
        }

        public void SelectPreviosItem()
        {
            var currentIndex = _listBox.SelectedIndex;
            var itemCount = _listBox.Items.Count;

            if (itemCount == 0)
            {
                return;
            }

            var newIndex = currentIndex == 0 ? itemCount - 1 : currentIndex - 1;
            _listBox.SelectedIndex = newIndex;
        }

        public void Show(Point location)
        {
            var placementRect = new Rect(location, Size.Empty);
            _popup.PlacementRectangle = placementRect;
            _popup.IsOpen = true;
        }
        #endregion


        #region Implementation
        private void InitializeListBox()
        {
            _listBox = new ListBox();
            _listBox.SetValue(ItemsControlServices.InactiveSelectionColorProperty, Colors.Khaki);
            _listBox.SelectionChanged += (s, e) => { _listBox.ScrollIntoView(_listBox.SelectedItem); };
            /*var scrollBehavior = new ScrollToSelectedItemBehavior();
            Interaction.GetBehaviors(_listBox).Add(scrollBehavior);*/
        }

        private void InitializePopup()
        {
            _popup = new Popup
            {
                Child = _listBox,
                MaxHeight = 200,
                MaxWidth = 150,
                MinHeight = 50,
                MinWidth = 100,
                Placement = PlacementMode.Relative,
                StaysOpen = true
            };
        }

        private void InitilizeComponents()
        {
            InitializeListBox();
            InitializePopup();
        }
        #endregion
    }
}