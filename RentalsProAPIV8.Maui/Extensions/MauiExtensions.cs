using CommunityToolkit.Mvvm.ComponentModel;

namespace RentalsProAPIV7.Maui.Extensions
{
    public static class MauiExtensions
    {
        public static void AddTab<TPage, TViewModel>(this TabbedPage tabbedPage, params object[] parameters) where TPage : ContentPage, new() where TViewModel : ObservableObject
        {
            if (tabbedPage == null) throw new ArgumentNullException(nameof(tabbedPage));

            var page = Activator.CreateInstance(typeof(TPage), parameters) as TPage
                ?? throw new InvalidOperationException($"Could not create an instance of {typeof(TPage)}.");

            page.OnAppearing((_, _) => page.BindingContext = Activator.CreateInstance(typeof(TViewModel), parameters));
            page.OnDisappearing((_, _) => page.BindingContext = null);

            tabbedPage.Children.Add(page);
        }

        public static void AddTab(this TabbedPage tabbedPage, ContentPage page, Type viewModelType, params object[] parameters)
        {
            if (tabbedPage == null) throw new ArgumentNullException(nameof(tabbedPage));
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (viewModelType == null) throw new ArgumentNullException(nameof(viewModelType));

            page.OnAppearing((_, _) => page.BindingContext = Activator.CreateInstance(viewModelType, parameters));
            page.OnDisappearing((_, _) => page.BindingContext = null);

            tabbedPage.Children.Add(page);
        }

        public static void SetBindingContext<TViewModel>(this Page page, TViewModel viewModel) where TViewModel : class
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            page.BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public static void AddView(this ContentPage page, View view)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (view == null) throw new ArgumentNullException(nameof(view));

            if (page.Content is Layout layout)
                layout.Children.Add(view);
            else
                page.Content = view;
        }

        public static void AddViewToLayout<TLayout>(this ContentPage page, View view, string layoutName) where TLayout : Layout
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (view == null) throw new ArgumentNullException(nameof(view));
            if (string.IsNullOrWhiteSpace(layoutName)) throw new ArgumentNullException(nameof(layoutName));

            page.FindByName<TLayout>(layoutName)?.Children.Add(view);
        }

        public static void OnAppearing(this ContentPage page, EventHandler handler)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            page.Appearing += handler;
        }

        public static void OnDisappearing(this ContentPage page, EventHandler handler)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            page.Disappearing += handler;
        }

        public static void AddToolbarItem(this ContentPage page, ToolbarItem item)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (item == null) throw new ArgumentNullException(nameof(item));

            page.ToolbarItems.Add(item);
        }

        public static void SetTitle(this ContentPage page, string title)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));

            page.Title = title;
        }

        public static void AddChild<TLayout>(this TLayout layout, View view) where TLayout : Layout
        {
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            if (view == null) throw new ArgumentNullException(nameof(view));

            layout.Children.Add(view);
        }

        public static TView FindChildByName<TView>(this Layout layout, string name) where TView : View
        {
            if (layout == null) throw new ArgumentNullException(nameof(layout));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            return layout.FindByName<TView>(name);
        }

        //public static async Task DisplayAlertAsync(this Page page, string title, string message, string cancel = "Ok")
        //{
        //    if (page != null)
        //    {
        //        await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        //    }
        //}
    }
}
