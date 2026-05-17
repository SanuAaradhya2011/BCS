using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSComService
{
    class FileUploadStatus
    {

        public bool isGeneralUploaded { get; set; }

        public bool isInstantUploaded { get; set; }

        public bool isBillingUploaded { get; set; }

        public bool isLoadSurveyUploaded { get; set; }

        public bool isTamperUploaded { get; set; }
    }
}
