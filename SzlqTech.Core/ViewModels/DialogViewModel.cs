﻿using CommunityToolkit.Mvvm.Input;
using Prism.Services.Dialogs;

namespace SzlqTech.Core.ViewModels
{
    public partial class DialogViewModel : ViewModelBase, IDialogAware
    {
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        [RelayCommand]
        public virtual void Cancel() => OnDialogClosed(ButtonResult.Cancel);

        [RelayCommand]
        public virtual void Save() => OnDialogClosed(ButtonResult.OK);

        public virtual bool CanCloseDialog() => true;

        public void OnDialogClosed(ButtonResult result)
        {
            RequestClose?.Invoke(new DialogResult(result));
        }

        public void OnDialogClosed(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public void OnDialogClosed() => OnDialogClosed(ButtonResult.OK);

        public virtual void OnDialogOpened(IDialogParameters parameters)
        { }
    }
}
