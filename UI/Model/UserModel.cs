using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Model;
using UI.ViewModel.Common;

namespace UI.Model
{
    //public class UserModel: User
    //{
    //    public bool IsPanelShow { get; set; }

    //    public string EnteredPassword { get; set; }

    //    public static UserModel Convert(User user)
    //    {
    //        return new UserModel
    //        {
    //            IsPanelShow = false,
    //            EnteredPassword = string.Empty,
    //            Name = user.Name,
    //            Hash = user.Hash,
    //            Salt = user.Salt,
    //            IsPasswordSet = user.IsPasswordSet,
    //            Id = user.Id,
    //            Assets = user.Assets
    //        };
    //    }

    //    public static User ConvertBack(UserModel userModel)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class UserModel : BaseViewModel
    {
        private bool _isPaneShow;
        public bool IsPanelShow
        {
            get { return _isPaneShow; }
            set { this.SetProperty(ref _isPaneShow, value); }
        }

        private string _enteredPassword;
        public string EnteredPassword
        {
            get { return _enteredPassword; }
            set { this.SetProperty(ref _enteredPassword, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref _name, value); }
        }

        private string _hash;
        public string Hash
        {
            get { return _hash; }
            set { this.SetProperty(ref _hash, value); }
        }

        private string _salt;
        public string Salt
        {
            get { return _salt; }
            set { this.SetProperty(ref _salt, value); }
        }

        private bool _isPasswordSet;

        public bool IsPasswordSet
        {
            get { return _isPasswordSet; }
            set { this.SetProperty(ref _isPasswordSet, value); }
        }


        private ObservableCollection<Asset> _assets;
        public ObservableCollection<Asset> Assets
        {
            get { return _assets; }
            set { SetProperty(ref _assets, value); }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public static UserModel Convert(User user)
        {
            return new UserModel
            {
                IsPanelShow = false,
                EnteredPassword = string.Empty,
                Name = user.Name,
                Hash = user.Hash,
                Salt = user.Salt,
                IsPasswordSet = user.IsPasswordSet,
                Id = user.Id,
                Assets = user.Assets == null ? new ObservableCollection<Asset>() : new ObservableCollection<Asset>(user.Assets)
            };
        }

        public static User ConvertBack(UserModel userModel)
        {
            throw new NotImplementedException();
        }
    }
}
