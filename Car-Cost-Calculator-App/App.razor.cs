using Car_Cost_Calculator_App.Shared;
using Microsoft.AspNetCore.Components;

namespace Car_Cost_Calculator_App.Components
{
    public partial class App
    {
        [Inject]
        public AppThemes AppThemes { get; set; } = null!;
    }
}