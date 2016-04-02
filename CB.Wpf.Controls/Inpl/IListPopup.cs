using System.Collections;
using System.Windows;


namespace CB.Wpf.Controls.Inpl
{
    public interface IListPopup
    {
        #region Properties & Indexers
        double ActualHeight { get; }
        double ActualWidth { get; }
        double FontSize { get; set; }
        bool IsOpen { get; set; }
        IEnumerable ItemsSource { get; set; }
        Point Location { get; set; }
        double Opacity { get; set; }
        UIElement PlacementTarget { get; set; }
        int SelectedIndex { get; set; }
        object SelectedItem { get; set; }
        #endregion


        #region Events
        event ItemClickRoutedEventHandler ItemClick;
        #endregion


        #region Abstract
        void MoveDownOneItem();
        void MoveDownOnePage();
        void MoveToEnd();
        void MoveToHome();
        void MoveUpOneItem();
        void MoveUpOnePage();
        #endregion
    }
}