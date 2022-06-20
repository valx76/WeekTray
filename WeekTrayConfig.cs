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

        Button buttonBackgroundColor = new Button();
        buttonBackgroundColor.Text = "Background color..";
        buttonBackgroundColor.Dock = DockStyle.Fill;
        buttonBackgroundColor.Click += (sender, args) => ChangeColor(ref backgroundColor);

        Label emptyLabel = new Label();
        emptyLabel.Text = String.Empty;
        emptyLabel.Dock = DockStyle.Fill;

        Button buttonSave = new Button();
        buttonSave.Text = "Save";
        buttonSave.Click += (sender, args) => this.DialogResult = DialogResult.OK;
        buttonSave.Dock = DockStyle.Bottom;
        
        label = new Label();
        label.Text = "00";
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleCenter;
        UpdateLabelStyle();
        

        TableLayoutPanel innerLeftPanel = new TableLayoutPanel();
        innerLeftPanel.ColumnCount = 1;
        innerLeftPanel.RowCount = 3;
        innerLeftPanel.Dock = DockStyle.Fill;
        innerLeftPanel.Controls.Add(buttonTextColor);
        innerLeftPanel.Controls.Add(buttonBackgroundColor);
        innerLeftPanel.Controls.Add(emptyLabel);

        Panel leftPanel = new Panel();
        leftPanel.Padding = new Padding(20);
        leftPanel.Dock = DockStyle.Left;
        leftPanel.Controls.Add(innerLeftPanel);
        leftPanel.Controls.Add(buttonSave);

        Panel rightPanel = new Panel();
        rightPanel.Dock = DockStyle.Right;
        rightPanel.Controls.Add(label);


        this.Controls.Add(leftPanel);
        this.Controls.Add(rightPanel);
        
        this.Width = Constants.SETTINGS_WINDOW_WIDTH;
        this.Height = Constants.SETTINGS_WINDOW_HEIGHT;
        this.AcceptButton = buttonSave;

        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MinimizeBox = false;
        this.MaximizeBox = false;


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