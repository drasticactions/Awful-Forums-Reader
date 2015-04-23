using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace AwfulForumsReader.Tools
{
    public class StatusBarBehavior : DependencyObject, IBehavior
    {
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible",
            typeof(bool),
            typeof(StatusBarBehavior),
            new PropertyMetadata(true, OnIsVisibleChanged));

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register("BackgroundOpacity",
            typeof(double),
            typeof(StatusBarBehavior),
            new PropertyMetadata(0d, OnOpacityChanged));

        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor",
            typeof(Color),
            typeof(StatusBarBehavior),
            new PropertyMetadata(null, OnForegroundColorChanged));

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor",
            typeof(Color),
            typeof(StatusBarBehavior),
            new PropertyMetadata(null, OnBackgroundChanged));

        public void Attach(DependencyObject associatedObject)
        {

        }

        public void Detach()
        {
        }

        public DependencyObject AssociatedObject { get; private set; }

        private static void OnIsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool isvisible = (bool)e.NewValue;
            var ns = "Windows.Phone.UI.Input.HardwareButtons";
            if (isvisible)
            {
                if (ApiInformation.IsTypePresent(ns))
                    StatusBar.GetForCurrentView().ShowAsync();
            }
            else
            {
                if (ApiInformation.IsTypePresent(ns))
                    StatusBar.GetForCurrentView().HideAsync();
            }
        }
        private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ns = "Windows.Phone.UI.Input.HardwareButtons";
            if (ApiInformation.IsTypePresent(ns))
            {
                StatusBar.GetForCurrentView().BackgroundOpacity = (double)e.NewValue;
            }
        }

        private static void OnForegroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ns = "Windows.Phone.UI.Input.HardwareButtons";
            if (ApiInformation.IsTypePresent(ns))
            {
                StatusBar.GetForCurrentView().ForegroundColor = (Color) e.NewValue;
            }
            else
            {
                ApplicationView.GetForCurrentView().TitleBar.ForegroundColor = (Color)e.NewValue;
            }
        }

        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (StatusBarBehavior)d;
            var ns = "Windows.Phone.UI.Input.HardwareButtons";
            if (ApiInformation.IsTypePresent(ns))
            {
                StatusBar.GetForCurrentView().BackgroundColor = behavior.BackgroundColor;
            }
            else
            {
                ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = behavior.BackgroundColor;
                ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = behavior.BackgroundColor;
            }

            // if they have not set the opacity, we need to so the new color is shown
            if (behavior.BackgroundOpacity == 0)
            {
                behavior.BackgroundOpacity = 1;
            }
        }
    }
}
