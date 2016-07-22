using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CB.IO.Common;


namespace CB.WPF.MahAppsFileExplorerControls
{
    [TemplatePart(Name = TXT_NAME, Type = typeof(TextBox))]
    public abstract class MahAppsFileSystemEntryControl: Control
    {
        #region Fields
        protected const string TXT_NAME = "PART_txtName";
        protected IFileSystemEntry _fileSystemEntry;
        protected bool _renaming;
        protected TextBox _txtName;
        #endregion


        #region Methods
        public void BeginRename()
        {
            if (_txtName == null || _fileSystemEntry == null) return;

            _txtName.Text = _fileSystemEntry.Name;
            _txtName.Visibility = Visibility.Visible;
            _txtName.SelectAll();
        }

        public void CancelRename()
        {
            if (!_renaming) return;

            _txtName.Visibility = Visibility.Collapsed;
            _renaming = false;
        }

        public async Task EndRenameAsync()
        {
            if (!_renaming) return;

            if (_fileSystemEntry != null && !string.IsNullOrEmpty(_txtName.Text))
                await _fileSystemEntry.RenameAsync(_txtName.Text);
            CancelRename();
        }
        #endregion


        #region Override
        public override void OnApplyTemplate()
        {
            _txtName = GetTemplateChild(TXT_NAME) as TextBox;
            if (_txtName != null)
            {
                _txtName.LostFocus += TxtName_LostFocus;
                _txtName.PreviewKeyDown += TxtName_PreviewKeyDown;
            }
            base.OnApplyTemplate();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    BeginRename();
                    break;

                case Key.Escape:
                    CancelRename();
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                _fileSystemEntry = e.NewValue as IFileSystemEntry;
            }
            base.OnPropertyChanged(e);
        }
        #endregion


        #region Event Handlers
        private async void TxtName_LostFocus(object sender, RoutedEventArgs e)
            => await EndRenameAsync();

        private async void TxtName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) await EndRenameAsync();
        }
        #endregion
    }
}