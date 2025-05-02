using MicroclimateSystem.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MicroclimateSystem
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
            LoadHistory();
        }

        private void LoadHistory()
        {
            var results = CalculationHistory.LoadResults();
            HistoryDataGrid.ItemsSource = results;
        }

        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryDataGrid.SelectedItem is CalculationResult selectedResult)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"Расчёт_{selectedResult.RoomName}_{selectedResult.Date:yyyyMMdd}.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    PdfExporter.ExportToPdf(selectedResult, saveFileDialog.FileName);
                    MessageBox.Show("Файл успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись из таблицы.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
