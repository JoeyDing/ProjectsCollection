using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.BuildsAndSource
{
    public interface IBuildsAndSourceView : IClickNext
    {
        string SourceCodeLocation { get; set; }
        IReadOnlyList<CheckableItem> SourceControl { get; set; }
        IReadOnlyList<CheckableItem> SourceStorage { get; set; }
        IReadOnlyList<SelectableItem> CodeReviewSystem { get; set; }
        IReadOnlyList<SelectableItem> BuildSystems { get; set; }

        event Action LoadBuildsAndSourceData;
    }
}