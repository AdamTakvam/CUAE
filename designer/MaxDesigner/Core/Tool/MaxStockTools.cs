using System;
using System.Collections;
using Metreos.Max.Core;
using Metreos.Max.Core.Package;
using Metreos.Max.Resources.Images;
using Metreos.Max.Drawing;
using Metreos.Max.Framework.Satellite.Property;



namespace Metreos.Max.Core.Tool
{
    /// <summary>Stock tool references for use outside the toolbox</summary>
    public class MaxStockTools
    {
        #region singleton
        private MaxStockTools() {}
        private static MaxStockTools instance;

        public  static MaxStockTools Instance
        {  get 
           {
             if (instance == null)
             {
                 instance = new MaxStockTools();
                 instance.Init();
             }
             return instance;
           }
        }
        #endregion

        private static MaxToolGroup    toolGroup    = new MaxToolGroup
            (Const.defaultStockToolGroup, Const.defaultStockToolGroup);
    
        private static MaxCodeTool       codeTool       = new MaxCodeTool();
        private static MaxVariableTool   variableTool   = new MaxVariableTool();                
        private static MaxFunctionTool   functionTool   = new MaxFunctionTool();                
        private static MaxStartTool      startTool      = new MaxStartTool();    
        private static MaxCommentTool    commentTool    = new MaxCommentTool();  
        private static MaxAnnotationTool annotationTool = new MaxAnnotationTool(); 
        private static MaxLoopTool       loopTool       = new MaxLoopTool();      
        private static MaxLabelTool      labelTool      = new MaxLabelTool();
        private static MaxNullTool       nullTool       = new MaxNullTool();

        public  MaxToolGroup      ToolGroup      { get { return toolGroup;     } }
        public  MaxCodeTool       CodeTool       { get { return codeTool;      } }
        public  MaxVariableTool   VariableTool   { get { return variableTool;  } }                
        public  MaxFunctionTool   FunctionTool   { get { return functionTool;  } }                 
        public  MaxStartTool      StartTool      { get { return startTool;     } }     
        public  MaxCommentTool    CommentTool    { get { return commentTool;   } } 
        public  MaxAnnotationTool AnnotationTool { get { return annotationTool;} }    
        public  MaxLoopTool       LoopTool       { get { return loopTool;      } }        
        public  MaxLabelTool      LabelTool      { get { return labelTool;     } }      
        public  MaxNullTool       NullTool       { get { return nullTool;      } }  

        private void Init()
        {
            MaxImageIndex images = MaxImageIndex.Instance;

            MaxTool[] tools = new MaxTool[]
            {
                CodeTool, variableTool, functionTool, commentTool, 
                annotationTool, startTool, loopTool, labelTool, nullTool 
            };

            foreach(MaxTool tool in tools)
            {
                tool.ToolGroup = toolGroup;
                tool.ImagesLg  = images.StockToolImages32x32;
                tool.ImagesSm  = images.StockToolImages16x16;
                toolGroup.Add(tool);
            }

            codeTool.Name         = Const.defaultCodeToolName;
            codeTool.Description  = Const.defaultCodeToolDesc;
            codeTool.ImageIndexSm = MaxImageIndex.stockTool16x16IndexCustomCode;
            codeTool.ImageIndexLg = MaxImageIndex.stockTool32x32IndexCustomCode;

            variableTool.Name          = Const.defaultVariableToolName;
            variableTool.Description   = Const.defaultVariableToolDesc;
            variableTool.ImageIndexSm  = MaxImageIndex.stockTool16x16IndexVariable;
            variableTool.ImageIndexLg  = MaxImageIndex.stockTool32x32IndexVariable;

            functionTool.Name          = Const.defaultFunctionToolName;
            functionTool.Description   = Const.defaultFunctionToolDesc;
            functionTool.ImageIndexSm  = MaxImageIndex.stockTool16x16IndexFunction;
            functionTool.ImageIndexLg  = MaxImageIndex.stockTool32x32IndexFunction;

            commentTool.Name           = Const.defaultCommentToolName;
            commentTool.Description    = Const.defaultCommentToolDesc;
            commentTool.ImageIndexSm   = MaxImageIndex.stockTool16x16IndexComment;

            annotationTool.Name        = Const.defaultAnnotToolName;
            annotationTool.Description = Const.defaultAnnotToolDesc;
            annotationTool.ImageIndexSm= MaxImageIndex.stockTool16x16IndexComment;

            startTool.Name             = Const.defaultStartToolName;
            startTool.Description      = Const.defaultStartToolDesc;
            startTool.ImageIndexLg     = MaxImageIndex.stockTool32x32IndexStart;

            loopTool.Name              = Const.defaultLoopToolName;
            loopTool.Description       = Const.defaultLoopToolDesc;
            loopTool.ImageIndexSm      = MaxImageIndex.stockTool16x16IndexLoop;

            labelTool.Name             = Const.defaultLabelToolName;
            labelTool.Description      = Const.defaultLabelToolDesc;
            labelTool.ImageIndexSm     = MaxImageIndex.stockTool16x16IndexLabel;

            nullTool.Name              = Const.defaultNullToolName;
            nullTool.Description       = Const.defaultNullToolDesc;
            nullTool.ImageIndexSm      = 0;
            nullTool.ImageIndexLg      = 0;
        } // Init()                    


