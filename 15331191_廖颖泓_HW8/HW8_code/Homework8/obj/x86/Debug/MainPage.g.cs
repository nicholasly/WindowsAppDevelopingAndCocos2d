﻿#pragma checksum "C:\Users\DELL\documents\visual studio 2015\Projects\Homework8\Homework8\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "648B3D2C78738874EB2502522183B5DD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Homework8
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.Play = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 13 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.Play).Click += this.Play_Click;
                    #line default
                }
                break;
            case 2:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element2 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 14 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element2).Click += this.Stop_Click;
                    #line default
                }
                break;
            case 3:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element3 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 15 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element3).Click += this.FullScreen_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.Display = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    #line 51 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.Display).MediaOpened += this.Display_MediaOpened;
                    #line 51 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.Display).MediaEnded += this.Display_MediaEnded;
                    #line 51 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.Display).CurrentStateChanged += this.Display_CurrentStateChanged;
                    #line default
                }
                break;
            case 5:
                {
                    this.Position = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 48 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.Position).ValueChanged += this.SeekToMediaPosition;
                    #line default
                }
                break;
            case 6:
                {
                    this.Volume = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 49 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.Volume).ValueChanged += this.Volume_ValueChanged;
                    #line default
                }
                break;
            case 7:
                {
                    this.Value = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 32 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Value).KeyDown += this.Go_KeyDown;
                    #line default
                }
                break;
            case 8:
                {
                    this.Select = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 33 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.Select).Click += this.Select_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

