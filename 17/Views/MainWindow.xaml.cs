using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HotelAnimatedApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnWindowLoaded;
            DataContextChanged += (s, e) => BindViewModel();
            if (DataContext != null) BindViewModel();
        }

        private void BindViewModel()
        {
            if (DataContext is ViewModels.HotelViewModel vm)
            {
                vm.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(vm.HasSelectedRoom) && vm.HasSelectedRoom)
                        AnimateDetailPanel();

                    if (e.PropertyName == nameof(vm.StatusMessage) &&
                        !string.IsNullOrEmpty(vm.StatusMessage))
                        AnimateStatusBar();
                };
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Шапка
            var headerAnim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.6));
            HeaderGrid.BeginAnimation(OpacityProperty, headerAnim);

            // Форма — FadeIn
            var formFade = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            formFade.BeginTime = TimeSpan.FromSeconds(0.3);
            FormBorder.BeginAnimation(OpacityProperty, formFade);

            // Форма — SlideIn (заменили "is not" на обычную проверку)
            if (!(FormBorder.RenderTransform is TranslateTransform))
                FormBorder.RenderTransform = new TranslateTransform();

            var formSlide = new DoubleAnimation(30, 0, TimeSpan.FromSeconds(0.5));
            formSlide.BeginTime = TimeSpan.FromSeconds(0.3);
            formSlide.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            ((TranslateTransform)FormBorder.RenderTransform)
                .BeginAnimation(TranslateTransform.YProperty, formSlide);

            // Статус
            var statusAnim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.4));
            statusAnim.BeginTime = TimeSpan.FromSeconds(0.5);
            StatusBar.BeginAnimation(OpacityProperty, statusAnim);
        }

        private void RoomCard_Loaded(object sender, RoutedEventArgs e)
        {
            // Заменили "is not Border" на обычную проверку
            Border border = sender as Border;
            if (border == null) return;

            // Создаём ScaleTransform программно
            var scale = new ScaleTransform(0.75, 0.75);
            border.RenderTransform = scale;
            border.RenderTransformOrigin = new Point(0.5, 0.5);

            var index = GetItemIndex(border);
            var delay = TimeSpan.FromSeconds(0.07 * Math.Max(0, index));

            // FadeIn
            var fadeAnim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.45));
            fadeAnim.BeginTime = delay;
            fadeAnim.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            border.BeginAnimation(OpacityProperty, fadeAnim);

            // Scale X
            var scaleX = new DoubleAnimation(0.75, 1, TimeSpan.FromSeconds(0.45));
            scaleX.BeginTime = delay;
            scaleX.EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleX);

            // Scale Y
            var scaleY = new DoubleAnimation(0.75, 1, TimeSpan.FromSeconds(0.45));
            scaleY.BeginTime = delay;
            scaleY.EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleY);
        }

        private int GetItemIndex(DependencyObject element)
        {
            try
            {
                var parent = VisualTreeHelper.GetParent(element);
                while (parent != null && !(parent is ContentPresenter))
                    parent = VisualTreeHelper.GetParent(parent);

                if (parent is ContentPresenter cp)
                    return RoomsControl.ItemContainerGenerator.IndexFromContainer(cp);
            }
            catch { }
            return 0;
        }

        private void AnimateDetailPanel()
        {
            Dispatcher.Invoke(() =>
            {
                // Заменили "is not" на обычную проверку
                if (!(DetailPanel.RenderTransform is TranslateTransform))
                    DetailPanel.RenderTransform = new TranslateTransform();

                var fade = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.35));
                fade.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
                DetailPanel.BeginAnimation(OpacityProperty, fade);

                var slide = new DoubleAnimation(40, 0, TimeSpan.FromSeconds(0.35));
                slide.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
                ((TranslateTransform)DetailPanel.RenderTransform)
                    .BeginAnimation(TranslateTransform.XProperty, slide);
            });
        }

        private void AnimateStatusBar()
        {
            Dispatcher.Invoke(() =>
            {
                var anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                StatusBar.BeginAnimation(OpacityProperty, anim);
            });
        }
    }
}