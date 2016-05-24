using CB.Model.Common;


namespace CB.Wpf.UserControls
{
    internal class DesignFileTransferProgressViewModel: FileTransferProgressViewModel
    {
        #region  Constructors & Destructor
        public DesignFileTransferProgressViewModel()
        {
            ProgressReporter = new FileProgressReporter
            {
                FileName = @"C:\Program Files\Data.mba",
                FileSize = 1422478364,
                BytesTransferred = 471264324
            };
            ProgressReporter.Start();
        }
        #endregion
    }
}