namespace Taskinator.Services
{
    public class DialogService : IDialogService
    {
        public async Task DisplayAlert(string title, string message, string accept)
        {
            var currentPage = GetCurrentPage();
            if (currentPage != null)
            {
                await currentPage.DisplayAlert(title, message, accept);
            }
            else
            {
                // Handle case when currentPage is null
            }
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            var currentPage = GetCurrentPage();
            if (currentPage != null)
            {
                return await currentPage.DisplayAlert(title, message, accept, cancel);
            }
            else
            {
                // Handle case when currentPage is null
                return false;
            }
        }

        public async Task<string> DisplayPromptDeleteOrEdit()
        {
            var currentPage = GetCurrentPage();
            if (currentPage != null)
            {
                string action = await currentPage.DisplayActionSheet(
                        "Choose action",
                        "Cancel",
                        null,
                        "Delete",
                        "Edit"
                    );

                switch (action)
                {
                    case "Delete":
                        return "Delete";
                    case "Edit":
                        return "Edit";
                    case "Cancel":
                        return "Cancel";
                    default:
                        return "Cancel";
                }
            }
            else
            {
                // Handle case when currentPage is null
                return "current page is null";
            }
        }


        public Page GetCurrentPage()
        {
            return App.Current.MainPage;
            //var currentShell = Shell.Current;
            //var stack = currentShell.Navigation.NavigationStack;
            //return stack.LastOrDefault();
        }
    }
}