        /// <summary>Produce a stock tool from its class name</summary>
        public static MaxTool GetStockToolByClassName(string classname)
        {
            MaxTool tool = null;
            switch(classname)
            {
                case Const.NameCodeTool:     tool = MaxStockTools.Instance.CodeTool;      break;
                case Const.NameVariableTool: tool = MaxStockTools.Instance.VariableTool;  break;          
                case Const.NameFunctionTool: tool = MaxStockTools.Instance.FunctionTool;  break;                
                case Const.NameStartTool:    tool = MaxStockTools.Instance.StartTool;     break;
                case Const.NameCommentTool:  tool = MaxStockTools.Instance.CommentTool;   break;
                case Const.NameAnnotTool:    tool = MaxStockTools.Instance.AnnotationTool;break;
                case Const.NameLoopTool:     tool = MaxStockTools.Instance.LoopTool;      break;
                case Const.NameLabelTool:    tool = MaxStockTools.Instance.LabelTool;     break;
            } 
            return tool;
        }


        /// <summary>Produce a stock tool from its display name</summary>
        public static MaxTool GetStockToolByName(string toolname)
        {
            MaxTool tool = null;
            switch(toolname)
            {
                case Const.NameCode:         tool = MaxStockTools.Instance.CodeTool;      break;
                case Const.NameVariable:     tool = MaxStockTools.Instance.VariableTool;  break;          
                case Const.NameStart:        tool = MaxStockTools.Instance.StartTool;     break;
                case Const.NameComment:      tool = MaxStockTools.Instance.CommentTool;   break;
                case Const.NameAnnotation:   tool = MaxStockTools.Instance.AnnotationTool;break;
                case Const.NameLoop:         tool = MaxStockTools.Instance.LoopTool;      break;
                case Const.NameLabel:        tool = MaxStockTools.Instance.LabelTool;     break;
            } 
            return tool;
        }


        // Convenience methods 
        public static MaxRecumbentVariableNode NewMaxVariableNode(MaxCanvas canvas, DataTypes.Type scope)
        {
            return new MaxRecumbentVariableNode(canvas, variableTool, scope);
        }

        public static MaxFunctionNode NewMaxFunctionNode(MaxCanvas canvas)
        {
            return new MaxFunctionNode(canvas, functionTool);
        }

        public static MaxCommentNode NewMaxCommentNode(MaxCanvas canvas, string text)
        {
            return new MaxCommentNode(canvas, commentTool, text);
        }

        public static MaxAnnotationNode NewMaxAnnotationtNode(MaxCanvas canvas, string text, IMaxNode parent)
        {
            return MaxAnnotationNode.CreateAnnotation(canvas, annotationTool, text, 0, parent);
        }

        public static MaxStartNode NewMaxStartNode(MaxCanvas canvas, string text)
        {
            return new MaxStartNode(canvas, startTool);
        }
    }   // class MaxStockTools
}     // namespace
