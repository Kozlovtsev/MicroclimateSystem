using Newtonsoft.Json;
using OfficeOpenXml;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;
using MicroclimateSystem.Helpers;


namespace MicroclimateSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void SaveResultToHistory(CalculationResult result)
        {
            string filePath = "calculation_history.json";
            List<CalculationResult> history = new List<CalculationResult>();

            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                history = JsonConvert.DeserializeObject<List<CalculationResult>>(existingJson) ?? new List<CalculationResult>();
            }

            history.Add(result);
            string newJson = JsonConvert.SerializeObject(history, Formatting.Indented);
            File.WriteAllText(filePath, newJson);
        }

        private Room _room;

        public MainWindow()
        {
            InitializeComponent();
            _room = new Room();
        }

        private void OpenHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.ShowDialog();
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            // Ввод данных
            _room.Length = double.Parse(txtLength.Text);
            _room.Width = double.Parse(txtWidth.Text);
            _room.Height = double.Parse(txtHeight.Text);
            _room.NumberOfPigs = int.Parse(txtNumberOfPigs.Text);
            _room.PigType = (PigType)cmbPigType.SelectedIndex;
            _room.IsWinter = cmbSeason.SelectedIndex == 0; // 0 - Зима, 1 - Лето

            // Расчеты
            _room.Temperature = ClimateCalculator.CalculateOptimalTemperature(_room.PigType, _room.IsWinter);
            _room.Humidity = ClimateCalculator.CalculateOptimalHumidity(_room.IsWinter);
            _room.VentilationRate = ClimateCalculator.CalculateVentilationRate(_room.NumberOfPigs, _room.PigType, _room.IsWinter);
            _room.CO2Level = ClimateCalculator.CalculateCO2Level(_room.NumberOfPigs, _room.PigType);
            _room.AmmoniaLevel = ClimateCalculator.CalculateAmmoniaLevel(_room.NumberOfPigs);

            // Отображение результатов
            txtOptimalTemperature.Text = _room.Temperature.ToString("F2");
            txtOptimalHumidity.Text = _room.Humidity.ToString("F2");
            txtRequiredVentilation.Text = _room.VentilationRate.ToString("F2");
            txtCO2Level.Text = _room.CO2Level.ToString("F2");
            txtAmmoniaLevel.Text = _room.AmmoniaLevel.ToString("F2");

            CalculationResult result = new CalculationResult
            {
                RoomName = "Свиноводческое помещение",
                Date = DateTime.Now,
                Temperature = _room.Temperature,
                Humidity = _room.Humidity,
                VentilationRate = _room.VentilationRate,
                Notes = $"Тип: {_room.PigType}, Сезон: {(_room.IsWinter ? "Зима" : "Лето")}"
            };

            SaveResultToHistory(result);

        }

        private void ExportData_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv|JSON file (*.json)|*.json|Excel file (*.xlsx)|*.xlsx",
                Title = "Save Data"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                if (filePath.EndsWith(".csv"))
                {
                    ExportToCsv(_room, filePath);
                }
                else if (filePath.EndsWith(".json"))
                {
                    ExportToJson(_room, filePath);
                }
                else if (filePath.EndsWith(".xlsx"))
                {
                    ExportToExcel(_room, filePath);
                }
                MessageBox.Show("Экспорт данных завершен", "Экспорт", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Метод для экспорта в CSV
        private void ExportToCsv(Room room, string filePath)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Parameter,Value");
            csv.AppendLine($"Length (m),{room.Length}");
            csv.AppendLine($"Width (m),{room.Width}");
            csv.AppendLine($"Height (m),{room.Height}");
            csv.AppendLine($"Number of Pigs,{room.NumberOfPigs}");
            csv.AppendLine($"Pig Type,{room.PigType}");
            csv.AppendLine($"Optimal Temperature (°C),{room.Temperature}");
            csv.AppendLine($"Optimal Humidity (%),{room.Humidity}");
            csv.AppendLine($"Required Ventilation (m³/h),{room.VentilationRate}");
            csv.AppendLine($"CO₂ Level (ppm),{room.CO2Level}");
            csv.AppendLine($"Ammonia Level (ppm),{room.AmmoniaLevel}");
            File.WriteAllText(filePath, csv.ToString());
        }

        // Метод для экспорта в JSON
        private void ExportToJson(Room room, string filePath)
        {
            string json = JsonConvert.SerializeObject(room, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }


        // Метод для экспорта в Excel
        private void ExportToExcel(Room room, string filePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Microclimate Data");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "Параметры";
                worksheet.Cells[1, 2].Value = "Значения";

                // Данные
                worksheet.Cells[2, 1].Value = "Длина помещения (m)";
                worksheet.Cells[2, 2].Value = room.Length;

                worksheet.Cells[3, 1].Value = "Ширина помещения (m)";
                worksheet.Cells[3, 2].Value = room.Width;

                worksheet.Cells[4, 1].Value = "Высота помещения (m)";
                worksheet.Cells[4, 2].Value = room.Height;

                worksheet.Cells[5, 1].Value = "Количество Свиней";
                worksheet.Cells[5, 2].Value = room.NumberOfPigs;

                worksheet.Cells[6, 1].Value = "Тип Свиней";
                worksheet.Cells[6, 2].Value = room.PigType;

                worksheet.Cells[7, 1].Value = "Оптимальная температура (°C)";
                worksheet.Cells[7, 2].Value = room.Temperature;

                worksheet.Cells[8, 1].Value = "Оптимальная Влажность (%)";
                worksheet.Cells[8, 2].Value = room.Humidity;

                worksheet.Cells[9, 1].Value = "Требуемая Вентиляция (m³/h)";
                worksheet.Cells[9, 2].Value = room.VentilationRate;

                worksheet.Cells[10, 1].Value = "CO₂ уровень (ppm)";
                worksheet.Cells[10, 2].Value = room.CO2Level;

                worksheet.Cells[11, 1].Value = "уровень Аммияка (ppm)";
                worksheet.Cells[11, 2].Value = room.AmmoniaLevel;

                // Авто-ширина столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохранение файла
                FileInfo excelFile = new FileInfo(filePath);
                package.SaveAs(excelFile);
            }
        }

    }

    public enum PigType
    {
        Piglet,     // Поросенок
        Growing,    // Подсвинок
        Adult       // Взрослая свинья
    }

    public class CalculationHistory
    {
        private const string FilePath = "calculation_history.json";

        public static void SaveResults(List<CalculationResult> results)
        {
            var json = JsonConvert.SerializeObject(results, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static List<CalculationResult> LoadResults()
        {
            if (!File.Exists(FilePath))
                return new List<CalculationResult>();

            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<CalculationResult>>(json);
        }

    }


}