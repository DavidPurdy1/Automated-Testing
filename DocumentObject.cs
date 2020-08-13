using System.Collections.Generic;
namespace ConsoleTests
{
    public class DocumentObject
    {
        public int TestCaseId { get; set; }
        public int DocumentId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public List<string> CustomFieldData { get; set; }
    }
}