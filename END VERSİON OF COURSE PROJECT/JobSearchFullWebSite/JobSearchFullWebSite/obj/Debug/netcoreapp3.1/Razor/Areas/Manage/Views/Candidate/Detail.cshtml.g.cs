#pragma checksum "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ffe5f61897a6c249ef322cad6a579a7a401c1a8f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Manage_Views_Candidate_Detail), @"mvc.1.0.view", @"/Areas/Manage/Views/Candidate/Detail.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
using JobSearchFullWebSite.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
using JobSearchFullWebSite.Enums;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
using System.ComponentModel;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ffe5f61897a6c249ef322cad6a579a7a401c1a8f", @"/Areas/Manage/Views/Candidate/Detail.cshtml")]
    public class Areas_Manage_Views_Candidate_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Candidate>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 5 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
  
    ViewData["Title"] = "Detail";
    Layout = "~/Areas/Manage/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"card mx-5 my-5 p-3\">\r\n    <div class=\"d-flex flex-row justify-content-center\"><h2> ");
#nullable restore
#line 11 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
                                                        Write(Model.FullName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2></div>\r\n");
#nullable restore
#line 12 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
      
        DescriptionAttribute a = (DescriptionAttribute)(((typeof(Gender)).GetField(Model.Gender.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), true))[0];

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"gender\">Cinsi: ");
#nullable restore
#line 14 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
                              Write(a.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
            WriteLiteral("    <div class=\"birthdate\">Doğum tarixi: ");
#nullable restore
#line 16 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
                                    Write(Model.BirthdayDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    <div class=\"age\">Yaşı: ");
#nullable restore
#line 17 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
                      Write(Model.Age);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    <div class=\"createdAt\">Yaradıldı: ");
#nullable restore
#line 18 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
                                 Write(Model.CreatedAt.ToString("MM.dd.yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    <div class=\"Email\">Email: ");
#nullable restore
#line 19 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
                         Write(Model.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    <div class=\"PhoneNumber\">Əlaqə: ");
#nullable restore
#line 20 "D:\Flescard\END VERSİON OF COURSE PROJECT\JobSearchFullWebSite\JobSearchFullWebSite\Areas\Manage\Views\Candidate\Detail.cshtml"
                               Write(Model.PhoneNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Candidate> Html { get; private set; }
    }
}
#pragma warning restore 1591
