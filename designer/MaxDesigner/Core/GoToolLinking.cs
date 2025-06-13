/*
 *  Copyright © Northwoods Software Corporation, 1998-2003. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 *
 *  This software is proprietary to and embodies the confidential
 *  technology of Northwoods Software Corporation. Possession, use, or
 *  copying of this software and media is authorized only pursuant to a
 *  valid written license from Northwoods or an authorized sublicensor.
 */
#define WINFORMS
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using Northwoods.Go;


#if WINFORMS
//namespace Northwoods.Go {
namespace Metreos.Max.Core {
#elif WEBFORMS
namespace Northwoods.GoWeb {
#endif
  /// <summary>
  /// The tool used to implement the user's drawing a new link or reconnecting an existing link.
  /// </summary>
  [Serializable]
  public abstract class GoToolLinking : GoTool 
  {
    /// <summary>
    /// The standard tool constructor.
    /// </summary>
    /// <param name="v"></param>
    protected GoToolLinking(GoView v) : base(v) {}
    
    /// <summary>
    /// Cleaning up from any kind of linking operation involves
    /// removing any temporary ports or link from the view and
    /// stopping the current transaction.
    /// </summary>
    public override void Stop() 
    {

#if WINFORMS  // autoscroll
      this.View.StopAutoScroll();
#endif

      this.Forwards = true;
      this.OriginalStartPort = null;
      this.OriginalEndPort = null;
      // if the link or either temporary port were added to a view, remove it
      if (this.Link != null) {
        GoObject obj = this.Link.GoObject;
        if (obj != null && obj.IsInView)
          obj.Remove();
      }
      this.Link = null;
      if (this.StartPort != null) {
        GoObject obj = this.StartPort.GoObject;
        if (obj != null && obj.IsInView)
          obj.Remove();
      }
      this.StartPort = null;
      if (this.EndPort != null) {
        GoObject obj = this.EndPort.GoObject;
        if (obj != null && obj.IsInView)
          obj.Remove();
      }
      this.EndPort = null;
      if (this.ValidPortsCache != null) {
        this.ValidPortsCache.Clear();
      }

      StopTransaction();
    }

    /// <summary>
    /// A mouse move during a linking operation involves
    /// calling <see cref="DoLinking"/> and autoscrolling the view.
    /// </summary>
    public override void DoMouseMove() {
      DoLinking(this.LastInput.DocPoint);
#if WINFORMS  // DoAutoScroll
      this.View.DoAutoScroll(this.LastInput.ViewPoint);
#endif
    }

    /// <summary>
    /// A mouse up event ends the linking operation.
    /// </summary>
    /// <remarks>
    /// Depending on whether the user is drawing a new link or relinking,
    /// whether the user is drawing <see cref="Forwards"/> or not, and
    /// whether <see cref="PickNearestPort"/> found a valid port at
    /// a reasonable distance, this method will call either
    /// <see cref="DoNewLink"/>, <see cref="DoRelink"/>,
    /// <see cref="DoNoNewLink"/>, or <see cref="DoNoRelink"/>.
    /// </remarks>
    public override void DoMouseUp() {
      IGoPort port = PickNearestPort(this.LastInput.DocPoint);

      if (port != null) {
        if (myLinkingNew) {
          if (this.Forwards)
            DoNewLink(this.OriginalStartPort, port);
          else
            DoNewLink(port, this.OriginalStartPort);
        } else {
          if (this.Forwards)
            DoRelink(this.Link, this.OriginalStartPort, port);
          else
            DoRelink(this.Link, port, this.OriginalStartPort);
        }
      } else {
        IGoPort invalidPort = PickPort(this.LastInput.DocPoint);
        if (myLinkingNew) {
          if (this.Forwards)
            DoNoNewLink(this.OriginalStartPort, invalidPort);
          else
            DoNoNewLink(invalidPort, this.OriginalStartPort);
        } else {
          if (this.Forwards)
            DoNoRelink(this.Link, this.OriginalStartPort, invalidPort);
          else
            DoNoRelink(this.Link, invalidPort, this.OriginalStartPort);
        }
      }
      StopTool();
    }

    /// <summary>
    /// Clean up the link state before stopping this tool.
    /// </summary>
    /// <remarks>
    /// Depending on whether the user is drawing a new link or relinking,
    /// whether the user is drawing <see cref="Forwards"/> or not,
    /// this method will call either
    /// <see cref="DoNoNewLink"/>, <see cref="DoNoRelink"/>,
    /// or <see cref="DoCancelRelink"/>.
    /// </remarks>
    public override void DoCancelMouse() {
      if (myLinkingNew) {
        if (this.Forwards)
          DoNoNewLink(this.StartPort, null);
        else
          DoNoNewLink(null, this.StartPort);
      } else if (this.OriginalEndPort == null) {
        if (this.Forwards)
          DoNoRelink(this.Link, this.StartPort, null);
        else
          DoNoRelink(this.Link, null, this.StartPort);
      } else {
        if (this.Forwards)
          DoCancelRelink(this.Link, this.OriginalStartPort, this.OriginalEndPort);
        else
          DoCancelRelink(this.Link, this.OriginalEndPort, this.OriginalStartPort);
      }
#if WINFORMS && !POCKET  // System.Windows.Forms.Cursor
      this.View.Cursor = this.View.DefaultCursor;
#endif
      StopTool();
    }


