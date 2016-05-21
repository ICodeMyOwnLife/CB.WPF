using CB.Model.Common;
using CB.Model.Prism;


namespace CB.Wpf.UserControls
{
    public class FileTransferProgressViewModel: PrismViewModelBase
    {
        public FileProgressReporter ProgressReporter { get; set; }
    }
}