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

using System;

using AgileCardsPrinting.Common;
using AgileCardsPrinting.Interfaces;
using AgileCardsPrinting.ViewModels;

using DevExpress.Mvvm;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace AgileCardsPrinting.Tests
{
//    [TestFixture]
    public class MainViewModelTests
    {
        //[Test]
        //public void OpenSettingsTest()
        //{
        //    CreateWindowMessage message = null;

        //    var messager = new Mock<IMessenger>();
        //    var jira = new Mock<IJiraService>();
        //    messager.Setup(m => m.Send(It.IsAny<CreateWindowMessage>(), It.IsAny<Type>(), It.IsAny<object>()))
        //        .Callback<CreateWindowMessage, Type, object>((m, t, o) => message = m);

        //    var viewModel = new MainViewModel(messager.Object, jira.Object);
        //    viewModel.OpenSettings(typeof(object));

        //    message.Should().NotBeNull();
        //}
         
        //[Test]
        //public void RefreshFilterListTest()
        //{
        //    Assert.Fail();
        //}

        //[Test]
        //public void RefreshIssuesListTest()
        //{
        //    Assert.Fail();
        //}

        //[Test]
        //public void PreparePrintTest()
        //{
        //    Assert.Fail();
        //}






        //[Test]
        //public void SetPreviewIssues_With_Null_List_Tests()
        //{
        //    var messager = new Mock<IMessenger>();
        //    var jira = new Mock<IJiraService>();
        //    var viewModel = new MainViewModel(messager.Object, jira.Object);

        //    //viewModel.SetPreviewIssues(null);

        //    viewModel.PreviewIssues.Should().BeEmpty();
        //}

        //[Test]
        //public void SetPreviewIssues_With_Empty_List_Tests()
        //{
        //    var messager = new Mock<IMessenger>();
        //    var jira = new Mock<IJiraService>();
        //    var viewModel = new MainViewModel(messager.Object, jira.Object);

        //    var list = new List<JiraIssue>();
        //    //viewModel.SetPreviewIssues(list);

        //    viewModel.PreviewIssues.Should().BeEmpty();
        //}

        //[Test]
        //public void SetPreviewIssues_With_List_Tests()
        //{
        //    var messager = new Mock<IMessenger>();
        //    var jira = new Mock<IJiraService>();
        //    var viewModel = new MainViewModel(messager.Object, jira.Object);

        //    var list = new List<JiraIssue> { new JiraIssue(), new JiraIssue()};
        //    //viewModel.SetPreviewIssues(list);

        //    viewModel.PreviewIssues.Should().HaveCount(2);
        //}
    }
}
