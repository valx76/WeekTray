namespace WeekTray;

static class Program
{
    [STAThread]
    static void Main()
    {
        using (Mutex mutex = new Mutex(false, "Global\\" + Constants.APP_ID))
        {
            if (!mutex.WaitOne(TimeSpan.Zero, false))
            {
                MessageBox.Show("Instance already running!", "WeekTray");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            var app = new WeekTray();
            app.Run();
        }
    }
}