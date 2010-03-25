/* Kevin Dockx
 * 
 * Project@CodePlex: http://www.codeplex.com/silverlightdragdrop/
 * Author site: http://kevindockx.blogspot.com/
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
    /// Initial values for Drag Drop Manager
    /// </summary>
    public class InitialValues
    {

        /// <summary>
        /// This property contains the Containing Layout Panel, used to correctly position DragSources
        /// when hovering and to make sure they are always on top.  You'd typically set this once, in your
        /// Page constructor, to the surrounding LayoutRoot of your application.
        /// </summary>
        public static Panel ContainingLayoutPanel { get; set; }
        
    }
}
