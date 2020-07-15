using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeImageFromYoutube.Facade.Functions
{
    class CutVideo
    {
        #region # Property

        private Log log;

        #endregion

        #region # Constructor

        public CutVideo()
        {
            log = new Log();
        }

        #endregion

        #region # Method

        public bool Cut(Info info)
        {
            try
            {

                return true;
            }
            catch (Exception ex)
            {
                log.WriteLog("[ERROR] : " + ex);
                return false;
            }
        }

        #endregion
    }
}
