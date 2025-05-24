using Prism.Services.Dialogs;


namespace SzlqTech.Core.Services.Session
{
    public interface IHostDialogService : IDialogService
    {
        Task<IDialogResult> ShowDialogAsync(
           string name,
           IDialogParameters parameters = null,
           string IdentifierName = "Root");

        IDialogResult ShowWindow(string name);

        void Close(string IdentifierName, DialogResult dialogResult);
    }
}
