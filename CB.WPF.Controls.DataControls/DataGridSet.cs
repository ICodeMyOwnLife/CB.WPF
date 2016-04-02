using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CB.WPF.Controls.DataControls
{
    [TemplatePart(Name = MAIN_PANEL, Type = typeof(Grid))]
    public class DataGridSet: Control
    {
        #region Fields
        private const string MAIN_PANEL = "MainPanel";
        private Grid _mainPanel;
        #endregion


        #region  Constructors & Destructor
        static DataGridSet()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridSet),
                new FrameworkPropertyMetadata(typeof(DataGridSet)));
        }
        #endregion


        #region Dependency Properties
        public static readonly DependencyProperty DataSetProperty = DependencyProperty.Register(
            nameof(DataSet), typeof(DataSet), typeof(DataGridSet),
            new PropertyMetadata(default(DataSet), OnDataSetChanged));

        public DataSet DataSet
        {
            get { return (DataSet)GetValue(DataSetProperty); }
            set { SetValue(DataSetProperty, value); }
        }
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainPanel = GetTemplateChild(MAIN_PANEL) as Grid;
        }
        #endregion


        #region Implementation
        private void ClearMainPanel()
        {
            _mainPanel.Children.Clear();
            _mainPanel.RowDefinitions.Clear();
        }

        private static GridSplitter CreateHorizontalSplitter(int rowIndex)
        {
            var splitter = new GridSplitter
            {
                Height = 4,
                Margin = new Thickness(0, -2, 0, 0),
                Background = Brushes.LightGray,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetRow(splitter, rowIndex);
            return splitter;
        }

        private static DataGrid GenerateDataGrid(DataTable table, int rowIndex)
        {
            var dataGrid = new DataGrid
            {
                ItemsSource = table.DefaultView,
                Margin = new Thickness(4),
                IsReadOnly = true
            };
            Grid.SetRow(dataGrid, rowIndex);
            return dataGrid;
        }

        private void GenerateRows(DataTableCollection tables)
        {
            for (var i = 0; i < tables.Count; i++)
            {
                _mainPanel.RowDefinitions.Add(new RowDefinition());
                var dataGrid = GenerateDataGrid(tables[i], i);
                _mainPanel.Children.Add(dataGrid);
                if (i != 0) _mainPanel.Children.Add(CreateHorizontalSplitter(i));
            }
        }

        private static void OnDataSetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataGridSet)d).OnDataSetChanged((DataSet)e.OldValue, (DataSet)e.NewValue);
        }

        protected virtual void OnDataSetChanged(DataSet oldValue, DataSet newValue)
        {
            if (_mainPanel == null) return;

            ClearMainPanel();
            if (newValue == null) return;
            GenerateRows(newValue.Tables);
        }
        #endregion
    }
}