    /// <summary>
    /// Find a port in the document at the given point.
    /// </summary>
    /// <param name="dc">a <c>PointF</c> in document coordinates</param>
    /// <returns>an <see cref="IGoPort"/>, or null if none was found at <paramref name="dc"/></returns>
    public virtual IGoPort PickPort(PointF dc) {
      GoObject obj = this.View.PickObject(true, false, dc, false);  // ports in documents; selectable doesn't matter
      return obj as IGoPort;
    }

    /// <summary>
    /// Start the process of drawing a new link from a given port.
    /// </summary>
    /// <param name="port"></param>
    /// <param name="dc"></param>
    /// <remarks>
    /// If <see cref="IsValidFromPort"/> is true, the user will be
    /// linking in the <see cref="Forwards"/> direction--i.e. from the
    /// source to the destination.
    /// This method calls <see cref="CreateTemporaryPort"/> to create both
    /// the <see cref="StartPort"/> and the <see cref="EndPort"/>, and
    /// it calls <see cref="CreateTemporaryLink"/> to create the <see cref="Link"/>.
    /// This starts a transaction.
    /// </remarks>
    public virtual void StartNewLink(IGoPort port, PointF dc) {
      if (port == null) return;

      StartTransaction();
      myLinkingNew = true;

      // By default, if where we started was a valid source port,
      // we are now creating a link with that port as the FROM port.
      // Only if it is not a valid source port do we draw the link backwards,
      // starting with the TO port.
      if (IsValidFromPort(port)) {
        this.Forwards = true;
        this.StartPort = CreateTemporaryPort(port, port.GoObject.Center, false, false);
        this.EndPort = CreateTemporaryPort(port, dc, true, true);
        this.Link = CreateTemporaryLink(this.StartPort, this.EndPort);
      } else {
        this.Forwards = false;
        this.StartPort = CreateTemporaryPort(port, port.GoObject.Center, true, false);
        this.EndPort = CreateTemporaryPort(port, dc, false, true);
        this.Link = CreateTemporaryLink(this.EndPort, this.StartPort);
      }
#if WINFORMS && !POCKET  // System.Windows.Forms.Cursor
      this.View.Cursor = System.Windows.Forms.Cursors.Hand;
#endif
    }

    /// <summary>
    /// This predicate is called to decide if it is OK for a user to start
    /// drawing a link from a given port.
    /// </summary>
    /// <param name="fromPort"></param>
    /// <returns>By default this returns the result of calling <see cref="IGoPort.CanLinkFrom"/></returns>
    public virtual bool IsValidFromPort(IGoPort fromPort) {
      return fromPort.CanLinkFrom();
    }

    /// <summary>
    /// This predicate is called to decide if it is OK for a user to start
    /// drawing a link backwards at a given port that will be the destination
    /// for the link.
    /// </summary>
    /// <param name="toPort"></param>
    /// <returns>By default this is true if <see cref="ForwardsOnly"/> is false
    /// <see cref="IGoPort.CanLinkTo"/> is true</returns>
    public virtual bool IsValidToPort(IGoPort toPort) {
      return !this.ForwardsOnly && toPort.CanLinkTo();
    }

