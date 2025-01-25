using CommunityToolkit.Mvvm.Input;
using RentalsProAPIV8.Maui.Infrastructure;

namespace RentalsProAPIV8.Maui.ViewModels
{
    public interface IViewModelBase : IQueryAttributable
    {
        public INavigation _navigation { get; }
        public IDeviceState _deviceState { get; }
        public IAsyncRelayCommand InitializeAsyncCommand { get; }
        public bool IsBusy { get; }
        public bool IsInitialized { get; }
        void InitializeDeviceConnection();
        void DisplayAlertAsync(string title, string message, string buttonText);
        Task InitializeAsync();
    }
}
