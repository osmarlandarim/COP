﻿

#pragma checksum "G:\CopRastreioObjetoFonte\COPRastreioObjeto\COPRastreioObjeto\COPRastreioObjeto\GroupDetailPageEndereco.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4227B90D9EB06DB30A0916B16004D843"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace COPRastreioObjeto
{
    partial class GroupDetailPageEndereco : global::COPRastreioObjeto.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 107 "..\..\..\GroupDetailPageEndereco.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 195 "..\..\..\GroupDetailPageEndereco.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 152 "..\..\..\GroupDetailPageEndereco.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnAdicionar_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 121 "..\..\..\GroupDetailPageEndereco.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).LostFocus += this.txtCEP_LostFocus;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


