﻿//  ****************************************************************************
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

using System.Collections.Generic;
using System.IO;
using AgileCards.Common.Interfaces;

namespace AgileCards.Common.Services
{
	/// <summary>
	/// An abstraction layer class for the local file system.  
	/// </summary>
	public class FileSystemService : IFileSystemService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string GetFullPath(string path) => Path.GetFullPath(path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool FileExists(string path) => File.Exists(path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public Stream OpenReadStream(string path) => File.Open(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public Stream OpenWriteStream(string path) => File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string GetFileName(string path) => Path.GetFileName(path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string GetFileNameWithoutExtension(string path) => Path.GetFileNameWithoutExtension(path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reportPath"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		public IEnumerable<string> GetFilesFrom(string reportPath, string mask) => Directory.GetFiles(reportPath, mask, SearchOption.TopDirectoryOnly);
	}
}
