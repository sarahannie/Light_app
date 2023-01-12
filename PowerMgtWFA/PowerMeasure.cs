using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace PowerMgtWFA
{
    public partial class PowerMeasure : Form
    {
        private readonly ComboBox processSelector = new ComboBox();
        private readonly Chart powerChart = new Chart();
        private readonly TextBox avgPowerTextBox = new TextBox();
        private PerformanceCounter counter;

        public PowerMeasure()
        {
            InitializeComponent();
            Text = "Power Consumption Measuring App";
            Size = new Size(590, 450);
            FormBorderStyle= FormBorderStyle.FixedSingle;   
            MaximizeBox= false;
            CenterToScreen();

            // Add average power text box
            avgPowerTextBox.Location = new Point(10, 360);
            avgPowerTextBox.Size = new Size(550, 30);
            avgPowerTextBox.ReadOnly = true;
            Controls.Add(avgPowerTextBox);

            // Add process selector
            processSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            processSelector.Location = new Point(10, 10);
            processSelector.Size = new Size(550, 30);
            processSelector.SelectedIndexChanged += ProcessSelector_SelectedIndexChanged;
            Controls.Add(processSelector);

            // Add power chart
            powerChart.Location = new Point(10, 50);
            powerChart.Size = new Size(550, 300);
            powerChart.ChartAreas.Add(new ChartArea());
            powerChart.Series.Add(new Series());
            powerChart.Series[0].ChartType = SeriesChartType.Line;
            Controls.Add(powerChart);
        }

        private void ProcessSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (counter != null)
            {
                counter.Dispose();
            }

            // Get selected process
            var selectedProcess = (Process)processSelector.SelectedItem;
            counter = new PerformanceCounter("Process", "Working Set - Private", selectedProcess.ProcessName);
            counter.NextValue();
            powerChart.Series[0].Points.Clear();

            // Start measuring power consumption
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, ev) =>
            {
                var power = counter.NextValue();
                powerChart.Series[0].Points.AddY(power);
                CalculateAvgPower();
            };
            timer.Start();
        }

        private void CalculateAvgPower()
        {
            var hour = 60 * 60 * 1000; // 60 minutes * 60 seconds * 1000 ms
            var avgPower = powerChart.Series[0].Points.Count > 0 ?
                powerChart.Series[0].Points.Average(p => p.YValues[0]) / (powerChart.Series[0].Points.Count / (hour / 1000.0)) : 0;
            avgPowerTextBox.Text = $"Average Power Consumption per Hour: {avgPower:0.00} W";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Get all running processes and add to selector
            var processes = Process.GetProcesses();
            processSelector.DataSource = processes.OrderBy(p => p.ProcessName).ToList();
            processSelector.DisplayMember = "ProcessName";
        }
    }
}