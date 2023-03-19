using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace TextEdit
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Font.ItemsSource = Fonts.SystemFontFamilies.OrderBy(u => u.Source);
            FontSize.ItemsSource = new List<double>() { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38 };
            Range.ItemsSource = new List<double>() { 1, 1.5, 2,2.5,3,3.5 };
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text files(*.rtf)|*.rtf|All files(*.*)|*.*";
            if (save.ShowDialog() != true) return;
            FileStream fileStream = new FileStream(save.FileName, FileMode.Create);
            TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files(*.rtf)|*.rtf|All files(*.*)|*.*";
            if (open.ShowDialog() != true) return;
            FileStream fileStream = new FileStream(open.FileName, FileMode.Open);
            TextRange range = new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd);
            range.Load(fileStream, DataFormats.Rtf);
        }

        private void Font_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Font.SelectedItem != null)
                Editor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, Font.SelectedItem);
        }

        private void FontSize_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Font.SelectedItem != null) return;
            Editor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, FontSize.SelectedItem);
        }


        private void Editor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = Editor.Selection.GetPropertyValue(Inline.FontWeightProperty);//Берём шрифт "Полужирный"
            Bold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = Editor.Selection.GetPropertyValue(Inline.FontStyleProperty);//Берём стиль курсив
            italic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = Editor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);//Берём линию снизу
            UnderLine.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = Editor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            Font.SelectedItem = temp;

            temp = Editor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            FontSize.Text = temp.ToString();
        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            var send = sender as Button;
            Editor.Selection.ApplyPropertyValue(Inline.ForegroundProperty, send.Background);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Range_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            Editor.Selection.ApplyPropertyValue(Paragraph.MarginProperty, new Thickness((Double)Range.SelectedItem));  

        }
    }
}
