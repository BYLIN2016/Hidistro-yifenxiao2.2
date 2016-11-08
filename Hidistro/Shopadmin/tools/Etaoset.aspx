<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Etaoset.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.Etaoset"
MasterPageFile="~/Shopadmin/ShopAdmin.Master"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title m_none m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>
                生成一淘（www.etao.com）商品FEED设置</h1>
            <span>您可以申请独立商城入驻一淘（open.etao.com）,并通过这里的设置将独立商城的商品及分类信息生成Feed文件，提交到一淘（open.etao.com）收录商品和分类信息。</span>
        </div>
        <div>   
        <div align="center" style="width:90%"> <asp:Button ID="BtnCreateEtao"  
                Visible="false"  CssClass="submit_DAqueding "  runat="server" Text="申请开通一淘" 
                onclick="BtnCreateEtao_Click" />&nbsp;&nbsp;&nbsp; <asp:Label
            ID="lbEtaoCreate" runat="server" Text=""  ForeColor="Red"  Visible="false" ></asp:Label></div>
        <div class="datafrom"  id="etaoset" runat="server" visible="false">  
       
            <div class="formitem" >
                <ul>
                    <li><span>请上传一淘TXT验证文件：</span>
                        <asp:FileUpload ID="fudVerifyFile" runat="server" CssClass="forminput" />
                        &nbsp;<asp:Button ID="btnUpoad" runat="server" Text="上传" CssClass="submit_queding" OnClick="btnUpoad_Click" />
                        <p id="ctfile">
                            请上传商家在一淘（etao.com）网站申请入驻时，进行域名审核验证时下载的验证文件。</p>
                    </li>
                    <li><span>请输入一淘帐号：<em>*</em></span>
                        <asp:TextBox ID="txtEtaoID" runat="server" CssClass="forminput" />
                        <p id="txtEtaoIDTip">
                            必填,商家申请入驻一淘（etao.com）网站时使用的账号，1-60个字符</p>
                    </li>
                    <li><span>是否开启:</span>
                        <Hi:YesNoRadioButtonList ID="rdobltIsCreateFeed" SelectedValue="false" RepeatLayout="Flow"
                            runat="server" />
                            <p>选择是否开启生成供一淘（Etao.com）搜索引擎自动抓取的商品FEED文件，默认不开启。</p>
                    </li>
                    <li runat="server"  id="incDir"><span>增量索引目录:</span><asp:Label runat="server" ID="lblEtaoFeedInc" />
                      <p>供一淘（Etao.com）搜索引擎自动抓取的增量商品FEED文件的存放目录，每隔30分钟会抓取一次。</p>
   
                    </li>
                    <li runat="server"  id="fulDir"><span>全量索引目录:</span><asp:Label runat="server" ID="lblEtaoFeedFull" />
                                   <p>供一淘（Etao.com）搜索引擎自动抓取的全量商品FEED文件的存放目录，每天凌晨会抓取一次。</p>
                                   </li>
                </ul>
                <div style="clear: both">
                </div>
                <ul class="btntf Pa_140" style="margin: 5px 0px; height: 30px;">
                    <asp:Button ID="btnChangeEmailSettings" runat="server" Text="保 存" 
                        CssClass="submit_DAqueding float" onclick="btnChangeEmailSettings_Click">
                    </asp:Button>
                </ul>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
