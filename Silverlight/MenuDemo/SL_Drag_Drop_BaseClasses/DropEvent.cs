/* Kevin Dockx
 * 
 * Drop Event arguments
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
    /// Delegate for creating drop events
    /// </summary>
    /// <param name="sender">The drop sender.</param>
    /// <param name="args">Drop event args.</param>
    public delegate void DropEventHandler(object sender, DropEventArgs args);

    /// <summary>
    /// Class defining the drop event arguments
    /// </summary>
    public class DropEventArgs: EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DropEventArgs()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        public DropEventArgs(DragSource source)
        {
            DragSource = source;
        }

        /// <summary>
        /// Contains the dragsource being dropped
        /// </summary>
        public DragSource DragSource { get; set; }
    }
}
