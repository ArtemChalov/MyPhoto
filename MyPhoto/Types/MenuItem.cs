using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhoto.Types
{
    class MenuItem
    {
        private object _Content;
        private string _Description;

        public object Content
        {
            get { return Content; }
            set { Content = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
    }
}
