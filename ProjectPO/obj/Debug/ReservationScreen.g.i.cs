﻿#pragma checksum "..\..\ReservationScreen.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "101FCA00091A6C30ED0276122EA45469B59514FDAE59FF59BB20A87D40D89E34"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using ProjectPO;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace ProjectPO {
    
    
    /// <summary>
    /// ReservationScreen
    /// </summary>
    public partial class ReservationScreen : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 58 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxName;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxLastName;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxEmailAddress;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxPhoneNumber;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxRooms;
        
        #line default
        #line hidden
        
        
        #line 165 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Calendar CheckInCalendar;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Calendar CheckOutCalendar;
        
        #line default
        #line hidden
        
        
        #line 212 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxBoards;
        
        #line default
        #line hidden
        
        
        #line 225 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonBook;
        
        #line default
        #line hidden
        
        
        #line 270 "..\..\ReservationScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LabelTotalPrice;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProjectPO;component/reservationscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ReservationScreen.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 29 "..\..\ReservationScreen.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Grid_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TextBoxName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.TextBoxLastName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.TextBoxEmailAddress = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.TextBoxPhoneNumber = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.ComboBoxRooms = ((System.Windows.Controls.ComboBox)(target));
            
            #line 151 "..\..\ReservationScreen.xaml"
            this.ComboBoxRooms.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxRooms_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CheckInCalendar = ((System.Windows.Controls.Calendar)(target));
            
            #line 172 "..\..\ReservationScreen.xaml"
            this.CheckInCalendar.SelectedDatesChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.CheckInCalendar_SelectedDatesChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CheckOutCalendar = ((System.Windows.Controls.Calendar)(target));
            
            #line 197 "..\..\ReservationScreen.xaml"
            this.CheckOutCalendar.SelectedDatesChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.CheckOutCalendar_SelectedDatesChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ComboBoxBoards = ((System.Windows.Controls.ComboBox)(target));
            
            #line 221 "..\..\ReservationScreen.xaml"
            this.ComboBoxBoards.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxBoards_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.ButtonBook = ((System.Windows.Controls.Button)(target));
            
            #line 231 "..\..\ReservationScreen.xaml"
            this.ButtonBook.Click += new System.Windows.RoutedEventHandler(this.ButtonBook_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.LabelTotalPrice = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

