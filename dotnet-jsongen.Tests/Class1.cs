using System.IO;
using System.Linq;
using Templates.Tools;
using Xunit;

namespace dotnet_codegen.Tests
{
    public class Class1
    {
        [Fact]
        public void TestCodeGen()
        {
            var root = Path.Combine(new DirectoryInfo( Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName,"data");
            Program.Main(new[] { root });
            var files = Directory.GetFiles(root, "*.cs");
            Assert.Equal(2, files.Length);
        }
    }
}