    /// <summary>
    /// Start the process of reconnecting an existing link at a given port.
    /// </summary>
    /// <param name="oldlink"></param>
    /// <param name="oldport"></param>
    /// <param name="dc"></param>
    /// <remarks>
    /// This starts a transaction.
    /// </remarks>
    public virtual void StartRelink(IGoLink oldlink, IGoPort oldport, PointF dc) {
      // if the OLDLINK is not part of a document, and if it is not
      // disconnected at one end, and if OLDPORT is not in the same document,
      // forget it
      if (oldlink == null) return;
      if (oldport == null) return;

      GoObject l = oldlink.GoObject;
      GoObject p = oldport.GoObject;
      if (l == null ||
          l.Layer == null ||
          (p != null &&
           (p.Layer == null ||
            l.Document != p.Document))) {
        return;
      }

      // don't need to check if ports are valid source or destination for
      // starting to draw a new link, because we already have a link that's
      // been started--we just need to finish drawing it

      StartTransaction();
      myLinkingNew = false;

      // remember the originally connected port, in case the user cancels
      this.OriginalEndPort = oldport;

      // this is the original link that is going to be continuously redrawn
      // as the user drags around the temporary port
      this.Link = oldlink;

      if (oldlink.ToPort == oldport) {
        this.Forwards = true;
        this.OriginalStartPort = oldlink.FromPort;
        this.StartPort = CreateTemporaryPort(this.OriginalStartPort, oldlink.FromPort.GoObject.Center, false, false);
        oldlink.FromPort = this.StartPort;
        this.EndPort = CreateTemporaryPort(this.OriginalEndPort, dc, true, true);
        oldlink.ToPort = this.EndPort;
      } else if (oldlink.FromPort == oldport) {
        this.Forwards = false;
        this.OriginalStartPort = oldlink.ToPort;
        this.StartPort = CreateTemporaryPort(this.OriginalStartPort, oldlink.ToPort.GoObject.Center, true, false);
        oldlink.ToPort = this.StartPort;
        this.EndPort = CreateTemporaryPort(this.OriginalEndPort, dc, false, true);
        oldlink.FromPort = this.EndPort;
      }
#if WINFORMS && !POCKET  // System.Windows.Forms.Cursor
      this.View.Cursor = System.Windows.Forms.Cursors.Hand;
#endif
    }

    /// <summary>
    /// This is responsible for creating a temporary port for the linking process.
    /// </summary>
    /// <param name="port">an existing port that the temporary port should be like; this may be null</param>
    /// <param name="pnt">the <c>PointF</c> in document coordinates for where the temporary port should be</param>
    /// <param name="forToPort">true if this is meant to be the <see cref="IGoLink.ToPort"/>
    /// instead of the <see cref="IGoLink.FromPort"/></param>
    /// <param name="atEnd">true if this is meant to be the <see cref="EndPort"/>
    /// instead of <see cref="StartPort"/></param>
    /// <returns>a <see cref="GoPort"/> in the view at <paramref name="pnt"/></returns>
    /// <remarks>
    /// This creates a new <see cref="GoPort"/> that is similar to the <paramref name="port"/>.
    /// By default the temporary port's <see cref="GoPort.Style"/> is <see cref="GoPortStyle.None"/>,
    /// so that it is not seen by the user.
    /// It is added to the default layer of the view.
    /// </remarks>
    protected virtual IGoPort CreateTemporaryPort(IGoPort port, PointF pnt, bool forToPort, bool atEnd) {
      GoPort tempPort = new GoPort();
      if (port.GoObject is GoPort) {
        GoPort origport = (GoPort)port.GoObject;
        tempPort.FromSpot = origport.FromSpot;
        tempPort.ToSpot = origport.ToSpot;
        tempPort.PortObject = origport.PortObject;
        tempPort.Size = origport.Size;
      } else {
        tempPort.FromSpot = GoObject.NoSpot;
        tempPort.ToSpot = GoObject.NoSpot;
        tempPort.Size = new SizeF();
      }
      tempPort.Center = pnt;  // follows mouse movement
      // these ports are added to the view, but are hidden
      tempPort.Style = GoPortStyle.None;
      this.View.Layers.Default.Add(tempPort);
      return tempPort;
    }

    /// <summary>
    /// This is responsible for creating a temporary link when the user is drawing a new link.
    /// </summary>
    /// <param name="fromPort"></param>
    /// <param name="toPort"></param>
    /// <returns>a <see cref="GoLink"/> in the view</returns>
    /// <remarks>
    /// By default this just creates a <see cref="GoView.NewLinkClass"/> instance
    /// using <paramref name="fromPort"/> and <paramref name="toPort"/> as the ends of the link.
    /// The temporary link is added to the default layer of the view, but unlike
    /// temporary ports, is visible.
    /// </remarks>
    protected virtual IGoLink CreateTemporaryLink(IGoPort fromPort, IGoPort toPort) {
      Type linktype = this.View.NewLinkClass;
      IGoLink tempLink = (IGoLink)Activator.CreateInstance(linktype);
      if (tempLink != null && tempLink.GoObject != null) {
        tempLink.FromPort = fromPort;
        tempLink.ToPort = toPort;
        GoObject linkobj = tempLink.GoObject;
        if (linkobj is GoLink) {
          GoLink l = (GoLink)linkobj;
          l.Orthogonal = this.Orthogonal;
          l.AdjustingStyle = GoLinkAdjustingStyle.Calculate;
        } else if (linkobj is GoLabeledLink) {
          GoLabeledLink ll = (GoLabeledLink)linkobj;
          ll.Orthogonal = this.Orthogonal;
          ll.AdjustingStyle = GoLinkAdjustingStyle.Calculate;
        }
        // these links exist in the view
        this.View.Layers.Default.Add(linkobj);
        return tempLink;
      } else {
        return null;
      }
    }

