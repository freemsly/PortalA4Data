using System;
using System.IO;
using System.Linq;
using System.Web;
using CamprayPortal.Core;
 
using CamprayPortal.Services.Media;
using CamprayPortal.Services.Seo;
using OfficeOpenXml;

namespace CamprayPortal.Services.ExportImport
{
    /// <summary>
    /// Import manager
    /// </summary>
    public partial class ImportManager : IImportManager
    {
        #region Fields

       
        private readonly IPictureService _pictureService;
        private readonly IUrlRecordService _urlRecordService;

        #endregion

        #region Ctor

        public ImportManager( IPictureService pictureService,
            IUrlRecordService urlRecordService)
        {
           
            this._pictureService = pictureService;
            this._urlRecordService = urlRecordService;
        }

        #endregion

        #region Utilities

        protected virtual int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }

        protected virtual string ConvertColumnToString(object columnValue)
        {
            if (columnValue == null)
                return null;

            return Convert.ToString(columnValue);
        }

        protected virtual string GetMimeTypeFromFilePath(string filePath)
        {
            var mimeType = MimeMapping.GetMimeMapping(filePath);

            //little hack here because MimeMapping does not contain all mappings (e.g. PNG)
            if (mimeType == "application/octet-stream")
                mimeType = "image/jpeg";

            return mimeType;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Import products from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        public virtual void ImportProductsFromXlsx(Stream stream)
        { 
        }

        #endregion
    }
}
