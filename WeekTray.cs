using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using Timer = System.Windows.Forms.Timer;

namespace WeekTray;

public class WeekTray
{
    private Font font;
    private StringFormat stringFormat;
    private Color textColor;
    private Color backgroundColor;
    private Brush textBrush;
    private Brush backgroundBrush;
    
    private Bitmap bitmap;
    private Rectangle bitmapRect;
    private Graphics g;
    
    private Timer timer;

    private NotifyIcon trayIcon;

    private WeekTrayConfig config;
    private WeekTrayUserSettings userSettings;


    public WeekTray()
    {
        ApplicationConfiguration.Initialize();
        Application.ApplicationExit += OnApplicationExit;
        
        font = new Font("Arial", Constants.TRAY_FONT_SIZE, FontStyle.Regular, GraphicsUnit.Pixel);
        stringFormat = new StringFormat();
        stringFormat.Alignment = StringAlignment.Center;
        stringFormat.LineAlignment = StringAlignment.Center;
        
        
        userSettings = new WeekTrayUserSettings();
        if (userSettings.TextColor == null || userSettings.BackgroundColor == null)
        {
            textColor = Constants.DEFAULT_TEXT_COLOR;
            backgroundColor = Constants.DEFAULT_BACKGROUND_COLOR;
            SaveUserSettings();
        }
        else
        {
            textColor = userSettings.TextColor ?? throw new ArgumentNullException(nameof(userSettings.TextColor));
            backgroundColor = userSettings.BackgroundColor ?? throw new ArgumentNullException(nameof(userSettings.BackgroundColor));
        }
        
        textBrush = new SolidBrush(textColor);
        backgroundBrush = new SolidBrush(backgroundColor);
        
        
        bitmap = new Bitmap(Constants.TRAY_ICON_SIZE, Constants.TRAY_ICON_SIZE);
        bitmapRect = new Rectangle(0, 0, Constants.TRAY_ICON_SIZE, Constants.TRAY_ICON_SIZE);
        g = Graphics.FromImage(bitmap);
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;


        timer = new Timer();
        timer.Tick += UpdateTrayIcon;
        timer.Interval = Constants.TIMER_DELAY;
        
        trayIcon = new NotifyIcon();
        trayIcon.ContextMenuStrip = CreateContextMenu();
        
        config = new WeekTrayConfig(textColor, backgroundColor);
    }
    
    public void Run()
    {
        trayIcon.Visible = true;
        UpdateTrayIcon(null, EventArgs.Empty);
        timer.Start();
        Application.Run();
    }

    private void OnApplicationExit(object? sender, EventArgs e)
    {
        timer.Stop();
        
        trayIcon.Dispose();
        timer.Dispose();
        g.Dispose();
        bitmap.Dispose();
        backgroundBrush.Dispose();
        textBrush.Dispose();
        stringFormat.Dispose();
        font.Dispose();
    }

    private void UpdateTrayIcon(object? sender, EventArgs e)
    {
        int week = ISOWeek.GetWeekOfYear(DateTime.Now);
        RenderTrayIcon(week.ToString());
    }

    private void RenderTrayIcon(string value)
    {
        g.Clear(Color.Transparent);
        g.FillRectangle(backgroundBrush, 0, 0, Constants.TRAY_ICON_SIZE, Constants.TRAY_ICON_SIZE);
        g.DrawString(value, font, textBrush, bitmapRect, stringFormat);

        IntPtr hIcon = bitmap.GetHicon();
        trayIcon.Icon = Icon.FromHandle(hIcon);
    }

    private ContextMenuStrip CreateContextMenu()
    {
        ContextMenuStrip menu = new ContextMenuStrip();

        ToolStripMenuItem menuConfigure = new ToolStripMenuItem("Configure");
        menuConfigure.Click += ContextMenu_Configure;
        
        ToolStripMenuItem menuAbout = new ToolStripMenuItem("About");
        menuAbout.Click += ContextMenu_About;
        
        ToolStripMenuItem menuExit = new ToolStripMenuItem("Exit");
        menuExit.Click += ContextMenu_Exit;

        menu.Items.Add(menuConfigure);
        menu.Items.Add(menuAbout);
        menu.Items.Add(menuExit);
        
        return menu;
    }

    private void ContextMenu_Configure(object? sender, EventArgs e)
    {
        if (config.ShowDialog() == DialogResult.OK)
        {
            ConfigDto data = config.GetData();
            textColor = data.textColor;
            backgroundColor = data.backgroundColor;
            
            textBrush.Dispose();
            backgroundBrush.Dispose();
            textBrush = new SolidBrush(textColor);
            backgroundBrush = new SolidBrush(backgroundColor);
            
            UpdateTrayIcon(null, EventArgs.Empty);
            SaveUserSettings();
        }
    }

    private void ContextMenu_About(object? sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = Constants.REPO_LINK,
            UseShellExecute = true
        });
    }
    
    private void ContextMenu_Exit(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    private void SaveUserSettings()
    {
        userSettings.TextColor = textColor;
        userSettings.BackgroundColor = backgroundColor;
        userSettings.Save();
    }
}