using System.Configuration;

namespace WeekTray;

public class WeekTrayUserSettings : ApplicationSettingsBase
{
    [UserScopedSetting()]
    public Color? TextColor
    {
        get => ((Color?)this[nameof(TextColor)]);
        set => this[nameof(TextColor)] = value;
    }
    
    [UserScopedSetting()]
    public Color? BackgroundColor
    {
        get => ((Color?)this[nameof(BackgroundColor)]);
        set => this[nameof(BackgroundColor)] = value;
    }
}