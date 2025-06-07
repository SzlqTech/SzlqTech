using NLog;
using SzlqTech.Common.Helper;
using CommunityToolkit.Mvvm.ComponentModel;


namespace SzlqTech.Entity
{
    [Serializable]
    public class LoginData : ObservableObject
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private string _userCode;
        private string _username;
        private string? _password;
        private bool _remember;

        public string UserCode
        {
            get => _userCode;
            set
            {
                if (_userCode == value) return;
                _userCode = value;
                OnPropertyChanged(nameof(UserCode));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public bool Remember
        {
            get => _remember;
            set
            {
                if (_remember == value) return;
                _remember = value;
                OnPropertyChanged(nameof(Remember));
            }
        }



        public static LoginData Load()
        {
            try
            {
                return BinHelper.Load<LoginData>();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return new LoginData();
            }
        }

        public static void Save(LoginData data)
        {
            BinHelper.Save(data);
        }

        public static void Save(string username, string? password, bool remember)
        {
            LoginData data = new LoginData();
            data.Username = username;
            data.Password = password;
            data.Remember = remember;
            BinHelper.Save(data);
        }

    }
}
