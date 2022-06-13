using System.Globalization;
using Timer = System.Windows.Forms.Timer;

namespace WeekTray;

public class WeekTray
{
    private Font font;
    private StringFormat stringFormat;
    private Brush textBrush;
    private Brush backgroundBrush;
    
    private Bitmap bitmap;
    private Rectangle bitmapRect;
    private Graphics g;
    
    private Timer timer;

    private NotifyIcon trayIcon;


    public WeekTray()
    {
        ApplicationConfiguration.Initialize();
        Application.ApplicationExit += OnApplicationExit;
        
        font = new Font("Arial", Constants.TRAY_FONT_SIZE, FontStyle.Regular, GraphicsUnit.Pixel);
        stringFormat = new StringFormat();
        stringFormat.Alignment = StringAlignment.Center;
        stringFormat.LineAlignment = StringAlignment.Center;
        
        textBrush = new SolidBrush(Color.White);
        backgroundBrush = new SolidBrush(Color.DimGray);
        
        bitmap = new Bitmap(Constants.TRAY_ICON_SIZE, Constants.TRAY_ICON_SIZE);
        bitmapRect = new Rectangle(0, 0, Constants.TRAY_ICON_SIZE, Constants.TRAY_ICON_SIZE);
        g = Graphics.FromImage(bitmap);

        timer = new Timer();
        timer.Tick += TimerTick;
        timer.Interval = Constants.TIMER_DELAY;
        
        trayIcon = new NotifyIcon();
        trayIcon.ContextMenuStrip = CreateContextMenu();
    }
    
    public void Run()
    {
        trayIcon.Visible = true;
        TimerTick(null, EventArgs.Empty);  // Trigger for initial display
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

    private void TimerTick(object? sender, EventArgs e)
    {
        int week = ISOWeek.GetWeekOfYear(DateTime.Now);
        UpdateTrayIcon(week.ToString());
    }

    private void UpdateTrayIcon(string value)
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

        ToolStripMenuItem menuExit = new ToolStripMenuItem("Exit");
        menuExit.Click += ContextMenu_Exit;

        menu.Items.Add(menuExit);
        
        return menu;
    }

    private void ContextMenu_Exit(object? sender, EventArgs e)
    {
        Application.Exit();
    }
}