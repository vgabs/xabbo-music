﻿#pragma checksum "..\..\..\..\Controls\NoteHolder.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2B075F670BB2C0D1EF5004375E5F45E941A4F1FA"
//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using xabbo_music.Controls;


namespace xabbo_music.Controls {
    
    
    /// <summary>
    /// NoteHolder
    /// </summary>
    public partial class NoteHolder : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\Controls\NoteHolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border MainBorder;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\Controls\NoteHolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border HighlightBorder;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\Controls\NoteHolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border DarkHighlightBorder;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Controls\NoteHolder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TB_Note;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/xabbo-music;component/controls/noteholder.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\NoteHolder.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainBorder = ((System.Windows.Controls.Border)(target));
            
            #line 9 "..\..\..\..\Controls\NoteHolder.xaml"
            this.MainBorder.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(this.Control_PreviewMouseMove);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\..\Controls\NoteHolder.xaml"
            this.MainBorder.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseUp);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\..\Controls\NoteHolder.xaml"
            this.MainBorder.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Control_MouseLeave);
            
            #line default
            #line hidden
            
            #line 9 "..\..\..\..\Controls\NoteHolder.xaml"
            this.MainBorder.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.HighlightBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.DarkHighlightBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            this.TB_Note = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

