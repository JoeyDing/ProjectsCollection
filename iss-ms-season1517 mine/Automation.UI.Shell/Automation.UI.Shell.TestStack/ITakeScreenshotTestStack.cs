using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.UI.Shell.Core;
using TestStack.White.UIItems;

namespace Automation.UI.Shell.TestStack
{
    public interface ITakeScreenshotTestStack : ITakeScreenshot
    {
        void SetUIItem(UIItem item);
    }
}
