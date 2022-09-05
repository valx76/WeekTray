namespace WeekTray;

public class WeekTrayConfig : Form
{
    private Color textColor;
    private Color backgroundColor;
    private ColorDialog colorDialog;
    private Label label;
    private ConfigDto dto;

    public WeekTrayConfig(Color defaultTextColor, Color defaultBackgroundColor)
    {
        textColor = defaultTextColor;
        backgroundColor = defaultBackgroundColor;

        colorDialog = new ColorDialog();
        colorDialog.AllowFullOpen = true;
        colorDialog.FullOpen = true;

        Button buttonTextColor = new Button();
        buttonTextColor.Text = "Text color..";
        buttonTextColor.Dock = DockStyle.Fill;
        buttonTextColor.Click += (sender, args) => ChangeColor(ref textColor);
        buttonTextColor.AutoSize = true;
        buttonTextColor.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        Button buttonBackgroundColor = new Button();
        buttonBackgroundColor.Text = "Background color..";
        buttonBackgroundColor.Dock = DockStyle.Fill;
        buttonBackgroundColor.Click += (sender, args) => ChangeColor(ref backgroundColor);
        buttonBackgroundColor.AutoSize = true;
        buttonBackgroundColor.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        Button buttonSave = new Button();
        buttonSave.Text = "Save";
        buttonSave.Click += (sender, args) => this.DialogResult = DialogResult.OK;
        buttonSave.Dock = DockStyle.Bottom;
        buttonSave.AutoSize = true;
        buttonSave.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        label = new Label();
        label.Text = "00";
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleCenter;
        UpdateLabelStyle();
        

        TableLayoutPanel innerLeftPanel = new TableLayoutPanel();
        innerLeftPanel.ColumnCount = 1;
        innerLeftPanel.RowCount = 3;
        innerLeftPanel.Dock = DockStyle.Fill;
        innerLeftPanel.AutoSize = true;
        innerLeftPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        innerLeftPanel.Controls.Add(buttonTextColor);
        innerLeftPanel.Controls.Add(buttonBackgroundColor);

        Panel leftPanel = new Panel();
        leftPanel.Padding = new Padding(20);
        leftPanel.Dock = DockStyle.Fill;
        leftPanel.AutoSize = true;
        leftPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        leftPanel.Controls.Add(innerLeftPanel);
        leftPanel.Controls.Add(buttonSave);

        Panel rightPanel = new Panel();
        rightPanel.Dock = DockStyle.Right;
        rightPanel.Width = 200;
        rightPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        rightPanel.Controls.Add(label);


        this.Controls.Add(leftPanel);
        this.Controls.Add(rightPanel);

        this.AutoSize = true;
        this.AcceptButton = buttonSave;

        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MinimizeBox = false;
        this.MaximizeBox = false;

        this.Text = "Configuration";


        dto = new ConfigDto()
        {
            textColor = textColor,
            backgroundColor = backgroundColor
        };
    }

    public ConfigDto GetData()
    {
        return dto;
    }

    private void ChangeColor(ref Color refColor)
    {
        colorDialog.Color = refColor;
        if (colorDialog.ShowDialog() == DialogResult.OK)
        {
            refColor = colorDialog.Color;
            UpdateLabelStyle();

            dto.backgroundColor = backgroundColor;
            dto.textColor = textColor;
        }
    }

    private void UpdateLabelStyle()
    {
        label.ForeColor = textColor;
        label.BackColor = backgroundColor;
    }
}