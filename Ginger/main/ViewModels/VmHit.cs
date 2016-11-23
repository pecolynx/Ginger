using Ginger.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.ViewModels
{
    internal class VmHit
    {
        public VmHit(Hit hit)
        {
            var id = hit.Document.Id;
            var fileName = this.GetFirstFieldValue(hit.Document, "file_name");
            var filePath = this.GetFirstFieldValue(hit.Document, "file_path");
            var fileContent = this.GetFirstFieldValue(hit.Document, "file_content");
            var fileCreatedAt = DateTime.Parse(this.GetFirstFieldValue(hit.Document, "file_created_at"));
            var fileUpdatedAt = DateTime.Parse(this.GetFirstFieldValue(hit.Document, "file_updated_at"));

            this.Document = new VmDocumentFile(id, fileName, filePath, fileContent, string.Empty, fileCreatedAt, fileUpdatedAt);
            this.Title = hit.Title;
            this.Highlight = hit.Highlight;
            this.Score = hit.Score;
        }

        public VmDocumentFile Document { get; set; }

        public string Title { get; set; }

        public string Highlight { get; set; }

        public double Score { get; set; }

        private string GetFirstFieldValue(DocumentFile documentFile, string fieldName)
        {
            return documentFile.DocumentFieldList
                .Where(x => x.FieldName == fieldName)
                .Select(x => x.FieldValueList.First())
                .First();
        }
    }
}
