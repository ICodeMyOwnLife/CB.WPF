using System.Windows;


namespace CB.Wpf.Controls.Inpl
{
    public delegate void ItemClickRoutedEventHandler(object sender, ItemClickEventArgs e);


    public class ItemClickEventArgs : RoutedEventArgs
    {
        #region Properties & Indexers
        public object ClickedContent { get; private set; }
        #endregion


        #region Constructors & Destructors
        public ItemClickEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }

        public ItemClickEventArgs(RoutedEvent routedEvent, object source, object clickedContent)
            : this(routedEvent, source)
        {
            ClickedContent = clickedContent;
        }
        #endregion
    }
}