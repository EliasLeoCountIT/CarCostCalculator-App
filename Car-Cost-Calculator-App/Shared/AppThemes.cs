using MudBlazor;

namespace Car_Cost_Calculator_App.Shared;
public class AppThemes
{
    public MudTheme Theme { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Black = Colors.Shades.Black,
            White = Colors.Shades.White,
            Primary = Colors.Blue.Lighten1,
            Secondary = Colors.Red.Default,
            Tertiary = Colors.Green.Lighten2,
            Success = Colors.Green.Lighten4,
            Info = Colors.Blue.Darken4,
            Warning = Colors.Orange.Lighten1,
            Error = Colors.Red.Accent4,
            Dark = Colors.Teal.Darken4,
            Background = Colors.Shades.White,
            AppbarBackground = Colors.Blue.Lighten1,
            AppbarText = Colors.Shades.White,
            DrawerBackground = Colors.Gray.Lighten4,
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "4px",
        },
    };
}

