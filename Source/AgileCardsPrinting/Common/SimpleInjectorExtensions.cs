//  ****************************************************************************
//  * The MIT License(MIT)
//  * Copyright � 2017 Thomas Due
//  * 
//  * Permission is hereby granted, free of charge, to any person obtaining a 
//  * copy of this software and associated documentation files (the �Software�), 
//  * to deal in the Software without restriction, including without limitation 
//  * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//  * and/or sell copies of the Software, and to permit persons to whom the  
//  * Software is furnished to do so, subject to the following conditions:
//  * 
//  * The above copyright notice and this permission notice shall be included in  
//  * all copies or substantial portions of the Software.
//  * 
//  * THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS  
//  * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL  
//  * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING  
//  * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
//  * IN THE SOFTWARE.
//  ****************************************************************************

// ReSharper disable InvalidXmlDocComment

namespace AgileCardsPrinting.Common
{
    /// <summary>Static class for various extension methods.</summary>
    public static class SimpleInjectorExtensions
    {
        /// <summary>Extension method for SimpleInjector to enable Fluent API. Used in place of
        /// the <see cref="SimpleInjector.Container.Register{TConcrete}" /> method.</summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        public static SimpleInjector.Container RegisterType<TType>(this SimpleInjector.Container container) 
            where TType : class
        {
            container.Register<TType>();
            return container;
        }

        /// <summary>Extension method for SimpleInjector to enable Fluent API. Used in place of the
        /// <see cref="SimpleInjector.Container.Register{TService, TImplementation}"/> method.</summary>
        /// <param name="container"></param>
        /// <param name="lifestyle"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <returns></returns>
        public static SimpleInjector.Container RegisterType<TSource, TImplementation>(this SimpleInjector.Container container, SimpleInjector.Lifestyle lifestyle)
            where TSource : class
            where TImplementation : class, TSource
        {
            container.Register<TSource, TImplementation>(lifestyle);
            return container;
        }
    }
}
