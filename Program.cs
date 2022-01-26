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
using System.IO;
//using System.Collections.Generic;

namespace exam_coding_assignment
{
    class Program
    {
        //read command line
        static String UI1()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }
        static void Main(string[] args)
        {
            // create a emoji object
            EmoticonGenerator.BuildFaceEmoji faceemoji = new EmoticonGenerator.BuildFaceEmoji();

            // create a emoji decorator oject
            EmoticonGenerator.EmojiDecorator emojidecorator;   
            // show emoji face svg
            emojidecorator=new EmoticonGenerator.FaceDecorator(faceemoji);
            emojidecorator.MakeFacialFeatures();
            // create a common Decorator for show/hide/reset
            EmoticonGenerator.EmojiDecorator commonDecorator=new EmoticonGenerator.CommonDecorator(faceemoji);
            
            // new a session for undo/redo
            EmoticonGenerator.Sessions session = new EmoticonGenerator.Sessions();
   
            string pdirection="";
            string pvalue="";
            string pstyle="";
            string paction="";
            string pactionReverse="";
            Console.WriteLine("Please input command, or input help for help.");

            while(true)
            {
                //read command line to command string
                String command=UI1();
                //if have any input
                if(command.Length>0)
                {
                    paction=command.Split(' ')[0].ToLower();
                    //check input value
                    switch (paction)
                    {
                        //if it is help command, means help
                        case "help":
                            Console.WriteLine(@"        show    { left-eye | right-eye | left-brow | right-brow | mouth } ");
                            Console.WriteLine(@"        hide    { left-eye | right-eye | left-brow | right-brow | mouth }");
                            Console.WriteLine(@"        move    { left-eye | right-eye | left-brow | right-brow | mouth } { up | down | left | right } value");
                            Console.WriteLine(@"        reset   { left-eye | right-eye | left-brow | right-brow | mouth }");
                            Console.WriteLine(@"        style   { left-eye | right-eye | left-brow | right-brow | mouth } { A | B | C }");
                            Console.WriteLine(@"        save    { <file> }");
                            Console.WriteLine(@"        draw");
                            Console.WriteLine(@"        undo");
                            Console.WriteLine(@"        redo");
                            Console.WriteLine(@"        help");
                            Console.WriteLine(@"        quit");
                            //return;
                            break;
                        case "show": case "hide": case "reset": 
                            if(paction=="hide"||paction=="reset")
                            {
                                pactionReverse="show";
                            }
                            else
                            {
                                pactionReverse="hide";
                            }
                            // have more thant 1 parameter
                            if(command.Split(' ').Length>1)
                            {
                                var ft=command.Split(' ')[1].ToLower();
                                if(ft=="left-brow"||ft=="right-brow"||ft=="left-eye"||ft=="right-eye"||ft=="mouth")
                                {
                                    switch(ft)
                                    {
                                        // do Generator
                                        case "left-brow":
                                            emojidecorator=new EmoticonGenerator.LeftBrowDecorator(commonDecorator);                                         
                                            break; 
                                        case "right-brow":
                                            emojidecorator=new EmoticonGenerator.RightBrowDecorator(commonDecorator);
                                            break;
                                        case "left-eye":
                                            emojidecorator=new EmoticonGenerator.LeftEyeDecorator(commonDecorator);
                                            break;
                                        case "right-eye":
                                            emojidecorator=new EmoticonGenerator.RightEyeDecorator(commonDecorator);
                                            break;
                                        case "mouth":
                                            emojidecorator=new EmoticonGenerator.MouthDecorator(commonDecorator);
                                            break; 
                                        /*       
                                        case "right-brow":
                                            emojidecorator=new EmoticonGenerator.RightBrowDecorator(faceemoji);
                                            break;
                                        case "left-eye":
                                            emojidecorator=new EmoticonGenerator.LeftEyeDecorator(faceemoji);
                                            break;
                                        case "right-eye":
                                            emojidecorator=new EmoticonGenerator.RightEyeDecorator(faceemoji);
                                            break;
                                        case "mouth":
                                            emojidecorator=new EmoticonGenerator.MouthDecorator(faceemoji);
                                            break; 
                                        */
                                        default: 
                                            break;                                                                                                                     
                                    }
                                    emojidecorator.action=paction;
                                    emojidecorator.actionReverse=pactionReverse;
                                    emojidecorator.actionForward=paction;
                                    session.Action(new EmoticonGenerator.AddCommand(emojidecorator));
                                }
                            }
                            // if any string not match 
                            else
                            {
                                Console.WriteLine("The input is incorrect, please input H for help ");
                            } 
                            break;
                        
                        case "move": 
                            // have more thant 1 parameter
                            if(command.Split(' ').Length>1)
                            {
                                
                                // if the number of parameter equals 4, which means pass a custom attribute, like up,down etc.
                                if(command.Split(' ').Length==4)
                                {
                                    pdirection=command.Split(' ')[2].ToLower();
                                    pvalue=command.Split(' ')[3].ToLower();
                                }
                                
                                if(pdirection=="up"||pdirection=="down"||pdirection=="left"||pdirection=="right")    
                                {
                                    switch(paction+pdirection)
                                    {
                                        case "moveup":
                                            pactionReverse="movedown";
                                            break;
                                        case "movedown":
                                            pactionReverse="moveup";
                                            break;
                                        case "moveleft":
                                            pactionReverse="moveright";
                                            break;
                                        case "moveright":
                                            pactionReverse="moveleft";
                                            break;
                                        default:
                                            break;   
                                    }
                                    // input string to lower after move and switch
                                    switch(command.Split(' ')[1].ToLower())
                                    {
                                        case "left-brow":
                                            emojidecorator=new EmoticonGenerator.LeftBrowDecorator(faceemoji);  
                                            break;
                                        case "right-brow":
                                            emojidecorator=new EmoticonGenerator.RightBrowDecorator(faceemoji);  
                                            break;
                                        case "left-eye":
                                            emojidecorator=new EmoticonGenerator.LeftEyeDecorator(faceemoji);  
                                            break;
                                        case "right-eye":
                                            emojidecorator=new EmoticonGenerator.RightEyeDecorator(faceemoji);  
                                            break;
                                        case "mouth":
                                            emojidecorator=new EmoticonGenerator.MouthDecorator(faceemoji);  
                                            break;    
                                        default: 
                                            break;                                                                                                                     
                                    }
                                    emojidecorator.action=paction+pdirection;
                                    emojidecorator.actionReverse=pactionReverse;
                                    emojidecorator.actionForward=paction+pdirection;
                                    emojidecorator.dvalue=Int32.Parse(pvalue);
                                    //emojidecorator.direction=pdirection;
                                    session.Action(new EmoticonGenerator.AddCommand(emojidecorator));
                                }
                                else
                                {
                                    Console.WriteLine("The input is incorrect, please input H for help ");
                                }
                            }
                            // any string not match 
                            else
                            {
                                Console.WriteLine("The input is incorrect, please input H for help ");
                            } 
                            break;
                        
                        case "style":
                            // have more thant 1 parameter
                            if(command.Split(' ').Length>1)
                            {
                                // if the number of parameter equals 3, which means pass a custom attribute, like A,B etc.
                                if(command.Split(' ').Length==3)
                                {
                                    pstyle=command.Split(' ')[2].ToUpper();
                                }
                                if(pstyle=="A"||pstyle=="B"||pstyle=="C")
                                {      
                                    // change input string to lower after style and switch
                                    switch(command.Split(' ')[1].ToLower())
                                    {
                                        case "left-brow":
                                            emojidecorator=new EmoticonGenerator.LeftBrowDecorator(faceemoji);  
                                            break;
                                        case "right-brow":
                                            emojidecorator=new EmoticonGenerator.RightBrowDecorator(faceemoji);  
                                            break;
                                        case "left-eye":
                                            emojidecorator=new EmoticonGenerator.LeftEyeDecorator(faceemoji);  
                                            break;
                                        case "right-eye":
                                            emojidecorator=new EmoticonGenerator.RightEyeDecorator(faceemoji);  
                                            break;
                                        case "mouth":
                                            emojidecorator=new EmoticonGenerator.MouthDecorator(faceemoji);  
                                            break; 
                                        default: 
                                            break;                                                                                                                     
                                    }
                                    emojidecorator.actionForward="Style "+pstyle;
                                    emojidecorator.action="Style "+pstyle;
                                    emojidecorator.styleType="Style "+pstyle;
                                    session.Action(new EmoticonGenerator.AddCommand(emojidecorator));
                                }
                                else
                                {
                                    Console.WriteLine("The input is incorrect, please input H for help ");
                                }
                            }
                            // if any string not match 
                            else
                            {
                                Console.WriteLine("The input is incorrect, please input H for help ");
                            } 
                            break;
                        case "undo":    
                            // undo command
                            session.Undo();
                            break;
                        case "redo":
                            // redo command
                            session.Redo();   
                            break;
                        case "quit":
                            //quit
                            Console.WriteLine("Goodbye!");
                            return;
                        case "draw":
                            Console.WriteLine(faceemoji.Show());
                            break;
                        case "save":
                            //output the emoji content to file emoji.svg
                            File.WriteAllText("emoji.svg",faceemoji.Show());  
                            // show message    
                            Console.WriteLine("Emoticon saved to file: emoji.svg");
                            break;
                        default: 
                            Console.WriteLine("The input is incorrect, please input H for help ");
                            break;
                    }
                }
                pstyle="";
                paction="";
                pactionReverse="";
                pdirection="";
                pvalue="";
            }   
        }
        
    }
}

