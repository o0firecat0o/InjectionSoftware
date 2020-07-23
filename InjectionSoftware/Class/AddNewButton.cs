using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;

namespace InjectionSoftware.Class
{
    public class AddNewButton
    {   
        //TODO: move the add new button to the bottom of all injections
        //TODO: update the row and column everytime a new injection has been added/ removed

        public int Row
        {
            get
            {
                return 9;
            }
        }

        public int Column
        {
            get
            {
                return 2;
            }
        }
    }
}
