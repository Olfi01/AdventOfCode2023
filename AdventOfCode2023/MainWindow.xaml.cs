using AdventOfCode2023.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Path = System.IO.Path;

namespace AdventOfCode2023
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string applicationDataFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AdventOfCode2023");
        private static readonly string sessionCookieFilePath = Path.Combine(applicationDataFilePath, "session.cookie");
        private static readonly string inputCacheFilePath = Path.Combine(applicationDataFilePath, "inputCache");

        private readonly Button[] buttons = new Button[25];
        private readonly Button runPart1;
        private readonly Button runPart2;
        private int selectedDay = -1;
        private readonly CookieContainer cookies = new();
        private readonly HttpClientHandler httpClientHandler;
        private readonly HttpClient httpClient;

        public MainWindow()
        {
            Directory.CreateDirectory(applicationDataFilePath);
            Directory.CreateDirectory(inputCacheFilePath);

            httpClientHandler = new() { CookieContainer = cookies };
            httpClient = new HttpClient(httpClientHandler);
            if (File.Exists(sessionCookieFilePath)) SetSessionCookie(File.ReadAllText(sessionCookieFilePath));

            InitializeComponent();
            for (int i = 0; i < 25; i++)
            {
                int day = i + 1;
                Button button = new()
                {
                    Margin = new Thickness(10),
                    Content = i + 1
                };
                Grid.SetColumn(button, i % 5);
                Grid.SetRow(button, i / 5 + 1);
                leftColumn.Children.Add(button);
                buttons[i] = button;
                button.Click += (sender, e) => { selectedDay = day; UpdateButtonStates(); };
            }

            runPart1 = new()
            {
                Margin = new Thickness(10),
                Content = "Part 1",
                IsEnabled = false
            };
            Grid.SetColumn(runPart1, 0);
            Grid.SetRow(runPart1, 6);
            leftColumn.Children.Add(runPart1);
            runPart1.Click += (sender, e) => Run(selectedDay, 1);

            runPart2 = new()
            {
                Margin = new Thickness(10),
                Content = "Part 2",
                IsEnabled = false
            };
            Grid.SetColumn(runPart2, 4);
            Grid.SetRow(runPart2, 6);
            leftColumn.Children.Add(runPart2);
            runPart2.Click += (sender, e) => Run(selectedDay, 2);

            yearSelector.ItemsSource = ReflectionHelpers.GetAvailableYears();
            yearSelector.DisplayMemberPath = "Year";
            yearSelector.SelectedValuePath = "Type";
            yearSelector.SelectedIndex = 1;
        }

        public void UpdateButtonStates()
        {
            Type selectedYear = (Type)yearSelector.SelectedValue;
            IEnumerable<MethodInfo> methods = selectedYear.GetMethods().Where(m => m.GetCustomAttribute<PuzzleAttribute>() != null);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == selectedDay - 1)
                {
                    buttons[i].Background = Brushes.Green;
                }
                else
                {
                    buttons[i].Background = Brushes.LightGray;
                }
                buttons[i].IsEnabled = methods.Any(m => m.GetCustomAttribute<PuzzleAttribute>()!.Day == i + 1);
            }
            if (selectedDay >= 0 && selectedDay < buttons.Length)
            {
                Button button = buttons[selectedDay];
                if (button != null)
                {
                    runPart1.IsEnabled = methods.Any(m => { PuzzleAttribute attribute = m.GetCustomAttribute<PuzzleAttribute>()!; return attribute.Day == selectedDay && attribute.Part == 1; });
                    runPart2.IsEnabled = methods.Any(m => { PuzzleAttribute attribute = m.GetCustomAttribute<PuzzleAttribute>()!; return attribute.Day == selectedDay && attribute.Part == 2; });
                }
            }
        }

        private async void Run(int day, int part)
        {
            Type selectedYear = (Type)yearSelector.SelectedValue;
            IEnumerable<MethodInfo> methods = selectedYear.GetMethods().Where(m => { PuzzleAttribute? attribute = m.GetCustomAttribute<PuzzleAttribute>(); return attribute != null && attribute.Day == day && attribute.Part == part; });
            if (!methods.Any())
            {
                MessageBox.Show("No method found for this part!");
                return;
            }
            if (methods.Count() > 1)
            {
                MessageBox.Show("Multiple methods annotated with this part, can't decide on one!");
                return;
            }
            string? input = await GetInputString(((ReflectionHelpers.YearObj)yearSelector.SelectedItem).Year, day);
            if (input == null) return;
            object[] parameters = new object[] { input, display, outputLabel };
            display.Children.Clear();
            methods.First().Invoke(null, parameters);
        }

        private async Task<string?> GetInputString(int year, int day)
        {
            if (manualInputCheckbox.IsChecked == true)
            {
                InputBox inputBox = new("Manual Input", "Enter your example input:", true);
                string? savedExampleInput = await GetExampleString(year, day);
                if (savedExampleInput != null) inputBox.Text = savedExampleInput;
                bool? dialogResult = inputBox.ShowDialog();
                if (dialogResult != true) return null;
                SaveExampleString(year, day, inputBox.Text);
                return inputBox.Text;
            }
            string fileCachePath = Path.Combine(inputCacheFilePath, year.ToString(), $"{day}.txt");
            if (File.Exists(fileCachePath)) return await File.ReadAllTextAsync(fileCachePath);
            HttpResponseMessage response = await httpClient.GetAsync($"https://adventofcode.com/{year}/day/{day}/input");
            while (!response.IsSuccessStatusCode)
            {
                InputBox inputBox = new("Authentication failed", "Please enter session cookie:");
                bool? dialogResult = inputBox.ShowDialog();
                if (dialogResult != true) return null;
                SetSessionCookie(inputBox.Text);
                response = await httpClient.GetAsync($"https://adventofcode.com/{year}/day/{day}/input");
            }
            string result = await response.Content.ReadAsStringAsync();
            Directory.CreateDirectory(Path.GetDirectoryName(fileCachePath)!);
            _ = File.WriteAllTextAsync(fileCachePath, result);
            return result;
        }

        private static async Task<string?> GetExampleString(int year, int day)
        {
            string fileCachePath = Path.Combine(inputCacheFilePath, year.ToString(), $"{day}_example.txt");
            if (File.Exists(fileCachePath)) return await File.ReadAllTextAsync(fileCachePath);
            else return null;
        }

        private static void SaveExampleString(int year, int day, string? str)
        {
            if (str == null) return;
            string fileCachePath = Path.Combine(inputCacheFilePath, year.ToString(), $"{day}_example.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(fileCachePath)!);
            File.WriteAllTextAsync(fileCachePath, str);
        }

        private void SetSessionCookie(string sessionCookie)
        {
            cookies.Add(new Uri("https://adventofcode.com"), new Cookie("session", sessionCookie));
            File.WriteAllText(sessionCookieFilePath, sessionCookie);
        }

        private void YearSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonStates();
        }

        private void Copy_Result(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(outputLabel.Content.ToString());
        }
    }
}
