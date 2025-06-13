using System;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Metreos.Max.Core;
using Metreos.Max.Framework.Satellite.Property;
using Northwoods.Go;
using Crownwood.Magic.Menus;



namespace Metreos.Max.Drawing
{	
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxActionLink
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxActionLink: MaxLabeledLink
    {
        public MaxActionLink()
        {     
        }

        public MaxActionLink(MaxCanvas canvas, IMaxNode node, string labeltext):
        base(canvas, node, labeltext)
        {     
        }

        public MaxActionLink(MaxCanvas canvas, IMaxNode node, string labeltext, LinkStyles linkStyle):
        base(canvas, node, labeltext, linkStyle)
        {
        }
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // MaxActionLinkLabel
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public class MaxActionLinkLabel: GoText 
    {
        public event Max.Drawing.MaxLinkLabelChanged LinkLabelChangedEvent;

        public MaxActionLinkLabel(MaxActionLink link, bool isArbitraryValueOK, 
        ArrayList choices, string defaultValue) 
        {
            this.link = link;               // Parent link
            this.specifiedDefaultText = defaultValue;
            this.Text = defaultValue == null? Const.initialActionLinkLabelText: defaultValue;
            this.Editable     = true;
            this.TextColor    = Const.linkLabelColor;
            this.EditorStyle  = GoTextEditorStyle.ComboBox;
            this.Choices      = choices;
            this.DropDownList = !isArbitraryValueOK;
            if  (this.DropDownList) this.Choices.Add(this.Text);
            mcRename = new MenuCommand(Const.menuGenericRename, new EventHandler(OnMenuRename));

            // Register callback to label's link's source node to validate label text change
            MaxActionNode node = link.Node as MaxActionNode;
            if (node != null)
                this.LinkLabelChangedEvent += node.LinkLabelChangeCallback; 
        } 


        public override void Changed
        ( int subhint, int oI, object ov, RectangleF or, int nI, object nv, RectangleF nr) 
        {   
            switch(subhint)
            { 
                case GoText.ChangedText:       
                     this.OnTextChanged(ov as string, nv as string);                 
                     break;

                default:
                     base.Changed(subhint, oI, ov, or, nI, nv, nr);
                     break;
            }    
        }


        private void OnTextChanged(string oldval, string newval)
        {
            if (newval == null) return;

            if (isMarkingUnconditional)     // Intercept recursive change
            {    
                isMarkingUnconditional = false;
                return;
            }

            if (newval.Length == 0 || newval.StartsWith(Const.blank))
                this.Text = this.specifiedDefaultText == null? 
                    Const.initialActionLinkLabelText: this.specifiedDefaultText; 
                                            // Change "unconditional" to nothing
            if (Config.EnableUnconditionalLinks && this.Text == Const.LinkLabelUnconditional)
            {    
                isMarkingUnconditional = true;
                this.Text = Const.emptystr; // Cause recursion
            }
                                            // Fire label text change event
            if (this.link != null && oldval != null && this.LinkLabelChangedEvent != null)     
                this.LinkLabelChangedEvent(this, oldval, newval);
        }


        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        { 
            // Do nothing in order to not show a focus rect for the link label
        }

        public  override bool CanDelete() { return false; }

        public override bool OnContextClick(GoInputEventArgs evt, GoView view)
        {
            PopupMenu popup = new PopupMenu();
            popup.MenuCommands.Add(mcRename);
            mcRename.Tag = this;      
            popup.TrackPopup(Control.MousePosition);
            return true;
        }

        public void OnMenuRename(object sender, EventArgs e)
        {
            this.DoBeginEdit(this.link.Node.Canvas.View);
        }

        private string specifiedDefaultText;  
        private MaxActionLink link; 
        private MenuCommand mcRename; 
        private bool isMarkingUnconditional;

    } // class MaxActionLinkLabel

} // namespace
