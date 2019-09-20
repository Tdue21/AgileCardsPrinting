//  ****************************************************************************
//  * The MIT License(MIT)
//  * Copyright © 2017 Thomas Due
//  * 
//  * Permission is hereby granted, free of charge, to any person obtaining a 
//  * copy of this software and associated documentation files (the “Software”), 
//  * to deal in the Software without restriction, including without limitation 
//  * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//  * and/or sell copies of the Software, and to permit persons to whom the  
//  * Software is furnished to do so, subject to the following conditions:
//  * 
//  * The above copyright notice and this permission notice shall be included in  
//  * all copies or substantial portions of the Software.
//  * 
//  * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS  
//  * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL  
//  * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING  
//  * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//  * IN THE SOFTWARE.
//  ****************************************************************************

using System.IO;
using System.Text;

using AgileCardsPrinting.Common;
using AgileCardsPrinting.Interfaces;
using AgileCardsPrinting.Models;

using FluentAssertions;

using Moq;

using Newtonsoft.Json;

using NUnit.Framework;

namespace AgileCardsPrinting.Tests
{
	/// <summary>
	/// </summary>
	[TestFixture]
    public class SettingsHandlerTests
    {
	    /// <summary>
	    /// </summary>
	    private ISettingsHandler _handler;

        /// <summary>
        /// </summary>
        private Mock<IFileSystemService> _fileSystem;

        /// <summary>
        /// </summary>
        [SetUp]
        public void SetUpTest()
        {
            _fileSystem = new Mock<IFileSystemService>();
            _fileSystem.Setup(f => f.GetFullPath(It.IsAny<string>())).Returns(@"C:\Temp\Settings.json");
            _handler = new JsonFileSettingsHandler(_fileSystem.Object);
        }

        /// <summary>
        /// </summary>
        [Test]
        public void LoadSettings_Settings_File_Does_Not_Exist_Return_Empty_Settings_Test()
        {
            // Arrange
            _fileSystem.Setup(f => f.FileExists(It.IsAny<string>())).Returns(false);
            
            // Act
            var expected = new SettingsModel();
            var actual = _handler.LoadSettings();

            // Assert
            actual.Should()
                  .NotBeNull()
                  .And
                  .Be(expected);
        }

        /// <summary>
        /// </summary>
        [Test]
        public void LoadSettings_Settings_File_Exists_Return_Valid_Settings_Test()
        {
            // Arrange
            _fileSystem.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);
            _fileSystem.Setup(f => f.OpenReadStream(It.IsAny<string>()))
                       .Returns(new MemoryStream(Encoding.UTF8.GetBytes(GetSettingsString())));

            // Act
            var expected = GetSettingsModel();
            var actual = _handler.LoadSettings();

            // Assert
            actual.Should()
                  .NotBeNull()
                  .And
                  .Be(expected);
        }

        [Test]
        /// <summary>
        /// </summary>
        public void SaveSettings_Test()
        {
            string actual = null;

            // Arrange
            var stream = new Mock<Stream>();
            stream.Setup(s => s.Write(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                  .Callback<byte[], int, int>((a, i, l) => actual = Encoding.UTF8.GetString(a));

            _fileSystem.Setup(f => f.OpenWriteStream(It.IsAny<string>()))
                       .Returns(stream.Object);

            // Act
            var expected = GetSettingsString();
            var settings = GetSettingsModel();
            _handler.SaveSettings(settings);


            // Assert
            actual.Should()
                  .NotBeNull()
                  .And
                  .BeEquivalentTo(expected);
        }

		/// <summary>
		/// </summary>
        private static string GetSettingsString() => JsonConvert.SerializeObject(GetSettingsModel());

		/// <summary>
		/// </summary>
        private static SettingsModel GetSettingsModel() =>
			new SettingsModel
			{
				HostAddress  = "https://bugs.mojang.com",
				MaxResult    = 100,
				CustomField1 = @"ThisIsATest",
				ReportName   = @"SimpleReport.rdlc",
				UserId       = "user",
				Password     = string.Empty.ConvertToSecureString()
			};
    }
}

