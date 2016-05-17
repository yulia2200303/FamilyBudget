using System;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.Web.Http;

namespace UI.Logic
{
    [DataContract]
    public class UserContext
    {
        private static readonly UserContext Context = CreateUserContext();
        private const string SettingsKey = "CurrentUser";

        private UserContext()
        {
           
        }

        private static UserContext CreateUserContext()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(SettingsKey))
            {
                return SerializeWrapper<UserContext>.Deserialize(ApplicationData.Current.LocalSettings.Values[SettingsKey].ToString());
            }

            return new UserContext();
        }

        [IgnoreDataMember]
        public static UserContext Current
        {
            get { return Context; }
        }

        [DataMember]
        public int UserId { get; private set; }

        [DataMember]
        public string UserName { get; private set; }

        [DataMember]
        public string Password { get; private set; }

        [DataMember]
        public Cookie Cookie { get; private set; }

        [IgnoreDataMember]
        public bool IsAuthenticated
        {
            get
            {
                return UserId != 0;
            }
        }

        public void Authenticate(string userName, string password, int userId, HttpCookie cookie)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.Password = password;
            this.Cookie = new CookieConverter().Convert(cookie, null, null, "en") as Cookie;
            ApplicationData.Current.LocalSettings.Values[SettingsKey] = SerializeWrapper<UserContext>.Serialize(this);
        }

        public void Logout()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(SettingsKey))
            {
                ApplicationData.Current.LocalSettings.Values.Remove(SettingsKey);
            }

            Context.UserId = 0;
            Context.UserName = "";
        }
    }

   
    [DataContract]
    public sealed class Cookie : IStringable
    {
        [DataMember]
        public string Domain { get; set; }

        [DataMember]
        public DateTimeOffset? Expires { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Path { get; set; }
        // public bool HttpOnly { get; set; }
        // public bool Secure { get; set; }
    }

    internal class CookieConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var httpCookie = value as HttpCookie;
            if (httpCookie == null) return DependencyProperty.UnsetValue;

            return new Cookie
            {
                Domain = httpCookie.Domain,
                Expires = httpCookie.Expires,
                Name = httpCookie.Name,
                Path = httpCookie.Path,
                Value = httpCookie.Value
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var cookie = value as Cookie;
            if (cookie == null) return DependencyProperty.UnsetValue;
            var httpCookie = new HttpCookie(cookie.Name, cookie.Domain, cookie.Path)
            {
                Value = cookie.Value,
                Expires = cookie.Expires
            };
            return httpCookie;
        }
    }

}
