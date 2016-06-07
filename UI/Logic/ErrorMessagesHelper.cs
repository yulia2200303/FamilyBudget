using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Prism.Windows.AppModel;

namespace UI.Logic
{
    public static class ErrorMessagesHelper
    {
        static ErrorMessagesHelper()
        {
            ResourceLoader = new ResourceLoaderAdapter(new ResourceLoader());
        }

        public static IResourceLoader ResourceLoader { get; set; }

        public static string RequiredErrorMessage
        {
            get { return ResourceLoader.GetString("FieldIsRequired"); }
        }

        public static string NameAlreadyExists
        {
            get { return ResourceLoader.GetString("NameExists"); }
        }

        public static string LengthRange
        {
            get { return ResourceLoader.GetString("LengthFrom4To16"); }
        }
    }
}
