using System;

namespace FolderContentPresenter
{
    public class FolderPresenterEventArgs: EventArgs
    {
        public FolderPresenterEventArgs(string filePath)
        {
            FilePath = filePath;
        }
        public string FilePath { get; set; }
    }
}
