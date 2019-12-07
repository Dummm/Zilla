using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zilla.Models
{
    public enum ToastType { Success, Warning, Danger, Info};
    public class Toast
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public ToastType Type { get; set; }
    }
}