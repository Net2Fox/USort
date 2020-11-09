using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace USort
{
    public class Pages //Данный класс служит для создание экземпляров страниц и доступа к ним из любой части программы
    {
        internal static MainPage mp;
        internal static Settings_Page sp;
        internal static Advance_Settings_Page asp;

        //internal static void DarkTheme()
        //{
        //    foreach (TextBlock tx in sp.Grid1.Children.OfType<TextBlock>())
        //    {
        //        tx.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        //    }
        //    foreach (TextBlock tx in mp.MainGrid.Children.OfType<TextBlock>())
        //    {
        //        tx.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        //    }
        //    foreach (CheckBox ch in sp.Grid1.Children.OfType<CheckBox>())
        //    {
        //        ch.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        //    }
        //    mp.MainGrid.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //    sp.Grid1.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //    //sp.DarkTheme.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        //}

        //internal static void WhiteTheme()
        //{
        //    foreach (TextBlock tx in sp.Grid1.Children.OfType<TextBlock>())
        //    {
        //        tx.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //    }
        //    foreach (TextBlock tx in mp.MainGrid.Children.OfType<TextBlock>())
        //    {
        //        tx.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //    }
        //    foreach (CheckBox ch in sp.Grid1.Children.OfType<CheckBox>())
        //    {
        //        ch.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //    }
        //    mp.MainGrid.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        //    sp.Grid1.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        //    //sp.DarkTheme.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        //}

    }
}
