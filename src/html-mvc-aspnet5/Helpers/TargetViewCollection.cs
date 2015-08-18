using Microsoft.AspNet.Mvc.Rendering;
using System.Collections.Generic;

namespace html_mvc_aspnet5.Helpers
{
    public class TargetViewCollection
    {
        public IList<IView> Views { get; } = new List<IView>();
    }
}
