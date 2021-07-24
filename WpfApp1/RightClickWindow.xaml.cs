﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StreamTrack
{
    /// <summary>
    /// Interaction logic for RightClickWindow.xaml
    /// </summary>
    public partial class RightClickWindow : Window
    {
        private readonly StackPanel panel;
        private readonly List<WpfApp1.SavedStreamer> SavedStreamers;
        private readonly List<WpfApp1.Streamer> Streamers;
        private readonly List<Newtonsoft.Json.Linq.JObject> JsonList;
        private readonly string path;
        private System.Action del;

        public RightClickWindow(StackPanel panel, Window window, List<WpfApp1.SavedStreamer> SavedStreamers, List<WpfApp1.Streamer> Streamers, List<Newtonsoft.Json.Linq.JObject> JsonList, string path, System.Action del)
        {
            this.panel = panel;
            this.SavedStreamers = SavedStreamers;
            this.Streamers = Streamers;
            this.JsonList = JsonList;
            this.path = path;
            this.del = del;
            Left = window.Left + Mouse.GetPosition(window).X;
            Top = window.Top + Mouse.GetPosition(window).Y;
            InitializeComponent();
            StreamerNameLabel.Content = panel.Tag.ToString();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string name = panel.Tag.ToString();
            SavedStreamers.Remove(SavedStreamers.Find(streamer => streamer.name == name));
            Streamers.Remove(Streamers.Find(streamer => streamer.user_name == name));
            JsonList.Remove(JsonList.Find(item => item.Property("name").Value.ToString() == name));

            using (System.IO.StreamWriter FileWriter = System.IO.File.CreateText(path))
            {
                Newtonsoft.Json.JsonSerializer Serializer = new();
                Serializer.Serialize(FileWriter, JsonList);
            }

            Grid grid = panel.Parent as Grid;
            grid.RowDefinitions.Remove(grid.RowDefinitions[Grid.GetRow(panel)]);
            grid.Children.Remove(panel);
            for(int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                Grid.SetRow(grid.Children[i], i);
            }
            del();
            Close();

        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(50, 200, 200, 200));
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
