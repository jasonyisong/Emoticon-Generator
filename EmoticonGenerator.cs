// created by Yi Song 2022/1/14
// VS code at Macbook Pro Env.
// Net5.0
// I implemented the Decorator Software Design Pattern for emoji decoration.
// I implemented the Command Software Design Pattern for Undo/Redo.
// vscode generate Class Diagram by using csharp plantUML
// reference: https://dotnettutorials.net/lesson/decorator-design-pattern-real-time-example/ . Decorator
// Reference L08.04 - A `Stack<T>` Class Example: Implementing an Undo-Redo System using Stacks. Command-driven
// updated by Yi Song 2022/1/25 for optimization

using System;
using System.Collections.Generic;

namespace EmoticonGenerator
{   
    public class Features
    {
        public string Name { get; set; } 
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public string Visiable { get; set; }

        public string Msg { get; set; }

        public string LastValue { get; set; }
    }
    
    // reference: https://dotnettutorials.net/lesson/decorator-design-pattern-real-time-example/
    // set up a interface 
    public interface FaceEmoji
    {
        List<Features> MakeFacialFeatures();

        void MakeShow(String name);

        void MakeHide(String name);

        void MakeReset(String name);

    }
    // set up a build emoji class
    public class BuildFaceEmoji : FaceEmoji
    {
        // emoji Face elements
        String[] featuresname = new String[] { "face","left-eye","right-eye","left-brow","right-brow","mouth"};

        //default value for svg emoji
        String[] defaultvalue = new String[] 
        { 
            $"<circle cx=\"100\" cy=\"100\" r=\"50\" stroke=\"black\" stroke-width=\"2\" fill=\"yellow\" />",
            $"<circle cx=\"120\" cy=\"100\" r=\"9\" style=\"stroke:black;stroke-width:2;fill:white\" /><circle cx=\"120\" cy=\"100\" r=\"4\" style=\"stroke:black;stroke-width:2;fill:black\" />",
            $"<circle cx=\"80\" cy=\"100\" r=\"9\" style=\"stroke:black;stroke-width:2;fill:white\" /><circle cx=\"80\" cy=\"100\" r=\"4\" style=\"stroke:black;stroke-width:2;fill:black\" />",
            $"<line x1=\"70\" y1=\"75\" x2=\"90\" y2=\"75\" style=\"stroke:black;stroke-width:3\" />",
            $"<line x1=\"110\" y1=\"75\" x2=\"130\" y2=\"75\" style=\"stroke:black;stroke-width:3\" />",
            $"<line x1=\"85\" y1=\"125\" x2=\"115\" y2=\"125\" style=\"stroke:black;stroke-width:3\" />"
        };
        // list for elements
        public List<Features> emojifeatureslist = new List<Features>();
        // put value to list
        public BuildFaceEmoji()
        {
            for(int i = 0; i < featuresname.Length; i++)
            {
                //default value is empty for hide, DefaultValue for default show
                emojifeatureslist.Add(new Features { Name = featuresname[i], Value = "",Visiable="N", DefaultValue=defaultvalue[i],Msg="",LastValue=""});
            }
            
        }
        public List<Features> MakeFacialFeatures()
        {
            return emojifeatureslist;
        }