    /// <summary>
    /// This method is called by <see cref="DoMouseMove"/> to find the nearest
    /// valid port and adjust the temporary link according to where the given point is.
    /// </summary>
    /// <param name="dc">a <c>PointF</c> in document coordinates, the mouse position</param>
    /// <remarks>
    /// This calls <see cref="PickNearestPort"/> to find the closest valid port
    /// that we might link to.
    /// </remarks>
    public virtual void DoLinking(PointF dc) {
      if (this.EndPort == null) return;
      GoObject tempObj = this.EndPort.GoObject;
      if (tempObj == null) return;
      GoPort tempPort = tempObj as GoPort;

      RectangleF newrect;
      IGoPort iport = PickNearestPort(dc);
      if (iport != null) {
        GoObject pobj = iport.GoObject;
        if (pobj == null) return;
        GoPort p = pobj as GoPort;
        if (p != null) {
          // have the temporary port act like the real one it might be connected to
          if (tempPort != null) {
            tempPort.FromSpot = p.FromSpot;
            tempPort.ToSpot = p.ToSpot;
            tempPort.PortObject = p.PortObject;
          }
          newrect = p.Bounds;
        } else {
          if (tempPort != null) {
            tempPort.FromSpot = GoObject.NoSpot;
            tempPort.ToSpot = GoObject.NoSpot;
            tempPort.PortObject = null;
          }
          newrect = pobj.Bounds;
        }
      } else {
        if (tempPort != null) {
          tempPort.FromSpot = GoObject.NoSpot;
          tempPort.ToSpot = GoObject.NoSpot;
          tempPort.PortObject = null;
        }
        newrect = new RectangleF(dc.X, dc.Y, 0, 0);
      }
      tempObj.Bounds = newrect;
    }

    /// <summary>
    /// This predicate is called during the process of finding the nearest port
    /// that the user can link to.
    /// </summary>
    /// <param name="fromPort"></param>
    /// <param name="toPort"></param>
    /// <returns>By default this is implemented as <c>fromPort.IsValidLink(toPort)</c></returns>
    /// <remarks>
    /// The results of these calls are stored in the <see cref="ValidPortsCache"/>
    /// hash table.  The ports are associated with <see cref="Valid"/> or <see cref="Invalid"/>
    /// values depending on whether this predicate returned true or false.
    /// Note that to check for links from a port to itself, this predicate may
    /// be called with the same value for both arguments.
    /// </remarks>
    public virtual bool IsValidLink(IGoPort fromPort, IGoPort toPort) {
      return fromPort != null && toPort != null && fromPort.IsValidLink(toPort);
    }

    /// <summary>
    /// Find the valid document port nearest to a given point.
    /// </summary>
    /// <param name="dc">a <c>PointF</c> in document coordinates</param>
    /// <returns></returns>
    /// <remarks>
    /// A nearby port (as determined by the distance between <paramref name="dc"/>
    /// and the result of <see cref="PortPoint"/>) must be within
    /// the <see cref="GoView.PortGravity"/> distance for it to qualify,
    /// and it must be in a <see cref="GoLayer"/> that is visible.
    /// This uses the <see cref="IsValidLink"/> predicate, passing it the
    /// <see cref="OriginalStartPort"/> along with each port to be considered.
    /// The results of <see cref="IsValidLink"/> are cached in <see cref="ValidPortsCache"/>,
    /// for the cases where determining valid links is computationally expensive.
    /// This cache is valid only for the duration of this linking tool as the
    /// current tool; it is cleared each time this tool is stopped.
    /// </remarks>
    public virtual IGoPort PickNearestPort(PointF dc) {
      IGoPort bestPort = null;
      float currentMaxDist = this.View.PortGravity;
      currentMaxDist *= currentMaxDist;  // square here so don't need to sqrt later

      foreach (GoLayer layer in this.View.Layers.Backwards) {
        if (!layer.IsInDocument) continue;
        if (!layer.CanViewObjects()) continue;
        foreach (GoObject obj in layer.Backwards) {
          bestPort = pickNearestPort1(obj, dc, bestPort, ref currentMaxDist);
        }
      }
      return bestPort;
    }

