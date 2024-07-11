using CommunityToolkit.Mvvm.ComponentModel;
using Taskinator.Services;

namespace Taskinator.ViewModels
{
    public partial class BaseViewModel : ObservableObject, IDisposable
    {
        protected bool IsConnectedToInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        // Implement IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean up managed resources (if any)
            }

            // Clean up unmanaged resources (if any)
        }

        // Finalizer (if necessary)
        ~BaseViewModel()
        {
            Dispose(false);
        }
    }
}
