// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Teronis.ObjectModel.TreeColumn;

namespace Teronis.Windows.PresentationFoundation.Data.TreeColumn
{
    public class ListViewTreeColumnKey : HeaderTreeColumnKey
    {
        /// <summary>
        /// Create a binding with a passed binding path. Can be omitted.
        /// </summary>
        public Func<string, Binding> GetBinding { get; set; }

        ///// <summary>
        ///// The horizontal alignment will be applied on the column.
        ///// </summary>
        //public HorizontalAlignment? HorizontalAlignment { get; set; }

        public Func<Binding, DataTemplate> GetCellTemplate { get; set; }

        /// <summary>
        /// Create a data template selector with a passed binding.
        /// </summary>
        public Func<Binding, DataTemplateSelector> GetCellTemplateSelector { get; set; }

        public ListViewTreeColumnKey(Type declarationType, string variableName) : base(declarationType, variableName) { }

        public ListViewTreeColumnKey(Type declarationType, string variableName, string header) : base(declarationType, variableName, header) { }
    }
}
