/* Kevin Dockx
 * 
 * DragEvent arguments
 * 
 */

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SL_Drag_Drop_BaseClasses
{
    /// <summary>
    /// Delegate for creating drag events
    /// </summary>
    /// <param name="sender">The dragging sender.</param>
    /// <param name="args">Drag event args.</param>
    public delegate void DragEventHandler(object sender, DragEventArgs args);

    /// <summary>
    /// Class defining the drag event arguments
    /// </summary>
    public class DragEventArgs : EventArgs
    {
        /// <summary>
        /// Blank Constuctor
        /// </summary>
        public DragEventArgs()
        {
        }

        /// <summary>
        /// Contstructor with bits
        /// </summary>
        /// <param name="horizontalChange">Horizontal change</param>
        /// <param name="verticalChange">Vertical change</param>
        /// <param name="mouseEventArgs">The mouse event args</param>
        public DragEventArgs(double horizontalChange, double verticalChange, MouseEventArgs mouseEventArgs)
        {
            this.HorizontalChange = horizontalChange;
            this.VerticalChange = verticalChange;
            this.MouseEventArgs = mouseEventArgs;
        }


        /// <summary>
        /// Gets or sets the horizontal change of the drag
        /// </summary>
        public double HorizontalChange{get; set;}

        /// <summary>
        /// Gets or sets the vertical change of the drag
        /// </summary>
        public double VerticalChange{get; set;}

        /// <summary>
        /// Gets or sets the mouse event args
        /// </summary>
        public MouseEventArgs MouseEventArgs{get; set;}

    }
}
