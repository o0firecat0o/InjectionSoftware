using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;

namespace InjectionSoftware.Class
{
    public class AddingButton
    {
        public int Row
        {
            get
            {
                return InjectionsManager.injections.Count % 10;
            }
        }

        public int Column
        {
            get
            {
                return InjectionsManager.injections.Count / 10;
            }
        }
    }
}
