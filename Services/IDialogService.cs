namespace Taskinator.Services
{
    public interface IDialogService
    {
        Task DisplayAlert(string title, string message, string accept);
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
        Task<string> DisplayPromptDeleteOrEdit();

        Page GetCurrentPage();
    }
}
