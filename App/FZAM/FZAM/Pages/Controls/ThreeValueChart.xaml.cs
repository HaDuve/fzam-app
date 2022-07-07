using FZAM.Microcharts.Charts;
using Microcharts.Forms;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FZAM.Pages.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThreeValueChart : ContentView
    {
        private int chartCount;

        public ThreeValueChart()
        {
            this.InitializeComponent();
        }

        public void AddChart(CopingLineChart chart)
        {
            if (this.chartCount >= 1)
                chart.LabelColor = SKColors.Transparent;
            else
                chart.DrawBackground = true;

            this.chartCount++;
            var chartView = new ChartView();
            chartView.Chart = chart;
            //chartView.HorizontalOptions = LayoutOptions.FillAndExpand;
            chartView.VerticalOptions = LayoutOptions.FillAndExpand;
            chartView.HorizontalOptions = LayoutOptions.Fill;
            this.ContentGrid.Children.Add(chartView, 0, 0);
        }
    }
}