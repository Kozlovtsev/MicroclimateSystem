
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using MicroclimateSystem.Models;
using MicroclimateSystem.Helpers;

namespace MicroclimateSystem.Views
{
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
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
