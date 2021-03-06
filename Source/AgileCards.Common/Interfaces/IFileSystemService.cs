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

namespace AgileCards.Common.Interfaces
{
	/// <summary>An <see langword="interface"/> for wrapping the file system.</summary>
	public interface IFileSystemService
	{
		/// <summary>
		/// Gets the full path.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		string GetFullPath(string path);

		/// <summary>
		/// Files the exists.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		bool FileExists(string path);

		/// <summary>
		/// Opens the read stream.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		Stream OpenReadStream(string path);

		/// <summary>
		/// Opens the write stream.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		Stream OpenWriteStream(string path);

		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns></returns>
		string GetFileName(string path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetFileNameWithoutExtension(string path);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reportPath"></param>
		/// <param name="mask"></param>
		/// <returns></returns>
		IEnumerable<string> GetFilesFrom(string reportPath, string mask);
	}
}