    private IGoPort pickNearestPort1(GoObject obj, PointF dc, IGoPort bestPort, ref float bestDist) {
      // remember that it's possible some object will implement both IGoPort and GoGroup
      IGoPort port = obj as IGoPort;
      if (port != null) {
        PointF toPoint = PortPoint(port, dc);
        float dx = dc.X - toPoint.X;
        float dy = dc.Y - toPoint.Y;
        float dist = dx*dx + dy*dy;  // don't bother taking sqrt
        if (dist <= bestDist) {  // closest so far
          Object cached = null;
          if (this.ValidPortsCache != null) {
            cached = this.ValidPortsCache[port];
            // will be either null (not previously seen), true, or false
          }
          if (cached == Valid) {
            // known to be a valid port for a link
            bestPort = port;
            bestDist = dist;
          } else if (cached == Invalid) { // known not Valid, don't need to call IsValidLink again
            // but if cached == null, try IsValidLink in the appropriate direction
          } else if ((this.Forwards && IsValidLink(this.OriginalStartPort, port)) ||
                     (!this.Forwards && IsValidLink(port, this.OriginalStartPort))) {
            // known Valid, remember in cache
            if (this.ValidPortsCache != null) {
              this.ValidPortsCache[port] = Valid;
            }
            bestPort = port;
            bestDist = dist;
          } else {
            // known not Valid, remember in cache
            if (this.ValidPortsCache != null) {
              this.ValidPortsCache[port] = Invalid;
            }
          }
        }
      }
      GoGroup group = obj as GoGroup;
      if (group != null) {
        foreach (GoObject child in group.GetEnumerator()) {
          bestPort = pickNearestPort1(child, dc, bestPort, ref bestDist);
        }
      }
      return bestPort;
    }

    /// <summary>
    /// Return a <c>PointF</c> representing the position of the port.
    /// </summary>
    /// <param name="port">an <see cref="IGoPort"/> whose distance is being considered</param>
    /// <param name="dc">the point nearest which we are searching for a port</param>
    /// <returns>normally, <c>port.GoObject.Center</c></returns>
    /// <remarks>
    /// This is called by <see cref="PickNearestPort"/> for each
    /// port in the document.
    /// For large ports, if the <paramref name="port"/> is a <see cref="GoPort"/>,
    /// this uses the result of <see cref="GoPort.GetLinkPointFromPoint"/>, which should
    /// be more accurate than the center of the port.
    /// </remarks>
    public virtual PointF PortPoint(IGoPort port, PointF dc) {
      GoPort p = port.GoObject as GoPort;
      if (p != null) {
        GoObject pobj = p.PortObject;
        if (pobj == null || pobj.Layer == null)
          pobj = p;
        SizeF size = pobj.Size;
        if (size.Width < 10 && size.Height < 10)
          return pobj.Center;
        else
          return p.GetLinkPointFromPoint(dc);
      }
      return port.GoObject.Center;
    }

    /// <summary>
    /// This is called by <see cref="DoMouseUp"/> in order to create a new link.
    /// </summary>
    /// <param name="fromPort"></param>
    /// <param name="toPort"></param>
    /// <remarks>
    /// This just calls <see cref="GoView.CreateLink"/> and <see cref="GoView.RaiseLinkCreated"/>.
    /// </remarks>
    public virtual void DoNewLink(IGoPort fromPort, IGoPort toPort) {
      IGoLink link = this.View.CreateLink(fromPort, toPort);
      if (link != null) {
        this.TransactionResult = GoUndoManager.NewLinkName;
        this.View.RaiseLinkCreated(link.GoObject);
      } else {
        this.TransactionResult = null;
      }
    }

    /// <summary>
    /// This is called by <see cref="DoMouseUp"/> or <see cref="DoCancelMouse"/>
    /// when no new link was drawn by the user.
    /// </summary>
    /// <param name="fromPort"></param>
    /// <param name="toPort"></param>
    public virtual void DoNoNewLink(IGoPort fromPort, IGoPort toPort) {
      this.TransactionResult = null;
    }
    
    /// <summary>
    /// This is called by <see cref="DoMouseUp"/> in order to reconnect the existing link.
    /// </summary>
    /// <param name="oldlink"></param>
    /// <param name="fromPort"></param>
    /// <param name="toPort"></param>
    /// <remarks>
    /// This makes sure <paramref name="oldlink"/> refers to <paramref name="fromPort"/>
    /// and <paramref name="toPort"/> and then calls <see cref="GoView.RaiseLinkRelinked"/>.
    /// </remarks>
    public virtual void DoRelink(IGoLink oldlink, IGoPort fromPort, IGoPort toPort) {
#if !EXPRESS  // no GoSubGraph
      GoSubGraph fsg = GoSubGraph.FindParentSubGraph(fromPort.GoObject);
      GoSubGraph tsg = GoSubGraph.FindParentSubGraph(toPort.GoObject);
      GoObject pgroup = GoObject.FindCommonParent(fsg, tsg);
      while (pgroup != null && !(pgroup is GoSubGraph))
        pgroup = pgroup.Parent;
      GoSubGraph sg = pgroup as GoSubGraph;  // null is quite likely, meaning a top-level link
      GoObject linkobj = oldlink.GoObject;
      GoDocument doc = linkobj.Document;
      if (linkobj.Parent != sg && doc != null) {
        linkobj.Remove();
        if (sg != null)
          sg.InsertBefore(null, linkobj);
        else
          doc.LinksLayer.Add(linkobj);
      }
#endif
      oldlink.FromPort = fromPort;
      oldlink.ToPort = toPort;
      this.TransactionResult = GoUndoManager.RelinkName;
      this.View.RaiseLinkRelinked(oldlink.GoObject);
    }