        public void MakeShow(String name)
        {
        }
        public void MakeHide(String name)
        {
        }
        public void MakeReset(String name)
        {
        }
        // show emoji svg
        public String Show()
        {
            String svgOpen=$"<svg xmlns=\"http://www.w3.org/2000/svg\" version=\"1.1\" width=\"200\" height=\"200\">"+Environment.NewLine;
            String svgContent="";
            String svgClose="</svg>";
            foreach (var i in emojifeatureslist)
            { 
               svgContent+="".PadLeft(6, ' ')+i.Value+Environment.NewLine;
            } 
            // Console.WriteLine(svgOpen+svgContent+svgClose);
            return svgOpen+svgContent+svgClose;
            
        }
    }
    // set up a emoji Decorator class
    public abstract class EmojiDecorator : FaceEmoji
    {
        // create a object from GetAttrValue
        // pass a feature line string to parse attributes get the detail value,
        // get the value like: x1, y1
        public GetAttrValue gav=new GetAttrValue();
        public string name;      
        public string action;
        public string actionReverse;
        public string actionForward;
        //public string direction;
        public int dvalue;
        public string styleType;
        public string styleA;
        public string styleB;
        public string styleC;
        public string currStyleType;
        public string style;
        //public string currValue;
        public string msg;
        //public string unresetcontent;
        public List<Features> featuresList;
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public int cx;
        public int cy;
        public int r;
        public int x1default;
        public int y1default;
        public int x2default;
        public int y2default;
        public int cxdefault;
        public int cydefault;
        public int rdefault;
        protected FaceEmoji emoji;
        public EmojiDecorator(FaceEmoji emoji)
        {
            // set all Decorator style default value is A
            styleType="Style A";
            this.emoji = emoji;
        }
        public virtual List<Features> MakeFacialFeatures()
        {  
           return emoji.MakeFacialFeatures();
        }
        public virtual void MakeShow(String name)
        {  
        }
        public virtual void MakeHide(String name)
        {  
        }
        public virtual void MakeReset(String name)
        {  
        }      
    }
    // same operate show/hide/reset for Face elements
    public class CommonDecorator : EmojiDecorator
    {
        public CommonDecorator(FaceEmoji emoji) : base(emoji)
        {    
            //name="";
        }
        public override List<Features> MakeFacialFeatures()
        {
            featuresList= emoji.MakeFacialFeatures();
            return featuresList;
        }
        public override void MakeShow(string name)
        {  
           featuresList.ForEach(i => { if (i.Name == name) { if(i.Value==""){i.Value=i.DefaultValue;} if(i.LastValue!=""){i.Value=i.LastValue;i.LastValue="";} } }); 
        }
        public override void MakeHide(string name)
        {  
            featuresList.ForEach(i => { if (i.Name == name) { if(i.Value!=""){i.LastValue=i.Value; i.Value="";} } });
        }
        public override void MakeReset(string name)
        {  
            featuresList.ForEach(i => { if (i.Name == name) { i.LastValue=i.Value; i.Value=i.DefaultValue;} });        
        }

    }
    // make a face 
    public class FaceDecorator : EmojiDecorator
    {
        public FaceDecorator(FaceEmoji emoji) : base(emoji)
        {    
            name="face";
        }
        public override List<Features> MakeFacialFeatures()
        {
            featuresList= emoji.MakeFacialFeatures();
            MakeFace();
            return featuresList;
        }
        private void MakeFace()
        {
            featuresList.ForEach(i => { if (i.Name == name) {i.Value=i.DefaultValue;} });
        }

    }
    // set up a Left Brow Decorator class
    public class LeftBrowDecorator : EmojiDecorator
    {
        public LeftBrowDecorator(FaceEmoji emoji) : base(emoji)
        {          
            name="left-brow";
            // set A,B,C style value
            styleA="stroke:black;stroke-width:3";
            styleB="stroke:red;stroke-width:2";
            styleC="stroke:blue;stroke-width:1";
            //styleType="Style A";
            // set default value, not use 
            x1default=70; y1default=75; x2default=90; y2default=75;
        }
        public override List<Features> MakeFacialFeatures()
        {
            featuresList= emoji.MakeFacialFeatures();
            GetCurrProperties();
            switch(action)
            {
                case "show":
                    emoji.MakeShow(name);
                    GetCurrProperties();
                    msg="Left Brow ("+currStyleType+") shown/unreset to emoticon.";
                    //MakeLeftBrowShow();
                    break;
                case "hide":
                    emoji.MakeHide(name);
                    msg="Left Brow ("+currStyleType+") hidden from emoticon.";
                    //MakeLeftBrowHide();
                    break;
                case "reset":
                    emoji.MakeReset(name);
                    GetCurrProperties();
                    msg="Left Brow ("+currStyleType+") reset to emoticon.";
                    //MakeLeftBrowReset();
                    break;
                case "moveup": case "movedown": case "moveleft": case "moveright":
                    MakeLeftBrowMove();
                    msg="Left Brow ("+currStyleType+") moved "+action.Replace("move","")+" "+dvalue+"px.";
                    break;
                case "Style A": case "Style B": case "Style C": 
                    MakeLeftBrowStyle();
                    msg="Left Brow style to "+styleType;
                    break;
                default:
                    break;
            }
            Console.WriteLine(msg);
            return featuresList;
        }
        /*
        private void MakeLeftBrowShow()
        {
            featuresList.ForEach(i => { if (i.Name == name) { if(i.Value==""){i.Value=i.DefaultValue;} if(i.LastValue!=""){i.Value=i.LastValue;i.LastValue="";} } }); 
        }
        private void MakeLeftBrowHide()
        {
            featuresList.ForEach(i => { if (i.Name == name) {if(i.Value!=""){i.LastValue=i.Value; i.Value="";} } });
        }
        private void MakeLeftBrowReset()
        {
            featuresList.ForEach(i => { if (i.Name == name) { i.LastValue=i.Value; i.Value=i.DefaultValue;} });        
        }
        */
        private void GetCurrProperties()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        gav.GetParameter(i.Value);     
                        x1=gav.X1; y1=gav.Y1; x2=gav.X2; y2=gav.Y2;
                        style=gav.Style;
                        if(gav.Style==styleA)
                        {
                            currStyleType="Style A";
                        }
                        if(gav.Style==styleB)
                        {
                            currStyleType="Style B";
                        }
                        if(gav.Style==styleC)
                        {
                            currStyleType="Style C";
                        } 
                    }
                    else
                    {
                        currStyleType=styleType;
                    }
                } 
            });           
        }
        private void MakeLeftBrowMove()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        //gav.GetParameter(i.Value);
                        //x1=gav.X1; y1=gav.Y1; x2=gav.X2; y2=gav.Y2;
                        //style=gav.Style;
                        switch (action)
                        {
                            case "moveup": 
                                y1=y1-dvalue;y2=y2-dvalue;
                                break; 
                            case "movedown": 
                                y1=y1+dvalue;y2=y2+dvalue;
                                break; 
                            case "moveleft": 
                                x1=x1-dvalue;x2=x2-dvalue;
                                break; 
                            case "moveright": 
                                x1=x1+dvalue;x2=x2+dvalue;
                                break; 
                            default:
                                break;                       
                        }
                        i.Value=$"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" style=\"{style}\" />";
                        
                    }
                } 
            });           
        }

        private void MakeLeftBrowStyle()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        //gav.GetParameter(i.Value);
                        // get the last value to current value, means not to change 
                        //x1=gav.X1; y1=gav.Y1; x2=gav.X2; y2=gav.Y2;
                        //style=styleA;
                        // if style type is differenct to change style
                        /*
                        if(gav.Style==styleA)
                        {
                            actionReverse="Style A";
                        }
                        if(gav.Style==styleB)
                        {
                            actionReverse="Style B";
                        }
                        if(gav.Style==styleC)
                        {
                            actionReverse="Style C";
                        }
                        */
                        actionReverse=currStyleType;
                        styleType=action;
                        switch(styleType)
                        {
                            case "Style A":
                                style=styleA;
                                break;
                            case "Style B":
                                style=styleB;
                                break;
                            case "Style C":
                                style=styleC;
                                break;
                            default:
                                break;
                        }
                        i.Value=$"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" style=\"{style}\" />";
                    }
                } 
            });              
        }
    }
    
    public class RightBrowDecorator : EmojiDecorator
    {
        public RightBrowDecorator(FaceEmoji emoji) : base(emoji)
        {
            name="right-brow";
            styleA="stroke:black;stroke-width:3";
            styleB="stroke:red;stroke-width:2";
            styleC="stroke:blue;stroke-width:1";
            x1default=110; y1default=75; x2default=130; y2default=75;
        }
        public override List<Features> MakeFacialFeatures()
        {  
            featuresList= emoji.MakeFacialFeatures();
            GetCurrProperties();
            switch(action)
            {
                case "show":
                    emoji.MakeShow(name);
                    GetCurrProperties();
                    msg="Right Brow ("+currStyleType+") shown/unreset to emoticon.";
                    //MakeRightBrowShow();
                    break;
                case "hide":
                    emoji.MakeHide(name);
                    msg="Right Brow ("+currStyleType+") hidden from emoticon.";
                    //MakeRightBrowHide();
                    break;
                case "reset":
                    emoji.MakeReset(name);
                    GetCurrProperties();
                    msg="Right Brow ("+currStyleType+") reset to emoticon.";
                    //MakeRightBrowReset();
                    break;
                case "moveup": case "movedown": case "moveleft": case "moveright":
                    MakeRightBrowMove();
                    msg="Right Brow ("+currStyleType+") moved "+action.Replace("move","")+" "+dvalue+"px.";
                    break;
                case "Style A": case "Style B": case "Style C": 
                    MakeRightBrowStyle();
                    msg="Right Brow style to "+styleType;
                    break;
                default:
                    break;
            }
            Console.WriteLine(msg);
            return featuresList;
        }
        /*
        private void MakeRightBrowShow()
        {
            featuresList.ForEach(i => { if (i.Name == name) {if(i.Value==""){i.Value=i.DefaultValue;} if(i.LastValue!=""){i.Value=i.LastValue;i.LastValue="";} } }); //else{i.Value=i.LastValue;}     
        }
        private void MakeRightBrowHide()
        {
            featuresList.ForEach(i => { if (i.Name == name) {if(i.Value!=""){i.LastValue=i.Value; i.Value="";} } }); 
        }

        private void MakeRightBrowReset()
        {
            featuresList.ForEach(i => { if (i.Name == name) {i.LastValue=i.Value; i.Value=i.DefaultValue;} }); 
               
        }
        */
        private void GetCurrProperties()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        gav.GetParameter(i.Value);     
                        x1=gav.X1; y1=gav.Y1; x2=gav.X2; y2=gav.Y2;
                        style=gav.Style;
                        if(gav.Style==styleA)
                        {
                            currStyleType="Style A";
                        }
                        if(gav.Style==styleB)
                        {
                            currStyleType="Style B";
                        }
                        if(gav.Style==styleC)
                        {
                            currStyleType="Style C";
                        } 

                    }
                    else
                    {
                        currStyleType=styleType;
                    }
                } 
            });           
        }
        private void MakeRightBrowMove()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        switch (action)
                        {
                            case "moveup": 
                                y1=y1-dvalue;y2=y2-dvalue;
                                break; 
                            case "movedown": 
                                y1=y1+dvalue;y2=y2+dvalue;
                                break; 
                            case "moveleft": 
                                x1=x1-dvalue;x2=x2-dvalue;
                                break; 
                            case "moveright": 
                                x1=x1+dvalue;x2=x2+dvalue;
                                break; 
                            default:
                                break;
                        }
                        i.Value=$"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" style=\"{style}\" />";
                    }
                } 
            });       
        }
        private void MakeRightBrowStyle()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        actionReverse=currStyleType;
                        styleType=action;
                        switch(styleType)
                        {
                            case "Style A":
                                style=styleA;
                                break;
                            case "Style B":
                                style=styleB;
                                break;
                            case "Style C":
                                style=styleC;
                                break;
                            default:
                                break;
                        }

                        i.Value=$"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" style=\"{style}\" />";
                    }
                } 
            });      
        }
    }
    
    public class LeftEyeDecorator : EmojiDecorator
    {
        
        public LeftEyeDecorator(FaceEmoji emoji) : base(emoji)
        {
            name="left-eye";
            styleA="stroke:black;stroke-width:2;fill:black";
            styleB="stroke:black;stroke-width:2;fill:red";
            styleC="stroke:black;stroke-width:2;fill:blue";
            cxdefault=80; cydefault=100; rdefault=4; 
        }
        public override List<Features>  MakeFacialFeatures()
        {
            featuresList= emoji.MakeFacialFeatures();
            GetCurrProperties();
            switch(action)
            {
                case "show":
                    emoji.MakeShow(name);
                    GetCurrProperties();
                    msg="Left Eye ("+currStyleType+") shown/unreset to emoticon.";
                    //MakeLeftEyeShow();
                    break;
                case "hide":
                    emoji.MakeHide(name);
                    msg="Left Eye ("+currStyleType+") hidden from emoticon.";
                    //MakeLeftEyeHide();
                    break;
                case "reset":
                    emoji.MakeReset(name);
                    GetCurrProperties();
                    msg="Left Eye ("+currStyleType+") reset to emoticon.";
                    //MakeLeftEyeReset();
                    break;
                case "moveup": case "movedown": case "moveleft": case "moveright":
                    MakeLeftEyeMove();
                    msg="Left Eye ("+currStyleType+") moved "+action.Replace("move","")+" "+dvalue+"px.";
                    break;
                case "Style A": case "Style B": case "Style C": 
                    MakeLeftEyeStyle();
                    msg="Left Eye style to "+styleType;
                    break;
                default:
                    break;
            }
            Console.WriteLine(msg);
            return featuresList;
        }
        /*
        private void MakeLeftEyeShow()
        {
              featuresList.ForEach(i => { if (i.Name == name) {if(i.Value==""){i.Value=i.DefaultValue;} if(i.LastValue!=""){i.Value=i.LastValue;i.LastValue="";} } }); //else{i.Value=i.LastValue;}
        }
        private void MakeLeftEyeHide()
        {
            featuresList.ForEach(i => { if (i.Name == name) { if(i.Value!=""){i.LastValue=i.Value; i.Value="";} } });  
        }
        private void MakeLeftEyeReset()
        {
            featuresList.ForEach(i => { if (i.Name == name) {i.LastValue=i.Value; i.Value=i.DefaultValue;} });
        }
        */
        private void GetCurrProperties()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        gav.GetParameter(i.Value);
                        cx=gav.CX; cy=gav.CY; r=gav.R; 
                        style=gav.Style;
                        if(gav.Style==styleA)
                        {
                            currStyleType="Style A";
                        }
                        if(gav.Style==styleB)
                        {
                            currStyleType="Style B";
                        }
                        if(gav.Style==styleC)
                        {
                            currStyleType="Style C";
                        } 
                    }
                    else
                    {
                        currStyleType=styleType;
                    }
                } 
            });           
        }
        private void MakeLeftEyeMove()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        switch (action)
                        {
                            case "moveup": 
                                cy=cy-dvalue;
                                break; 
                            case "movedown": 
                                cy=cy+dvalue;
                                break; 
                            case "moveleft": 
                                cx=cx-dvalue;
                                break; 
                            case "moveright": 
                                cx=cx+dvalue;
                                break; 
                            default:
                                break;
                        }
                        i.Value=$"<circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r+5}\" style=\"stroke:black;stroke-width:2;fill:white\" /><circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" style=\"{style}\" />";
                    }
                } 
            });       
        }
        private void MakeLeftEyeStyle()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        actionReverse=currStyleType;
                        styleType=action;
                        switch(styleType)
                        {
                            case "Style A":
                                style=styleA;
                                break;
                            case "Style B":
                                style=styleB;
                                break;
                            case "Style C":
                                style=styleC;
                                break;
                            default:
                                break;
                        }
                        i.Value=$"<circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r+5}\" style=\"stroke:black;stroke-width:2;fill:white\" /><circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" style=\"{style}\" />";
                    }        
                }        
            });             
        }
    }

    public class RightEyeDecorator : EmojiDecorator
    {
        public RightEyeDecorator(FaceEmoji emoji) : base(emoji)
        {
            name="right-eye";
            styleA="stroke:black;stroke-width:2;fill:black";
            styleB="stroke:black;stroke-width:2;fill:red";
            styleC="stroke:black;stroke-width:2;fill:blue";
            cxdefault=120; cydefault=100; rdefault=4; 
        }
        public override List<Features>  MakeFacialFeatures()
        {  
            featuresList= emoji.MakeFacialFeatures();
            GetCurrProperties();
            switch(action)
            {
                case "show":
                    emoji.MakeShow(name);
                    GetCurrProperties();
                    msg="Right Eye ("+currStyleType+") shown/unreset to emoticon.";
                    //MakeRightEyeShow();
                    break;
                case "hide":
                    emoji.MakeHide(name);
                    msg="Right Eye ("+currStyleType+") hidden from emoticon.";
                    //MakeRightEyeHide();
                    break;
                case "reset":
                    emoji.MakeReset(name);
                    GetCurrProperties();
                    msg="Right Eye ("+currStyleType+") reset to emoticon.";
                    //MakeRightEyeReset();
                    break;
                case "moveup": case "movedown": case "moveleft": case "moveright":
                    MakeRightEyeMove();
                    msg="Right Eye ("+currStyleType+") moved "+action.Replace("move","")+" "+dvalue+"px.";
                    break;
                case "Style A": case "Style B": case "Style C": 
                    MakeRightEyeStyle();
                    msg="Right Eye style to "+styleType;
                    break;
                default:
                    break;
            }
            Console.WriteLine(msg);
            return featuresList;
        }
        /*
        private void MakeRightEyeShow()
        {
              featuresList.ForEach(i => { if (i.Name == name) { if(i.Value==""){i.Value=i.DefaultValue;} if(i.LastValue!=""){i.Value=i.LastValue;i.LastValue="";} } }); //else{i.Value=i.LastValue;}
        }
        private void MakeRightEyeHide()
        {
            featuresList.ForEach(i => { if (i.Name == name) { if(i.Value!=""){i.LastValue=i.Value; i.Value="";} } });  
        }
        private void MakeRightEyeReset()
        {
            featuresList.ForEach(i => { if (i.Name == name) { i.LastValue=i.Value; i.Value=i.DefaultValue;} });  
        }
        */
        private void GetCurrProperties()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        gav.GetParameter(i.Value);
                        cx=gav.CX; cy=gav.CY; r=gav.R; 
                        style=gav.Style;
                        if(gav.Style==styleA)
                        {
                            currStyleType="Style A";
                        }
                        if(gav.Style==styleB)
                        {
                            currStyleType="Style B";
                        }
                        if(gav.Style==styleC)
                        {
                            currStyleType="Style C";
                        } 
                    }
                    else
                    {
                        currStyleType=styleType;
                    }
                } 
            });           
        }
        private void MakeRightEyeMove()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        switch (action)
                        {
                            case "moveup": 
                                cy=cy-dvalue;
                                break; 
                            case "movedown": 
                                cy=cy+dvalue;
                                break; 
                            case "moveleft": 
                                cx=cx-dvalue;
                                break; 
                            case "moveright": 
                                cx=cx+dvalue;
                                break; 
                            default:
                                break;
                        }
                        i.Value=$"<circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r+5}\" style=\"stroke:black;stroke-width:2;fill:white\" /><circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" style=\"{style}\" />";
                    }
                } 
            });     
        }
        private void MakeRightEyeStyle()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        actionReverse=currStyleType;
                        styleType=action;
                        switch(styleType)
                        {
                            case "Style A":
                                style=styleA;
                                break;
                            case "Style B":
                                style=styleB;
                                break;
                            case "Style C":
                                style=styleC;
                                break;
                            default:
                                break;
                        }
                        i.Value=$"<circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r+5}\" style=\"stroke:black;stroke-width:2;fill:white\" /><circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" style=\"{style}\" />";
                    }           
                }          
            });           
        }
    }
    public class MouthDecorator : EmojiDecorator
    {
        public MouthDecorator(FaceEmoji emoji) : base(emoji)
        {
            name="mouth";
            styleA="stroke:black;stroke-width:3";
            styleB="stroke:red;stroke-width:2";
            styleC="stroke:blue;stroke-width:1";
            x1default=85; y1default=125; x2default=115; y2default=125;
        }
        public override List<Features> MakeFacialFeatures()
        {  
            featuresList= emoji.MakeFacialFeatures();
            GetCurrProperties();
            switch(action)
            {
                case "show":
                    emoji.MakeShow(name);
                    GetCurrProperties();
                    msg="Mouth ("+currStyleType+") shown/unreset to emoticon.";
                    //MakeMouthShow();
                    break;
                case "hide":
                    emoji.MakeHide(name);
                    msg="Mouth ("+currStyleType+") hidden from emoticon.";
                    //MakeMouthHide();
                    break;
                case "reset":
                    emoji.MakeReset(name);
                    GetCurrProperties();
                    msg="Mouth ("+currStyleType+") reset to emoticon.";
                    //MakeMouthReset();
                    break;
                case "moveup": case "movedown": case "moveleft": case "moveright":
                    MakeMouthMove();
                    msg="Mouth ("+currStyleType+") moved "+action.Replace("move","")+" "+dvalue+"px.";
                    break;
                case "Style A": case "Style B": case "Style C": 
                    MakeMouthtyle();
                    msg="Mouth style to "+styleType;
                    break;
                default:
                    break;
            }
            Console.WriteLine(msg);
            return featuresList;
        }
        /*
        private void MakeMouthShow()
        {
              featuresList.ForEach(i => { if (i.Name == name) { if(i.Value==""){i.Value=i.DefaultValue;} if(i.LastValue!=""){i.Value=i.LastValue;i.LastValue="";} } }); //else{i.Value=i.LastValue;}
        }
        private void MakeMouthHide()
        {
            featuresList.ForEach(i => { if (i.Name == name) {if(i.Value!=""){i.LastValue=i.Value; i.Value="";} } }); 
        }
        private void MakeMouthReset()
        {
            featuresList.ForEach(i => { if (i.Name == name) { i.LastValue=i.Value; i.Value=i.DefaultValue;} });  
        }
        */
        private void GetCurrProperties()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        gav.GetParameter(i.Value);
                        x1=gav.X1; y1=gav.Y1; x2=gav.X2; y2=gav.Y2;
                        style=gav.Style;
                        if(gav.Style==styleA)
                        {
                            currStyleType="Style A";
                        }
                        if(gav.Style==styleB)
                        {
                            currStyleType="Style B";
                        }
                        if(gav.Style==styleC)
                        {
                            currStyleType="Style C";
                        } 
                    }
                    else
                    {
                        currStyleType=styleType;
                    }
                } 
            });           
        }
        private void MakeMouthMove()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        switch (action)
                        {
                            case "moveup": 
                                y1=y1-dvalue;y2=y2-dvalue;
                                break; 
                            case "movedown": 
                                y1=y1+dvalue;y2=y2+dvalue;
                                break; 
                            case "moveleft": 
                                x1=x1-dvalue;x2=x2-dvalue;
                                break; 
                            case "moveright": 
                                x1=x1+dvalue;x2=x2+dvalue;
                                break; 
                            default:
                                break;
                        }
                        i.Value=$"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" style=\"{style}\" />";
                    }
                } 
            });         
        }
        private void MakeMouthtyle()
        {
            featuresList.ForEach(i => 
            { 
                if (i.Name == name) 
                {
                    if(i.Value!="")
                    {
                        actionReverse=currStyleType;
                        styleType=action;
                        switch(styleType)
                        {
                            case "Style A":
                                style=styleA;
                                break;
                            case "Style B":
                                style=styleB;
                                break;
                            case "Style C":
                                style=styleC;
                                break;
                            default:
                                break;
                        }
                        i.Value=$"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" style=\"{style}\" />";
                    }
                }            
            });             
        }    
    }
    
    // Reference L08.04 - A `Stack<T>` Class Example: Implementing an Undo-Redo System using Stacks
    // Command-driven
    // Do, Undo and Redo operations are typical operations (commands) in drawing, editing or gaming applications to it should not be a surprise to hear that there are standard design patters that we can draw on to inform our implementations.
    // The Sessions (Invoker) Class
    public class Sessions
    {
        private Stack<Command> undo;
        private Stack<Command> redo;

        public int UndoCount { get => undo.Count; }
        public int RedoCount { get => undo.Count; }
        public Sessions()
        {
            Reset();
        }
        public void Reset()
        {
            undo = new Stack<Command>();
            redo = new Stack<Command>();
        }

        public void Action(Command command)
        {
            // first update the undo - redo stacks
            undo.Push(command);  // save the command to the undo command
            redo.Clear();        // once a new command is issued, the redo stack clears

            // next determine  action from the Command object type
            // this is going to be AddCommand or DeleteCommand and more 
            Type t = command.GetType();
            if (t.Equals(typeof(AddCommand)))
            {
                command.Do();
            }
            /*
            if (t.Equals(typeof(RemoveCommand)))
            {
                command.Do();
            }
            */
        }

        // Undo
        public void Undo()
        {
            if (undo.Count > 0)
            {
                Command c = undo.Pop(); c.Undo(); redo.Push(c);
            }
        }

        // Redo
        public void Redo()
        {
            
            if (redo.Count > 0)
            {
                Command c = redo.Pop(); c.Do(); undo.Push(c);
            }
        }

    }
    // Abstract Command (Command) class - commands can do something and also undo
    public abstract class Command
    {
        public abstract void Do();     // what happens when we execute (do)
        public abstract void Undo();   // what happens when we unexecute (undo)
    }

    // Add feature Command - it is a ConcreteCommand Class (extends Command)
    // This adds a feature to the Face emoji as the "Do" action
    public class AddCommand : Command
    {
        
        //FaceEmoji faceemoji;
        EmojiDecorator emojidecorator;

        public AddCommand(EmojiDecorator ed)
        {
            
            emojidecorator=ed;

        }

        // Adds a feature to the face as "Do" action
        public override void Do()
        {
            //Console.WriteLine(emojidecorator.action);
            emojidecorator.action=emojidecorator.actionForward;
            emojidecorator.MakeFacialFeatures();

        }
        // Removes a feature from the face as "Undo" action
        public override void Undo()
        {
            emojidecorator.action=emojidecorator.actionReverse;
            emojidecorator.MakeFacialFeatures();
        }

    }

    // Remove feature Command - it is a ConcreteCommand Class (extends Command)
    // This deletes a feature from the face as the "Do" action
    /*
    public class RemoveCommand : Command
    {
        public RemoveCommand(EmojiDecorator ed,FaceEmoji fe)
        {
        }
        // Removes a feature from the face as "Do" action
        public override void Do()
        {

           //facedecorator.MakeFacialFeatures(faceemoji,"hide","","",laststyletype,"");
        }
        // Restores a feature to the face a an "Undo" action
        public override void Undo()
        {
            //facedecorator.MakeFacialFeatures(faceemoji,"show","","",laststyletype,"");
        }
    }
    */
   

    // set up a class for get attributes from a line string of svg
    public class GetAttrValue
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int R { get; set; }
        public int CX { get; set; }
        public int CY { get; set; }
        public int RX { get; set; }
        public int RY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Style { get;  set; } 
        /*
        public string Fill { get;  set; }  // for fill
        public string Stroke { get;  set; }   // for stroke  
        public string StrokeWidth { get;  set; }  // for stroke-width
        public string Transform { get;  set; }  // for stroke-width
        public string textcontent { get;  set; } // for Text content

        // for Text styling
        public string fontfamily { get;  set; }
        public string fontsize { get;  set; }      
        public string fontsizeadjust { get;  set; }
        public string fontstretch { get;  set; }
        public string fontstyle { get;  set; }      
        public string fontvariant { get;  set; }
        public string fontweight { get;  set; }
        public string Points { get;  set; }   // for polyline and polygon points
        public string PathD { get;  set; }     // for path shape D
        */
        public void GetParameter(String str)
        {
            //Fill = "grey"; Stroke = "black"; StrokeWidth="1pt";
            
            if(!string.IsNullOrEmpty(str))
            {
                
                string[] ParaSplit = str.Split(new string[] { " ","=\"","\"" }, StringSplitOptions.None); 
                
                for (int z=0; z<=ParaSplit.Length-1; z++)
                {
                    // upper every value for switch, if found, set the shape properties
                    switch(ParaSplit[z].ToUpper())
                    {
                        case "X":
                            //set the shape properties with next value of the array
                            X=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "Y":
                            Y=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "WIDTH":
                            Width=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "HEIGHT":
                            Height=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "CX":
                            CX=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "CY":
                            CY=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "R":
                            R=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "RX":
                            RX=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "RY":
                            RY=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "X1":
                            X1=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "Y1":
                            Y1=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "X2":
                            X2=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "Y2":
                            Y2=Int32.Parse(ParaSplit[z+1]);
                            break;
                        case "STYLE":
                            Style=ParaSplit[z+1];
                            break;
                        /*
                        case "POINTS":
                            // if is points parameter, replace "/" with " ", and replace "~" with "," for points of polyline and polygon shape properties
                            Points=ParaSplit[z+1].Replace("/"," ").Replace("~",",");
                            //strpoint=c[y].Substring(c[y].IndexOf("=\"")+2,c[y].Length-c[y].IndexOf("=\"")-4).Replace("/"," ");
                            break;
                        case "D":
                            // if is D parameter, replace "/" with " " for D of path properties
                            PathD=ParaSplit[z+1].Replace("/"," ");
                            //pathD=c[y].Substring(c[y].IndexOf("=\"")+2,c[y].Length-c[y].IndexOf("=\"")-4).Replace("/"," ");
                            break;
                        case "FILL":
                            Fill=ParaSplit[z+1];
                            break;
                        case "STROKE":
                            Stroke=ParaSplit[z+1];
                            break;
                        case "STROKE-WIDTH":
                            StrokeWidth=ParaSplit[z+1];
                            break;
                        case "TRANSFORM":
                            //Console.WriteLine(ParaSplit[z+1]);
                            // if is transform parameter, replace "/" with " " for transform of the shape properties
                            Transform=ParaSplit[z+1].Replace("/"," ").Replace("<","(").Replace(">",")");
                            break;
                        // Set Text properties
                        case "FONT-FAMILY":
                            fontfamily=ParaSplit[z+1];
                            break;
                        case "FONT-SIZE":
                            fontsize=ParaSplit[z+1];
                            break;
                        case "FONT-SIZE-ADJUST":
                            fontsizeadjust=ParaSplit[z+1];
                            break;
                        case "FONT-STRETCH":
                            fontstretch=ParaSplit[z+1];
                            break;
                        case "FONT-STYLE":
                            fontstyle=ParaSplit[z+1];
                            break;
                        case "FONT-VARIANT":
                            fontvariant=ParaSplit[z+1];
                            break;
                        case "FONT-WEIGHT":
                            fontweight=ParaSplit[z+1];
                            break;
                        case "TEXT-CONTENT":
                            textcontent=ParaSplit[z+1];
                            break;
                        */
                        default:
                            break;
                    }
                }  
            }
        }
    }
}
