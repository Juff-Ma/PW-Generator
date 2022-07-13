using Avalonia.Controls;
using Avalonia;
using Avalonia.Platform;
using System;

namespace PW_Generator
{
    public partial class MainWindow : Window
    {
        const string chars = "^!\"$%&/()=?*+~#-_.:,;{[]}\\<>|@°´`";
        const string lettersBig = "QWERTZUIOPASDFGHJKLYXCVBNM";
        const string lettersSmall = "qwertzuiopasdfghjklyxcvbnm";
        const string numbers = "0123456789";
        public MainWindow()
        {
            InitializeComponent();
            if (AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem == OperatingSystemType.Linux)
            {
                title.IsVisible = false;
            }
            custom.Checked += Custom_Checked;
            custom.Unchecked += Custom_Checked;
            gen.Click += Gen_Click;
            save.Click += Save_Click;
        }

        private async void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            FileDialogFilter filter1 = new();filter1.Name = "Text Files";filter1.Extensions = new System.Collections.Generic.List<string>(){ "txt" };
            FileDialogFilter filter2 = new(); filter2.Name = "All Files"; filter2.Extensions = new System.Collections.Generic.List<string>() { "*" };
            var saveDialog = new SaveFileDialog() { DefaultExtension = ".txt", Title = "save password", InitialFileName = "password", Filters = new System.Collections.Generic.List<FileDialogFilter>() { filter1, filter2 } };

            string? path = await saveDialog.ShowAsync(this);

            if (path == null || path == "")
            {
                return;
            }
            System.IO.File.WriteAllText(path, output.Text);
        }

        private void Gen_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var r = new Random();
            if (custom.IsChecked == false)
            {
                System.Collections.Generic.List<char> pw = new();
                string complete = "";
                complete += lettersBigBox.IsChecked == true ? lettersBig : "";
                complete += lettersSmallBox.IsChecked == true ? lettersSmall : "";
                complete += numbersBox.IsChecked == true ? numbers : "";
                complete += charsBox.IsChecked == true ? chars : "";
                var completeC = complete.ToCharArray();
                if (completeC.Length == 0) { return; }
                for (int i = 0; i < Convert.ToInt64(length.Text); i++)
                {
                    pw.Add(completeC[r.Next(0, completeC.Length)]);
                }
                output.Text = new string(pw.ToArray());
            }
            else
            {
                char[] tmp = layout.Text.ToCharArray();
                string all = lettersBig + lettersSmall + chars + numbers;
                string lettersAll = lettersBig + lettersSmall;
                for(int i = 0; i < tmp.Length; i++)
                {
                    switch (tmp[i])
                    {
                        case '?':
                            tmp[i] = all[r.Next(0, all.Length)];
                            break;
                        case '=':
                            tmp[i] = numbers[r.Next(0, numbers.Length)];
                            break;
                        case '%':
                            tmp[i] = chars[r.Next(0, chars.Length)];
                            break;
                        case '#':
                            if (tmp[i] - 1 == '!')
                            {
                                tmp[i - 1] = '\0';
                                tmp[i] = lettersSmall[r.Next(0, lettersSmall.Length)];
                            }
                            else if (tmp[i] - 1 == '|')
                            {
                                tmp[i - 1] = '\0';
                                tmp[i] = lettersBig[r.Next(0, lettersBig.Length)];
                            }
                            else
                            {
                                tmp[i] = lettersAll[r.Next(0, lettersAll.Length)];
                            }
                            break;
                        default: break;
                    }
                }

                output.Text = new string(tmp);
            }
            save.IsEnabled = true;
        }

        private void Custom_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            lettersSmallBox.IsEnabled = !lettersSmallBox.IsEnabled;
            lettersBigBox.IsEnabled = !lettersBigBox.IsEnabled;
            numbersBox.IsEnabled = !numbersBox.IsEnabled;
            charsBox.IsEnabled = !charsBox.IsEnabled;
            layout.IsEnabled = !layout.IsEnabled;
            length.IsEnabled = !length.IsEnabled;
        }
    }
}
