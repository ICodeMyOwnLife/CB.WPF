using System.Diagnostics;
using System.Windows;
using TestCB.WPF.Controls.Views;


namespace TestCB.WPF.Controls
{
    public partial class App
    {
        #region Override
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new TestDialogContentControlWindow();
            window.ShowDialog();
            Debug.WriteLine(window.DialogResult);
        }
        #endregion
    }
}