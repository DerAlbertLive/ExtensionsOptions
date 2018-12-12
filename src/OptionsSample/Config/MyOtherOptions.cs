using System.ComponentModel.DataAnnotations;

namespace Options.Tests
{
    public class MyOtherOptions
    {
        public string FullName { get; set; }

        [Required(ErrorMessage = "Hello, Unknown")]
        public string Unknown { get; set; }
    }
}