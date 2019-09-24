using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.Shared
{
    public class BreadcrumpsViewModel
    {
        [JsonProperty("breadcrumps")]
        public List<BreadcrumpViewModel> Breadcrumps { get; set; }

        public BreadcrumpsViewModel()
        {
            Breadcrumps = new List<BreadcrumpViewModel>();
        }
    }
}
