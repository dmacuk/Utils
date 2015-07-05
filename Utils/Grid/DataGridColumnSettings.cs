using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Utils.Grid
{
    /// <summary>
    /// Save/restore the columns settings of a DataGrid
    /// </summary>
    public static class DataGridColumnSettings
    {

        /// <summary>
        /// Get the supplied Datagrid's column settings
        /// </summary>
        /// <param name="dg">A DataGrid</param>
        /// <returns></returns>
        public static string GetColumOptions(this DataGrid dg)
        {
            var columnProperties = dg.Columns.Select(column => new ColumnProperty(column.DisplayIndex, column.Width.DisplayValue, column.Visibility, column.SortDirection)).ToList();
            return JsonConvert.SerializeObject(columnProperties);
        }

        /// <summary>
        /// Set the passed DataGrid's columns settings based on the passed XML
        /// </summary>
        /// <param name="dg">A DataGrid</param>
        /// <param name="json">Column settings for the passed DataGrid. This is usually the String returned from <strong>GetColumOption</strong></param>
        public static void SetColumnOptions(this DataGrid dg, string json)
        {
            try
            {
                var columnProperties = JsonConvert.DeserializeObject<List<ColumnProperty>>(json);
                var columnCount = 0;
                foreach (var columnProperty in columnProperties)
                {
                    var column = dg.Columns[columnCount];
                    column.DisplayIndex = columnProperty.DisplayIndex;
                    column.Width = columnProperty.Width;
                    column.Visibility = columnProperty.Visibility;
                    if (columnProperty.ListSortDescription.HasValue)
                    {
                        column.SortDirection = columnProperty.ListSortDescription;
                        dg.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
                    }
                    columnCount++;
                }

            }
            catch (Exception e)
            {
                
                Debug.WriteLine(e.Message);
            }
        }
    }

    /// <summary>
    /// Holder for various DataGrid column properties
    /// </summary>
    class ColumnProperty
    {
#pragma warning disable 1591
        public ColumnProperty(int displayIndex, double width, Visibility visibility, ListSortDirection? listSortDirection)
        {
            DisplayIndex = displayIndex;
            Width = width;
            Visibility = visibility;
            ListSortDescription = listSortDirection;
        }

        public int DisplayIndex { get; set; }
        public double Width { get; set; }
        public Visibility Visibility { get; set; }
        public ListSortDirection? ListSortDescription { get; set; }
#pragma warning restore 1591



        /*
         *             colTable.Columns.Add(ColDisplayindex, typeof (Int32));
            colTable.Columns.Add(ColWidth, typeof (double));
            colTable.Columns.Add(ColVisibility, typeof (Int32));
            colTable.Columns.Add(ColSortdirection, typeof (Int32));

         */

    }
}
