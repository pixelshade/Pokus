// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.17020
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Pokus {
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    
    
    public partial class LoginPage : ContentPage {
        
        private Label valueLabel;
        
        private Entry usernameEntry;
        
        private Entry passwordEntry;
        
        private void InitializeComponent() {
            this.LoadFromXaml(typeof(LoginPage));
            valueLabel = this.FindByName <Label>("valueLabel");
            usernameEntry = this.FindByName <Entry>("usernameEntry");
            passwordEntry = this.FindByName <Entry>("passwordEntry");
        }
    }
}