    /// <summary>
    /// This is called by <see cref="DoMouseUp"/> or <see cref="DoCancelMouse"/>
    /// when an existing link was purposely not reconnected by the user to any port.
    /// </summary>
    /// <param name="oldlink"></param>
    /// <param name="fromPort"></param>
    /// <param name="toPort"></param>
    /// <remarks>
    /// Because this case effectively results in an object being removed from the
    /// document, this method calls <see cref="GoView.RaiseSelectionDeleting"/>
    /// and <see cref="GoView.RaiseSelectionDeleted"/>.
    /// If the <see cref="GoView.RaiseSelectionDeleting"/> event results in
    /// a cancellation, this calls <see cref="DoCancelMouse"/> instead of
    /// removing the link.
    /// This method does not remove the link if <see cref="GoObject.CanDelete"/>
    /// is false.
    /// </remarks>
    public virtual void DoNoRelink(IGoLink oldlink, IGoPort fromPort, IGoPort toPort) {
      GoObject l = oldlink.GoObject;
      if (l != null && l.Layer != null) {
        if (l.CanDelete()) {
          CancelEventArgs cancellable = new CancelEventArgs();
          this.View.RaiseSelectionDeleting(cancellable);
          if (!cancellable.Cancel) {
            l.Remove();
            this.View.RaiseSelectionDeleted();
            this.TransactionResult = GoUndoManager.RelinkName;
            return;
          } else {
            DoCancelMouse();
          }
        } else {
          DoCancelMouse();
        }
      }
      this.TransactionResult = null;
    }

    /// <summary>
    /// This is called by <see cref="DoCancelMouse"/> when a relinking was cancelled
    /// by the user.
    /// </summary>
    /// <param name="oldlink"></param>
    /// <param name="fromPort"></param>
    /// <param name="toPort"></param>
    public virtual void DoCancelRelink(IGoLink oldlink, IGoPort fromPort, IGoPort toPort) {
      oldlink.FromPort = fromPort;
      oldlink.ToPort = toPort;
      this.TransactionResult = null;
    }


    /// <summary>
    /// Gets or sets whether the user's linking operation started at the "From" port.
    /// </summary>
    /// <remarks>
    /// When this property is true, the <see cref="OriginalStartPort"/> and
    /// <see cref="StartPort"/> ports were or are at the "From" end of the
    /// <see cref="Link"/>.
    /// </remarks>
    public bool Forwards {
      get { return myForwards; }
      set { myForwards = value; }
    }

    /// <summary>
    /// Gets or sets the port from which the user is starting or modifying a link.
    /// </summary>
    /// <remarks>
    /// When creating a new link, the <see cref="GoToolLinkingNew"/> tool sets this
    /// property to the port under the initial mouse point.
    /// When reconnecting an existing link, the <see cref="GoToolRelinking"/> tool
    /// sets this property to the port at the other end of the link from the resize
    /// handle that the user is moving.
    /// This will be a port that already existed in the document prior to the
    /// linking operation.
    /// </remarks>
    /// <seealso cref="OriginalEndPort"/>
    /// <seealso cref="StartPort"/>
    /// <seealso cref="Link"/>
    public IGoPort OriginalStartPort {
      get { return myOrigStartPort; }
      set { myOrigStartPort = value; }
    }

    /// <summary>
    /// Gets or sets the port at the end of an existing link that is being reconnected.
    /// </summary>
    /// <remarks>
    /// When creating a new link, this property is not relevant.
    /// When reconnecting an existing link, the <see cref="GoToolRelinking"/> tool
    /// sets this property to the port at the end of the existing link that the
    /// user is disconnecting from.
    /// This will be a port that already existed in the document prior to the
    /// linking operation.
    /// </remarks>
    /// <seealso cref="OriginalStartPort"/>
    /// <seealso cref="EndPort"/>
    /// <seealso cref="Link"/>
    public IGoPort OriginalEndPort {
      get { return myOrigEndPort; }
      set { myOrigEndPort = value; }
    }

    /// <summary>
    /// Gets or sets the temporary starting port.
    /// </summary>
    /// <remarks>
    /// When creating a new link or when reconnecting an existing link, the tool
    /// sets this property to the value of <see cref="CreateTemporaryPort"/>.
    /// This will be a new port that only exists in this view.
    /// If <see cref="Forwards"/> is true, this port will correspond to
    /// the <see cref="IGoLink.FromPort"/> of the link; otherwise it will
    /// correspond to the <see cref="IGoLink.ToPort"/>
    /// </remarks>
    /// <seealso cref="EndPort"/>
    /// <seealso cref="OriginalStartPort"/>
    /// <seealso cref="Link"/>
    public IGoPort StartPort {
      get { return myTempStartPort; }
      set { myTempStartPort = value; }
    }

