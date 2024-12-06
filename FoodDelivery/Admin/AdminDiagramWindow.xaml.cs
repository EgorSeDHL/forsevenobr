using FoodDelivery.FoodDeliveryDBDataSetTableAdapters;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FoodDelivery.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminDiagramWindow.xaml
    /// </summary>
    public partial class AdminDiagramWindow : Window
    {
        OrdersTableAdapter orders = new OrdersTableAdapter();
        public PlotModel SalesPlotModel { get; private set; }

        public AdminDiagramWindow()
        {
            InitializeComponent();
            DataContext = this;
            List<Sale> salesData = new List<Sale>();
            var allOrders = orders.GetData();
            foreach (var order in allOrders)
            {
                salesData.Add(new Sale { Date = order.order_date, Amount = (double)order.total_amount });
            }
            // Пример данных о продажах

            // Настройка модели диаграммы
            SalesPlotModel = new PlotModel { Title = "Sales Over Time" };
            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "MMM yyyy",
                Title = "Date"
            };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Sales Amount",
                Minimum = 0
            };

            SalesPlotModel.Axes.Add(dateAxis);
            SalesPlotModel.Axes.Add(valueAxis);

            var series = new LineSeries
            {
                Title = "Sales",
                MarkerType = MarkerType.Circle
            };

            foreach (var sale in salesData)
            {
                series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(sale.Date), sale.Amount));
            }

            SalesPlotModel.Series.Add(series);
        }
    }

    public class Sale
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
}
