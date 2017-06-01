using System;
using System.Windows.Forms;

// ------------------------------------------------------------------
// Wraps System.Windows.Forms.OpenFileDialog to make it present
// a vista-style dialog.
// ------------------------------------------------------------------

namespace TheLongestString
{
    /// <summary>
    /// Wraps System.Windows.Forms.OpenFileDialog to make it present
    /// a vista-style dialog.
    /// </summary>
    public class FolderSelectDialog
    {
        // Wrapped dialog
        private System.Windows.Forms.OpenFileDialog openFileDialog = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FolderSelectDialog()
        {
            openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.Filter = "Folders|\n";
            openFileDialog.AddExtension = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.DereferenceLinks = true;
            openFileDialog.Multiselect = false;
        }

        #region Properties

        /// <summary>
        /// Gets/Sets the initial folder to be selected. A null value selects the current directory.
        /// </summary>
        public string InitialDirectory
        {
            get { return openFileDialog.InitialDirectory; }
            set { openFileDialog.InitialDirectory = value == null || value.Length == 0 ? Environment.CurrentDirectory : value; }
        }

        /// <summary>
        /// Gets/Sets the title to show in the dialog
        /// </summary>
        public string Title
        {
            get { return openFileDialog.Title; }
            set { openFileDialog.Title = value == null ? "Select a folder" : value; }
        }

        /// <summary>
        /// Gets the selected folder
        /// </summary>
        public string SelectedPath
        {
            get { return openFileDialog.FileName; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <returns>True if the user presses OK else false</returns>
        public bool ShowDialog()
        {
            return ShowDialog(IntPtr.Zero);
        }

        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <param name="hWndOwner">Handle of the control to be parent</param>
        /// <returns>True if the user presses OK else false</returns>
        public bool ShowDialog(IntPtr hWndOwner)
        {
            bool flag = false;

            if (Environment.OSVersion.Version.Major >= 6)
            {
                var nativeAssembly = new AssemblyManager("System.Windows.Forms");

                uint num = 0;
                Type typeIFileDialog = nativeAssembly.GetType("FileDialogNative.IFileDialog");
                object dialog = nativeAssembly.InvokeMethod(openFileDialog.GetType(), openFileDialog, "CreateVistaDialog", null);
                nativeAssembly.InvokeMethod(openFileDialog.GetType(), openFileDialog, "OnBeforeVistaDialog", new object[] { dialog });

                uint options = (uint)nativeAssembly.InvokeMethod(typeof(System.Windows.Forms.FileDialog), openFileDialog, "GetOptions", null);
                options |= (uint)nativeAssembly.GetEnum("FileDialogNative.FOS", "FOS_PICKFOLDERS");
                nativeAssembly.InvokeMethod(typeIFileDialog, dialog, "SetOptions", new object[] { options });

                object vistaDialog = nativeAssembly.CreateInstance("FileDialog.VistaDialogEvents", new object[] { openFileDialog });
                object[] parameters = new object[] { vistaDialog, num };
                nativeAssembly.InvokeMethod(typeIFileDialog, dialog, "Advise", parameters);
                num = (uint)parameters[1];
                try
                {
                    int num2 = (int)nativeAssembly.InvokeMethod(typeIFileDialog, dialog, "Show", new object[] { hWndOwner });
                    flag = 0 == num2;
                }
                finally
                {
                    nativeAssembly.InvokeMethod(typeIFileDialog, dialog, "Unadvise", new object[] { num });
                    GC.KeepAlive(vistaDialog);
                }
            }
            else
            {
                var fbd = new FolderBrowserDialog();
                fbd.Description = this.Title;
                fbd.SelectedPath = this.InitialDirectory;
                fbd.ShowNewFolderButton = false;
                if (fbd.ShowDialog(new WindowWrapper(hWndOwner)) != DialogResult.OK) return false;
                openFileDialog.FileName = fbd.SelectedPath;
                flag = true;
            }

            return flag;
        }

        #endregion Methods
    }

    /// <summary>
    /// Creates IWin32Window around an IntPtr
    /// </summary>
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="handle">Handle to wrap</param>
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        /// <summary>
        /// Original ptr
        /// </summary>
        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}