    /// <summary>
    /// Gets or sets the temporary ending port.
    /// </summary>
    /// <remarks>
    /// When creating a new link or when reconnecting an existing link, the tool
    /// sets this property to the value of <see cref="CreateTemporaryPort"/>.
    /// This will be a new port that only exists in this view.
    /// If <see cref="Forwards"/> is true, this port will correspond to
    /// the <see cref="IGoLink.ToPort"/> of the link; otherwise it will
    /// correspond to the <see cref="IGoLink.FromPort"/>
    /// </remarks>
    /// <seealso cref="StartPort"/>
    /// <seealso cref="OriginalEndPort"/>
    /// <seealso cref="Link"/>
    public IGoPort EndPort {
      get { return myTempEndPort; }
      set { myTempEndPort = value; }
    }

    /// <summary>
    /// Gets or sets the link that the user is manipulating for this linking operation.
    /// </summary>
    /// <remarks>
    /// When creating a new link, the <see cref="GoToolLinkingNew"/> tool sets this
    /// property to the value of <see cref="CreateTemporaryLink"/>, a new link that
    /// only exists in this view.
    /// When reconnecting an existing link, the <see cref="GoToolRelinking"/> tool
    /// sets this property to the existing link in the document.
    /// </remarks>
    public IGoLink Link {
      get { return myTempLink; }
      set { myTempLink = value; }
    }

    /// <summary>
    /// Gets the hashtable of all known ports that are valid for this particular linking operation.
    /// </summary>
    /// <remarks>
    /// This collection is initially empty for each linking operation.
    /// As the <see cref="PickNearestPort"/> method is called,
    /// the port is added to this collection, with a value depending on
    /// whether <see cref="IsValidLink"/> returns true.
    /// The value is <see cref="Valid"/> if it returned true,
    /// <see cref="Invalid"/> if it returned false.
    /// The cacheing is done because the computation to determine valid links can
    /// be expensive.
    /// You can turn off the cacheing by setting this property to null.
    /// </remarks>
    public Hashtable ValidPortsCache {
      get { return myValidPortsCache; }
      set { myValidPortsCache = value; }
    }

    /// <summary>
    /// Gets or sets whether users must draw their new links starting at the "from" port
    /// and going to the "to" port.
    /// </summary>
    /// <value>
    /// This value defaults to false, which will allow users to draw links "backwards".
    /// </value>
    public virtual bool ForwardsOnly {
      get { return myForwardsOnly; }
      set { myForwardsOnly = value; }
    }

    /// <summary>
    /// Gets or sets whether the temporary link is drawn orthogonally.
    /// </summary>
    /// <value>
    /// The default value is false.
    /// </value>
    /// <remarks>
    /// This property is used to initialize the link created by the default
    /// implementation of <see cref="CreateTemporaryLink"/>.
    /// </remarks>
    public virtual bool Orthogonal {
      get { return myOrthogonal; }
      set { myOrthogonal = value; }
    }
    

    /// <summary>
    /// This value associated with a port in the <see cref="ValidPortsCache"/>
    /// indicates that it is valid to make a link between <see cref="OriginalStartPort"/>
    /// and that port.
    /// </summary>
    /// <value>
    /// <see cref="PickNearestPort"/> calls <see cref="IsValidLink"/> with
    /// <see cref="OriginalStartPort"/> and each visible port in the document.
    /// If the predicate returned true, the port is associated with this Valid
    /// value in the <see cref="ValidPortsCache"/> hash table.
    /// </value>
    public static readonly Object Valid = "Valid";

    /// <summary>
    /// This value associated with a port in the <see cref="ValidPortsCache"/>
    /// indicates that it is not valid to make a link between <see cref="OriginalStartPort"/>
    /// and that port.
    /// </summary>
    /// <value>
    /// <see cref="PickNearestPort"/> calls <see cref="IsValidLink"/> with
    /// <see cref="OriginalStartPort"/> and each visible port in the document.
    /// If the predicate returned false, the port is associated with this Invalid
    /// value in the <see cref="ValidPortsCache"/> hash table.
    /// </value>
    public static readonly Object Invalid = "Invalid";


    // GoToolLinking state
    private bool myForwardsOnly = false;
    private bool myOrthogonal = false;

    [NonSerialized]
    private bool myLinkingNew = true;

    [NonSerialized]
    private bool myForwards = true;

    [NonSerialized]
    private IGoPort myOrigStartPort = null;

    [NonSerialized]
    private IGoPort myOrigEndPort = null;

    [NonSerialized]
    private IGoPort myTempStartPort = null;

    [NonSerialized]
    private IGoPort myTempEndPort = null;

    [NonSerialized]
    private IGoLink myTempLink = null;

    [NonSerialized]
    private Hashtable myValidPortsCache = new Hashtable();
  }


