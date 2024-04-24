using KonverterAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KonverterAPI
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static async Task Main(string Z, string V, decimal sum)
        {
            string baseCurrency = Z; // Базовая валюта
            string targetCurrency = V; // Целевая валюта
            decimal amount = sum; // Сумма для конвертации
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://api.exchangerate-api.com/v4/latest/{baseCurrency}");
                    response.EnsureSuccessStatusCode(); // Гарантирует, что ответ успешный
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Преобразуем ответ в объект
                    ExchangeRates exchangeRates = Newtonsoft.Json.JsonConvert.DeserializeObject<ExchangeRates>(responseBody);
                    // Получаем курс для целевой валюты
                    decimal exchangeRate = exchangeRates.Rates.First(r => r.Key == targetCurrency).Value;
                    // Вычисляем конвертированную сумму
                    decimal convertedAmount = amount * exchangeRate;
                    MessageBox.Show($"{amount} {baseCurrency} = {convertedAmount} {targetCurrency}");
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show($"Ошибка: {e.Message}");
                }
            }
        }
        // Класс для десериализации JSON-ответа
        public class ExchangeRates
        {
            public string Base { get; set; }
            public string Date { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((TextBlock)Z_cb.SelectedItem.Content);
            MessageBox.Show(V_cb.SelectedItem.ToString());
            MessageBox.Show(sum.Text);
            //Main(Z_cb.SelectedItem.ToString(),, decimal.Parse(sum.Text));
        }
    }
}