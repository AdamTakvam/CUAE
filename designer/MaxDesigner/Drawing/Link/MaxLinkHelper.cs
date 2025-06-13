using System;
using System.Xml;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Framework.Satellite.Property;
using Crownwood.Magic.Menus;
using Northwoods.Go;



namespace Metreos.Max.Drawing
{
    /// <summary>Helper class to assuage pain of uncommon link inheritance</summary>
    public class MaxLinkHelper
    {
        public MaxLinkHelper(IMaxLink link, GoLink golink, GoStroke stroke)
        {
            this.link = link; this.golink = golink; this.stroke = stroke;

            this.CreateMenus();
        }


        /// <summary>Set common properties of a link</summary>
        public void Init()
        {
            link.LinkWidth    = Config.UserLinkWidth;
            link.LinkColor    = Config.linkColorNormal;
            link.IsOrthogonal = Config.UserLinkOrtho;

            if (link.LinkStyle == LinkStyles.None)
                link.LinkStyle = Config.UserLinkStyle;

            switch(link.LinkStyle)
            {
                case LinkStyles.Bezier: golink.Style = GoStrokeStyle.Bezier;      break;
                case LinkStyles.Vector: golink.Style = GoStrokeStyle.Line;        break;
                case LinkStyles.Bevel:  golink.Style = GoStrokeStyle.RoundedLine; break;
            }

            golink.Orthogonal   = link.IsOrthogonal;
            golink.Pen          = new Pen(link.LinkColor, link.LinkWidth);
            golink.Reshapable   = true;
            golink.Brush        = new SolidBrush(link.LinkColor);
            golink.ToArrow      = true;
            if (link.LinkWidth == 1) golink.ToArrowWidth -= 2;
            stroke.HighlightPen = new Pen(Config.linkHighlight, golink.Pen.Width+2);
            stroke.HighlightWhenSelected = true;
        }


        /// <summary>Display a context menu for the link</summary>
        public void PopLinkContextMenu()    
        {
            Crownwood.Magic.Menus.PopupMenu popup = new Crownwood.Magic.Menus.PopupMenu();

            popup.MenuCommands.AddRange(ContextMenuCommands);

            mcbCurveStyle.Checked = link.LinkStyle == LinkStyles.Bezier;
            mcbLineStyle.Checked  = link.LinkStyle == LinkStyles.Vector;
            mcbBevelStyle.Checked = link.LinkStyle == LinkStyles.Bevel;
            mcaSegmented.Checked  = link.IsOrthogonal;
  
            CancelEventArgs args = new CancelEventArgs(false);
            IMaxNode node = link.Node;      // Check with link's source node
            if (node != null)               // as to whether OK to delete link
                node.CanDeleteLink(link, args); 
            mcaDelete.Enabled = !args.Cancel;
      
            popup.TrackPopup(Control.MousePosition);
        }


        public void UpdateLink(IMaxLink link) { this.link = link; }


        public void OnMenuProperties(object sender, EventArgs e)
        {
            PmProxy.ShowProperties(link, link.PmObjectType);
        }


        /// <summary>Delete selected from context menu so delete link</summary>
        public void OnMenuDelete(object sender, EventArgs e)
        {
            this.link.Node.OnLinkDeleted(this.link);
        }


        public void OnMenuLinkSegmented(object sender, EventArgs e)
        {
            link.IsOrthogonal = !link.IsOrthogonal;   
            golink.Orthogonal =  link.IsOrthogonal;
            golink.CalculateStroke();

            if (!link.IsOrthogonal && link.LinkStyle == LinkStyles.Bevel)
            {    // Changing orthogonality causes link style to degrade to line
                link.LinkStyle = LinkStyles.Vector;
                golink.Style   = GoStrokeStyle.Line;
            }
        }


        public void OnMenuCurveStyle(object sender, EventArgs e)
        {
            if (link.LinkStyle == LinkStyles.Bezier) return;
            link.LinkStyle = LinkStyles.Bezier;
            golink.Style   = GoStrokeStyle.Bezier; 
            golink.CalculateStroke();
        }


        public void OnMenuLineStyle(object sender, EventArgs e)
        {
            if (link.LinkStyle == LinkStyles.Vector) return;
            link.LinkStyle = LinkStyles.Vector;
            golink.Style   = GoStrokeStyle.Line;
            golink.CalculateStroke();
        }


        public void OnMenuBevelStyle(object sender, EventArgs e)
        {
            if (link.LinkStyle == LinkStyles.Bevel) return;
            link.LinkStyle = LinkStyles.Bevel;
            golink.Style   = GoStrokeStyle.RoundedLine;
            link.IsOrthogonal = golink.Orthogonal = true;
            golink.CalculateStroke();
        }


        private void CreateMenus()
        {
            mcaProps      = new MenuCommand(Const.menuGenericProperties, new EventHandler(OnMenuProperties));
            mcaDelete     = new MenuCommand(Const.menuGenericDelete,     new EventHandler(OnMenuDelete)); 
            separator     = new MenuCommand(Const.dash);
            mcaLinkStyle  = new MenuCommand(Const.menuLinkLinkStyle);
            mcaSegmented  = new MenuCommand(Const.menuLinkLinkOrthogonal,new EventHandler(OnMenuLinkSegmented));
            mcbCurveStyle = new MenuCommand(Const.menuLinkLinkCurve,     new EventHandler(OnMenuCurveStyle));
            mcbLineStyle  = new MenuCommand(Const.menuLinkLinkLine,      new EventHandler(OnMenuLineStyle));
            mcbBevelStyle = new MenuCommand(Const.menuLinkLinkRounded,   new EventHandler(OnMenuBevelStyle));

            MenuCommand[] submenuCommands = new MenuCommand[] 
            {
                mcbCurveStyle, mcbLineStyle, mcbBevelStyle
            }; 
            mcaLinkStyle.MenuCommands.AddRange(submenuCommands); 

            ContextMenuCommands = new MenuCommand[]
            {
                mcaDelete, separator, mcaLinkStyle, mcaSegmented, separator, mcaProps 
            };
        }

        private IMaxLink link;
        private GoLink   golink;
        private GoStroke stroke;

        public MenuCommand mcaProps;
        public MenuCommand mcaDelete;
        public MenuCommand separator;
        public MenuCommand mcaLinkStyle;
        public MenuCommand mcaSegmented;

        public MenuCommand mcbCurveStyle;
        public MenuCommand mcbLineStyle;
        public MenuCommand mcbBevelStyle;

        public MenuCommand[] ContextMenuCommands;

    } // class MaxLinkHelper

} // namespace