  /// <summary>
  /// The tool used to handle a user's drawing a new link between two ports.
  /// </summary>
  [Serializable]
  public class GoToolLinkingNew : GoToolLinking {
    /// <summary>
    /// The standard tool constructor.
    /// </summary>
    /// <param name="v"></param>
    public GoToolLinkingNew(GoView v) : base(v) {}

    /// <summary>
    /// The user can draw a new link if the view allows it
    /// and if the port at the input event point is a valid
    /// source port or a valid destination port.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This calls <see cref="GoToolLinking.PickPort"/> to find a port
    /// at the mouse down point.  At least one of the
    /// <see cref="GoToolLinking.IsValidFromPort"/> and
    /// <see cref="GoToolLinking.IsValidToPort"/> predicates
    /// must be true for the linking to start.
    /// </remarks>
    public override bool CanStart() {
      if (this.FirstInput.IsContextButton)
        return false;
      if (!this.View.CanLinkObjects())
        return false;

      IGoPort port = PickPort(this.FirstInput.DocPoint);
      this.OriginalStartPort = port;
      return port != null &&
             (IsValidFromPort(port) ||
              IsValidToPort(port));
    }

    /// <summary>
    /// This is just a matter of calling <see cref="GoToolLinking.StartNewLink"/>.
    /// </summary>
    public override void Start() {
      base.Start();
      StartNewLink(this.OriginalStartPort, this.LastInput.DocPoint);
    }
  }
  

  /// <summary>
  /// The tool used to handle the user's dragging one end of a link in order
  /// to connect it up to another port.
  /// </summary>
  [Serializable]
  public class GoToolRelinking : GoToolLinking {
    /// <summary>
    /// The standard tool constructor.
    /// </summary>
    /// <param name="v"></param>
    public GoToolRelinking(GoView v) : base(v) {}

    /// <summary>
    /// The user can relink if the view allows it and if the handle
    /// found at the input event point has an ID that indicates it
    /// is relinkable.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This calls <see cref="PickRelinkHandle"/> to find a handle.
    /// The <see cref="IGoHandle.HandleID"/> should be either
    /// <see cref="GoLink.RelinkableFromHandle"/> or
    /// <see cref="GoLink.RelinkableToHandle"/>.  The ID also
    /// determines which end of the link is disconnected; the
    /// original port at the disconnected end is remembered as
    /// the <see cref="GoToolLinking.OriginalEndPort"/> property.
    /// </remarks>
    public override bool CanStart() {
      if (this.FirstInput.IsContextButton)
        return false;
      if (!this.View.CanLinkObjects())
        return false;

      IGoHandle h = PickRelinkHandle(this.FirstInput.DocPoint);
      if (h == null) return false;
      if (h.HandleID == GoLink.RelinkableFromHandle) {
        GoObject link = h.SelectedObject;
        if (link == null) return false;
        this.Link = (IGoLink)link;
        this.OriginalEndPort = this.Link.FromPort;
        return true;
      } else if (h.HandleID == GoLink.RelinkableToHandle) {
        GoObject link = h.SelectedObject;
        if (link == null) return false;
        this.Link = (IGoLink)link;
        this.OriginalEndPort = this.Link.ToPort;
        return true;
      } else {
        return false;
      }
    }
    
    /// <summary>
    /// Start relinking by by calling <see cref="GoToolLinking.StartRelink"/>
    /// and hiding any selection handles for the link.
    /// </summary>
    public override void Start() {
      base.Start();

      StartRelink(this.Link, this.OriginalEndPort, this.LastInput.DocPoint);

      // hide all the selection handles, so we don't have to drag them along
      mySelectionHidden = true;
      GoObject l = this.Link.GoObject;
      if (l != null && l.IsInDocument && l.SelectionObject != null)
        l.SelectionObject.RemoveSelectionHandles(this.Selection);
    }

    /// <summary>
    /// Find a resize handle at the given point.
    /// </summary>
    /// <param name="dc">a <c>PointF</c> in document coordinates</param>
    /// <returns>an <see cref="IGoHandle"/> resize handle</returns>
    public virtual IGoHandle PickRelinkHandle(PointF dc) {
      GoObject obj = this.View.PickObject(false, true, dc, true);  // selectable handle in view
      return obj as IGoHandle;
    }
    
    /// <summary>
    /// Restore the selection handles on the link.
    /// </summary>
    public override void Stop() {
      // maybe restore the selection handles
      if (mySelectionHidden) {
        mySelectionHidden = false;
        GoObject l = this.Link.GoObject;
        if (l != null && l.IsInDocument && l.SelectionObject != null)
          l.SelectionObject.AddSelectionHandles(this.Selection, l);
      }

      base.Stop();
    }

    // GoToolRelinking state
    [NonSerialized]
    private bool mySelectionHidden = false;
  }
}
